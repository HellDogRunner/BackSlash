using Scripts.Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace RedMoonGames.Window
{
	public class DialogueWindow : GameBasicWindow
	{	
		[SerializeField] private DialogueAnimator _animator;
		
		[Header("Buttons")]
		[SerializeField] private Button _backButton;
		[SerializeField] private Button _nextButton;
		[SerializeField] private Button _tradeButton;
		[Space]
		[SerializeField] private TMP_Text _nextTMP;
		
		[Header("Next Button Texts")]
		[SerializeField] private string _nextText = "NEXT PHRASE";
		[SerializeField] private string _skipText = "SKIP";
		[SerializeField] private string _leaveText = "LEAVE";

		[Header("Answers")]
		[SerializeField] private Button _positiveButton;
		[SerializeField] private Button _negativeButton;
		[SerializeField] private TMP_Text _positiveAnswer;
		[SerializeField] private TMP_Text _negativeAnswer;

		private InteractionSystem _interactionSystem;
		private UIActionsController _uiActions;
		private DialogueSystem _dialogueSystem;

		[Inject]
		private void Construct(UIActionsController uiActions, InteractionSystem interactionSystem, DialogueSystem dialogueSystem)
		{
			_interactionSystem = interactionSystem;
			_dialogueSystem = dialogueSystem;
			_uiActions = uiActions;
		}

		private void Awake()
		{
			_dialogueSystem.OnShowPhrase += AnimatePhrase;
			_dialogueSystem.OnWaitAnswer += WaitAnswer;
			_dialogueSystem.OnDialogueGone += LastPhrase;
			_animator.TextAnimationEnd += PhraseAnimationEnd;
			_uiActions.OnDialogueAnswer += DialogueAnswer;
			
			_positiveButton.onClick.AddListener(ButtonPositive);
			_negativeButton.onClick.AddListener(ButtonNegative);
			_backButton.onClick.AddListener(BackButton);
			_nextButton.onClick.AddListener(NextButton);
			if (_interactionSystem.GetTraderInventory() != null)
			{
				_tradeButton.onClick.AddListener(TradeButton);
				_tradeButton.gameObject.SetActive(true);
			}
			else _tradeButton.gameObject.SetActive(false);
			
			SetName();
		}

		private void ButtonPositive() { DialogueAnswer(true); }
		private void ButtonNegative() { DialogueAnswer(false); }
		
		private void BackButton()
		{
			Debug.Log("BackButton");
			
			_interactionSystem.TryStopInteract();
		}
		
		private void NextButton()
		{
			Debug.Log("NextButton");
			
			_interactionSystem.TryInteract();
		}
		
		private void TradeButton()
		{
			Debug.Log("TradeButton");
			
			_interactionSystem.OpenWindow();
		}
		
		private void SetName()
		{
			_animator.SetName(_interactionSystem.GetName());
		}
		
		private void AnimatePhrase(string phrase)
		{
			if (_animator.GetTextAnimationActive())
			{
				_animator.ShowWholePhrase(phrase);
			}
			else
			{
				_nextTMP.text = _skipText;
				_animator.PhraseAnimation(phrase);
			}
		}
		
		private void PhraseAnimationEnd()
		{
			_nextTMP.text = _nextText;
			_dialogueSystem.OnPhraseShowEnd();
		}

		private void LastPhrase()
		{
			_nextTMP.text = _leaveText;
		}

		private void WaitAnswer(string positive, string negative)
		{
			_positiveAnswer.text = positive;
			_negativeAnswer.text = negative;
			
			_animator.ShowAnswerKeys();
		}

		private void DialogueAnswer(bool answer)
		{
			_animator.HideAnswerKeys();
			_dialogueSystem.DialogueAnswer(answer);
		}

		private void OnDestroy()
		{
			_dialogueSystem.OnShowPhrase -= AnimatePhrase;
			_dialogueSystem.OnWaitAnswer -= WaitAnswer;
			_dialogueSystem.OnDialogueGone -= LastPhrase;
			_animator.TextAnimationEnd -= PhraseAnimationEnd;
			_uiActions.OnDialogueAnswer += DialogueAnswer;
			
			_positiveButton.onClick.RemoveListener(ButtonPositive);
			_negativeButton.onClick.RemoveListener(ButtonNegative);
			_backButton.onClick.RemoveListener(BackButton);
			_nextButton.onClick.RemoveListener(NextButton);
			if (_interactionSystem.GetTraderInventory() != null)
			{
				_tradeButton.onClick.RemoveListener(TradeButton);
				_tradeButton.gameObject.SetActive(false);
			}
		}
	}
}