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
		[SerializeField] private Button _tradeButton;

		[Header("Next Button Texts")]
		[SerializeField] private GameObject _next;
		[SerializeField] private GameObject _skip;
		[SerializeField] private GameObject _leave;

		[Header("Interactive Keys")]
		[SerializeField] private Button _positiveButton;
		[SerializeField] private Button _negativeButton;

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
			_interactionSystem.OnStartInteract += ShowDialogue;
			_interactionSystem.CanTrade += EnableTradeButton;
			_dialogueSystem.OnDialogueEnd += HideDialogue;
			_dialogueSystem.OnShowPhrase += AnimatePhrase;
			_dialogueSystem.OnWaitAnswer += WaitAnswer;
			_dialogueSystem.OnDialogueGone += LastPhrase;
			_dialogueAnimation.TextAnimationEnd += PhraseAnimationEnd;
			
			_tradeButton.gameObject.SetActive(false);
		}

		private void ButtonPositive() { DialogueAnswer(true); }
		private void ButtonNegative() { DialogueAnswer(false); }
		private void NextButton() { _dialogueSystem.ShowNextPhrase(); }

		private void EnableTradeButton() 
		{
			_tradeButton.onClick.AddListener(_interactionSystem.OpenTradeWindow);
			_tradeButton.gameObject.SetActive(true);
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
		}

		private void ShowDialogue()
		{
			_uiActions.OnBackKeyPressed += EndDialogue;

			OnSwitchDialogue?.Invoke(true);

			_hudController.SwitchOverlay();
			_dialogueAnimation.InteractionKey();
			_dialogueAnimation.ShowWindow();
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
			_uiActions.OnBackKeyPressed -= EndDialogue;

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

		private void DialogueAnswer(bool answer)
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

			_dialogueSystem.DialogueEnd();
		}

		private void EnterTrigger(string name)
		{
			_dialogueAnimation.InteractionKey(1);
			SetName(name);

			_nextButton.onClick.AddListener(NextButton);
			_backButton.onClick.AddListener(EndDialogue);
		}

		private void ExitTrigger()
		{
			_dialogueAnimation.AnswerKeys();
			_dialogueAnimation.HideWindow();
			_dialogueAnimation.InteractionKey(0);
			_hudController.SwitchOverlay(1);

			_nextButton.onClick.RemoveListener(NextButton);
			_backButton.onClick.RemoveListener(EndDialogue);
		}

		private void OnDestroy()
		{
			_interactionSystem.OnExitTrigger -= ExitTrigger;
			_interactionSystem.OnEnterTrigger -= EnterTrigger;
			_interactionSystem.OnStartInteract -= ShowDialogue;
			_interactionSystem.CanTrade -= EnableTradeButton;
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
			_tradeButton.onClick.RemoveListener(_interactionSystem.OpenTradeWindow);
		}
	}
}