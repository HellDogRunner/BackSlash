using Scripts.Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

[RequireComponent(typeof(CanvasGroup))]
public class DialogueWindow : MonoBehaviour
{
    [Header("Text")]
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _phrase;

    [Header("Buttons")]
    [SerializeField] private Button _leaveButton;
    [SerializeField] private Button _nextButton;

    [Header("Interactive Keys")]
    [SerializeField] private Button _talkButton;
    [SerializeField] private Button _positiveButton;
    [SerializeField] private Button _negativeButton;

    [Header("Canvas Groups")]
    [SerializeField] private CanvasGroup _dialogue;
    [SerializeField] private CanvasGroup _buttons;
    [SerializeField] private CanvasGroup _talk;
    [SerializeField] private CanvasGroup _answerKeys;

    private HUDAnimationService _animationService;
    private InteractionSystem _interactionSystem;
    private DialogueSystem _dialogueSystem;
    private UIActionsController _uiActions;

    [Inject]
    private void Construct(HUDAnimationService animationService, InteractionSystem interactionSystem, UIActionsController uiActions, DialogueSystem dialogueSystem)
    {
        _interactionSystem = interactionSystem;
        _animationService = animationService;
        _dialogueSystem = dialogueSystem;
        _uiActions = uiActions;
    }

    private void Awake()
    {
        _interactionSystem.OnExitTrigger += UnsubscribeToActions;
        _interactionSystem.OnEnterTrigger += SubscribeToActions;
        _interactionSystem.OnInteract += ShowDialogue;
        _interactionSystem.OnEndInteract += HideDialogue;
        _dialogueSystem.OnSetModel += SetName;
        _dialogueSystem.OnShowPhrase += SetPhraseText;
        _dialogueSystem.OnWaitAnswer += WaitAnswer;

        HideAllComponents();
    }

    private void SetName(string name) { _name.text = name; }
    private void SetPhraseText(string phrase) { _phrase.text = phrase; }
    private void ButtonPositive() { DialogueAnswer(true); }
    private void ButtonNegative() { DialogueAnswer(false); }

    private void ShowDialogue()
    {
        _animationService.AnimateHide(_talk);
        _animationService.AnimateShow(_buttons);
        _animationService.AnimateShow(_dialogue);
    }

    private void HideDialogue()
    {
        HideAllComponents();
        _animationService.AnimateShow(_talk);
    }

    private void WaitAnswer()
    {
        _animationService.AnimateShow(_answerKeys);

        _positiveButton.onClick.AddListener(ButtonPositive);
        _negativeButton.onClick.AddListener(ButtonNegative);
        _uiActions.OnDialogueAnswer += DialogueAnswer;
    }


    private void DialogueAnswer(bool answer)
    {
        _uiActions.OnDialogueAnswer -= DialogueAnswer;
        _positiveButton.onClick.RemoveListener(ButtonPositive);
        _negativeButton.onClick.RemoveListener(ButtonNegative);

        _animationService.AnimateHide(_answerKeys);
        _dialogueSystem.DialogueAnswer(answer);
    }


    private void EndDialogue()
    {
        HideAllComponents();
        _animationService.AnimateShow(_talk);

        _dialogueSystem.OnDialogueEnd();
    }

    private void HideAllComponents()
    {
        _animationService.AnimateHide(_talk);
        _animationService.AnimateHide(_buttons);
        _animationService.AnimateHide(_answerKeys);
        _animationService.AnimateHide(_dialogue);
    }

    private void SubscribeToActions()
    {
        HideAllComponents();
        _animationService.AnimateShow(_talk);

        _nextButton.onClick.AddListener(_dialogueSystem.ShowNextPhrase);
        _leaveButton.onClick.AddListener(EndDialogue);
        _uiActions.OnBackKeyPressed += EndDialogue;
    }

    private void UnsubscribeToActions()
    {
        HideAllComponents();

        _nextButton.onClick.RemoveListener(_dialogueSystem.ShowNextPhrase);
        _leaveButton.onClick.RemoveListener(EndDialogue);
        _uiActions.OnBackKeyPressed -= EndDialogue;
    }

    private void OnDestroy()
    {
        _interactionSystem.OnExitTrigger -= UnsubscribeToActions;
        _interactionSystem.OnEnterTrigger -= SubscribeToActions;
        _interactionSystem.OnInteract -= ShowDialogue;
        _interactionSystem.OnEndInteract -= HideDialogue;
        _dialogueSystem.OnSetModel -= SetName;
        _dialogueSystem.OnShowPhrase -= SetPhraseText;
        _dialogueSystem.OnWaitAnswer -= WaitAnswer;
        _uiActions.OnDialogueAnswer -= DialogueAnswer;
        _uiActions.OnBackKeyPressed -= EndDialogue;

        _positiveButton.onClick.RemoveListener(ButtonPositive);
        _negativeButton.onClick.RemoveListener(ButtonNegative);
        _nextButton.onClick.RemoveListener(_dialogueSystem.ShowNextPhrase);
        _leaveButton.onClick.RemoveListener(EndDialogue);
    }
}