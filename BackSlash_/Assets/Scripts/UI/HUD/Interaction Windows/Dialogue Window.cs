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

        [Header("Next Button Texts")]
        [SerializeField] private GameObject _next;
        [SerializeField] private GameObject _skip;
        [SerializeField] private GameObject _leave;

        [Header("Interactive Keys")]
        [SerializeField] private Button _positiveButton;
        [SerializeField] private Button _negativeButton;

        [SerializeField] private bool canShow = true;

        private HUDController _hudController;
        private InteractionSystem _interactionSystem;
        private DialogueSystem _dialogueSystem;
        private UIActionsController _uiActions;

        private bool _lastPhrase;

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
            _dialogueSystem.OnShowPhrase += AnimatePhrase;
            _dialogueSystem.OnWaitAnswer += WaitAnswer;
            _dialogueSystem.OnDialogueGone += LastPhrase;
            _dialogueAnimation.TextAnimationEnd += PhraseAnimationEnd;
        }

        private void ButtonPositive() { DialogueAnswer(1); }
        private void ButtonNegative() { DialogueAnswer(2); }

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
            else
            {
                SwitchNextButtonText(true);
                _dialogueAnimation.PhraseAnimation(phrase);
            }
        }

        private void PhraseAnimationEnd()
        {
            SwitchNextButtonText(false);
            _dialogueSystem.OnPhraseShowEnd();
            _lastPhrase = false;
        }

        private void ShowDialogue()
        {
            if (canShow)
            {
                OnSwitchDialogue?.Invoke(true);

                _hudController.SwitchOverlay();
                _dialogueAnimation.InteractionKey();
                _dialogueAnimation.ShowWindow();
            }
            else
            {
                canShow = true;
            }
        }

        private void SwitchNextButtonText(bool textTupping)
        {
            _next.gameObject.SetActive(!textTupping);
            _skip.gameObject.SetActive(textTupping);
            _leave.gameObject.SetActive(false);
        }

        private void LastPhrase()
        {
            _next.gameObject.SetActive(false);
            _leave.gameObject.SetActive(true);
        }

        private void HideDialogue()
        {
            canShow = false;

            if (_dialogueAnimation._text.IsActive()) _dialogueAnimation._text.Kill();

            _interactionSystem.EndInteraction();
            OnSwitchDialogue?.Invoke(false);

            _dialogueAnimation.HideWindow();
            _dialogueAnimation.AnswerKeys();
            _dialogueAnimation.InteractionKey(1);
            _hudController.SwitchOverlay(1);
        }

        private void WaitAnswer(string positive, string negative)
        {
            _dialogueAnimation.SetAnswers(positive, negative);
            _dialogueAnimation.AnswerKeys(1);

            _positiveButton.onClick.AddListener(ButtonPositive);
            _negativeButton.onClick.AddListener(ButtonNegative);
            _uiActions.OnDialogueAnswer += DialogueAnswer;
        }

        private void DialogueAnswer(int answer)
        {
            _uiActions.OnDialogueAnswer -= DialogueAnswer;
            _positiveButton.onClick.RemoveListener(ButtonPositive);
            _negativeButton.onClick.RemoveListener(ButtonNegative);

            _dialogueAnimation.AnswerKeys();
            _dialogueSystem.DialogueAnswer(answer);
        }

        public void OnGamePause(bool enable)
        {
            if (enable) _uiActions.OnBackKeyPressed -= EndDialogue;
            else _uiActions.OnBackKeyPressed += EndDialogue;
        }

        private void EndDialogue()
        {
            _dialogueAnimation.AnswerKeys();
            HideDialogue();

            canShow = true;

            _dialogueSystem.DialogueEnd();
        }

        private void EnterTrigger(string name)
        {
            _dialogueAnimation.InteractionKey(1);
            SetName(name);

            _nextButton.onClick.AddListener(NextButton);
            _backButton.onClick.AddListener(EndDialogue);
            _uiActions.OnBackKeyPressed += EndDialogue;
        }

        private void ExitTrigger()
        {
            _dialogueAnimation.AnswerKeys();
            _dialogueAnimation.HideWindow();
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
            _dialogueSystem.OnShowPhrase -= AnimatePhrase;
            _dialogueSystem.OnWaitAnswer -= WaitAnswer;
            _dialogueSystem.OnDialogueGone -= LastPhrase;
            _dialogueAnimation.TextAnimationEnd -= PhraseAnimationEnd;
            _uiActions.OnDialogueAnswer -= DialogueAnswer;
            _uiActions.OnBackKeyPressed -= EndDialogue;

            _positiveButton.onClick.RemoveListener(ButtonPositive);
            _negativeButton.onClick.RemoveListener(ButtonNegative);
            _nextButton.onClick.RemoveListener(_dialogueSystem.ShowNextPhrase);
            _backButton.onClick.RemoveListener(EndDialogue);
        }
    }
}