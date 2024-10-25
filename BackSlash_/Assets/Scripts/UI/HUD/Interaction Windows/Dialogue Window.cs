using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Scripts.Player
{
	public class DialogueWindow : MonoBehaviour
	{
		[Header("Buttons")]
		[SerializeField] private Button _backButton;
		[SerializeField] private Button _nextButton;
		[SerializeField] private Button _tradeButton;

		[Header("Next Button Texts")]
		[SerializeField] private GameObject _next;
		[SerializeField] private GameObject _skip;
		[SerializeField] private GameObject _leave;

		[Header("Answers")]
		[SerializeField] private Button _positiveButton;
		[SerializeField] private Button _negativeButton;
		[SerializeField] private TMP_Text _positiveAnswer;
		[SerializeField] private TMP_Text _negativeAnswer;

		private InteractionAnimator _animator;
		private InteractionSystem _interactionSystem;
		private UIActionsController _uiActions;
		private DialogueSystem _dialogueSystem;

		[Inject]
		private void Construct(UIActionsController uiActions, InteractionAnimator animator, InteractionSystem interactionSystem, DialogueSystem dialogueSystem)
		{
			_interactionSystem = interactionSystem;
			_dialogueSystem = dialogueSystem;
			_animator = animator;
			_uiActions = uiActions;
		}

		private void Awake()
		{
			_interactionSystem.SetInteractionButtons(_backButton, _nextButton, _tradeButton);
			
			_interactionSystem.OnExitTrigger += ExitTrigger;
			_interactionSystem.OnEnterTrigger += EnterTrigger;
			_interactionSystem.ShowDialogue += ShowDialogue;
			_interactionSystem.EndInteracting += HideDialogue;
			_interactionSystem.ShowTrade += HideDialogue;
			_dialogueSystem.OnShowPhrase += AnimatePhrase;
			_dialogueSystem.OnWaitAnswer += WaitAnswer;
			_dialogueSystem.OnDialogueGone += LastPhrase;
			_animator.TextAnimationEnd += PhraseAnimationEnd;
		}

		private void ButtonPositive() { DialogueAnswer(true); }
		private void ButtonNegative() { DialogueAnswer(false); }
		private void EnterTrigger() { _animator.TalkKey(1); }
		private void ExitTrigger() { _animator.TalkKey(0); }
		
		private void AnimatePhrase(string phrase)
		{
			if (_animator.GetTextAnimationActive())
			{
				_animator.ShowWholePhrase(phrase);
			}
			else
			{
				SwitchNextButtonText(true);
				_animator.PhraseAnimation(phrase);
			}
		}
		
		private void PhraseAnimationEnd()
		{
			SwitchNextButtonText(false);
			_dialogueSystem.OnPhraseShowEnd();
		}

		private void ShowDialogue()
		{
			_animator.Dialogue(1);
		}
		
		private void HideDialogue()
		{
			_animator.Dialogue();
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

		private void WaitAnswer(string positive, string negative)
		{
			_positiveButton.onClick.AddListener(ButtonPositive);
			_negativeButton.onClick.AddListener(ButtonNegative);
			_uiActions.OnDialogueAnswer += DialogueAnswer;
			
			_positiveAnswer.text = positive;
			_negativeAnswer.text = negative;
			
			_animator.AnswerKeys(1);
		}

		private void DialogueAnswer(bool answer)
		{
			_positiveButton.onClick.RemoveListener(ButtonPositive);
			_negativeButton.onClick.RemoveListener(ButtonNegative);
			_uiActions.OnDialogueAnswer -= DialogueAnswer;;

			_animator.AnswerKeys();
			_dialogueSystem.DialogueAnswer(answer);
		}

		private void OnDestroy()
		{
			_interactionSystem.OnExitTrigger -= ExitTrigger;
			_interactionSystem.OnEnterTrigger -= EnterTrigger;
			_interactionSystem.ShowDialogue -= ShowDialogue;
			_interactionSystem.EndInteracting -= HideDialogue;
			_interactionSystem.ShowTrade -= HideDialogue;
			_dialogueSystem.OnShowPhrase -= AnimatePhrase;
			_dialogueSystem.OnWaitAnswer -= WaitAnswer;
			_dialogueSystem.OnDialogueGone -= LastPhrase;
			_animator.TextAnimationEnd -= PhraseAnimationEnd;

			_positiveButton.onClick.RemoveListener(ButtonPositive);
			_negativeButton.onClick.RemoveListener(ButtonNegative);
		}
	}
}