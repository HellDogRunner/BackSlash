using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Scripts.Player
{
    [RequireComponent(typeof(CanvasGroup))]
    public class DialogueWindow : MonoBehaviour
    {
        [SerializeField] private DialogueAnimation _dialogueAnimation;

        [Header("Buttons")]
        [SerializeField] private Button _backButton;
        [SerializeField] private Button _nextButton;

        [Header("Interactive Keys")]
        [SerializeField] private Button _positiveButton;
        [SerializeField] private Button _negativeButton;

        private bool canShow = true;

        private HUDController _hudController;
        private InteractionSystem _interactionSystem;
        private DialogueSystem _dialogueSystem;
        private UIActionsController _uiActions;

        public event Action<bool> OnSwitchDialogue;

        [Inject]
        private void Construct(HUDController hudController, InteractionSystem interactionSystem, UIActionsController uiActions, DialogueSystem dialogueSystem)
        {
            _interactionSystem = interactionSystem;
            _hudController = hudController;
            _dialogueSystem = dialogueSystem;
            _uiActions = uiActions;
        }

        private void Awake()
        {
            _interactionSystem.OnExitTrigger += ExitTrigger;
            _interactionSystem.OnEnterTrigger += EnterTrigger;
            _interactionSystem.OnInteract += ShowDialogue;
            _dialogueSystem.OnDialogueEnd += HideDialogue;
            _dialogueSystem.OnSetModel += SetName;
            _dialogueSystem.OnShowPhrase += AnimatePhrase;
            _dialogueSystem.OnWaitAnswer += WaitAnswer;
            _dialogueAnimation.AnimationEnd += PhraseAnimationEnd;

            _dialogueAnimation.AnswerKeys();
            _dialogueAnimation.Window();
            _dialogueAnimation.InteractionKey();
        }

        private void ButtonPositive() { DialogueAnswer(true); }
        private void ButtonNegative() { DialogueAnswer(false); }

        private void NextButton()
        {
            _dialogueSystem.ShowNextPhrase();
            canShow = true;
        }

        private void SetName(string name) 
        {
            _dialogueAnimation.Name.text = name; 
            _dialogueAnimation._firstPhrase = true;
        }

        private void AnimatePhrase(string phrase) 
        { 
            if (_dialogueAnimation._text.IsActive())
            {
                _dialogueAnimation.ShowWholePhrase(phrase);
            }
            else _dialogueAnimation.PhraseAnimation(phrase);
        }

        private void PhraseAnimationEnd()
        {
            _dialogueSystem.OnPhraseShowEnd();
        }

        private void ShowDialogue()
        {
            if (canShow)
            {
                OnSwitchDialogue?.Invoke(false);

                _hudController.SwitchOverlay();
                _dialogueAnimation.InteractionKey();
                _dialogueAnimation.Window(1);
            }
            else
            {
                canShow = true;
            }
        }

        private void HideDialogue()
        {
            canShow = false;

            if (_dialogueAnimation._text.IsActive()) _dialogueAnimation._text.Kill();

            OnSwitchDialogue?.Invoke(true);

            _dialogueAnimation.Window();
            _dialogueAnimation.AnswerKeys();
            _dialogueAnimation.InteractionKey(1);
            _hudController.SwitchOverlay(1);
        }

        private void WaitAnswer()
        {
            _dialogueAnimation.AnswerKeys(1);

            _positiveButton.onClick.AddListener(ButtonPositive);
            _negativeButton.onClick.AddListener(ButtonNegative);
            _uiActions.OnDialogueAnswer += DialogueAnswer;
        }

        private void DialogueAnswer(bool answer)
        {
            _uiActions.OnDialogueAnswer -= DialogueAnswer;
            _positiveButton.onClick.RemoveListener(ButtonPositive);
            _negativeButton.onClick.RemoveListener(ButtonNegative);

            _dialogueAnimation.AnswerKeys();
            _dialogueSystem.DialogueAnswer(answer);
        }

        private void EndDialogue()
        {
            _dialogueAnimation.AnswerKeys();
            HideDialogue();

            canShow = true;

            _dialogueSystem.DialogueEnd();
        }

        private void EnterTrigger()
        {
            _dialogueAnimation.InteractionKey(1);

            _nextButton.onClick.AddListener(NextButton);
            _backButton.onClick.AddListener(EndDialogue);
            _uiActions.OnBackKeyPressed += EndDialogue;
        }

        private void ExitTrigger()
        {
            _dialogueAnimation.AnswerKeys();
            _dialogueAnimation.Window();
            _dialogueAnimation.InteractionKey(0);
            _hudController.SwitchOverlay(1);

            _nextButton.onClick.RemoveListener(NextButton);
            _backButton.onClick.RemoveListener(EndDialogue);
            _uiActions.OnBackKeyPressed -= EndDialogue;
        }

        private void OnDestroy()
        {
            _interactionSystem.OnExitTrigger -= ExitTrigger;
            _interactionSystem.OnEnterTrigger -= EnterTrigger;
            _interactionSystem.OnInteract -= ShowDialogue;
            _dialogueSystem.OnDialogueEnd -= HideDialogue;
            _dialogueSystem.OnSetModel -= SetName;
            _dialogueSystem.OnShowPhrase -= AnimatePhrase;
            _dialogueSystem.OnWaitAnswer -= WaitAnswer;
            _uiActions.OnDialogueAnswer -= DialogueAnswer;
            _uiActions.OnBackKeyPressed -= EndDialogue;
            _dialogueAnimation.AnimationEnd -= PhraseAnimationEnd;

            _positiveButton.onClick.RemoveListener(ButtonPositive);
            _negativeButton.onClick.RemoveListener(ButtonNegative);
            _nextButton.onClick.RemoveListener(_dialogueSystem.ShowNextPhrase);
            _backButton.onClick.RemoveListener(EndDialogue);
        }
    }
}