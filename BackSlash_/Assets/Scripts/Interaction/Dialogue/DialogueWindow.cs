using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace RedMoonGames.Window
{
	public class DialogueWindow : BasicInteractionWindow
	{	
		[SerializeField] private WindowHandler _tradeHandler;
		[SerializeField] private DialogueAnimator _dialogueAnimator;
		
		[Header("Buttons")]
		[SerializeField] private Button _nextButton;
		[SerializeField] private Button _tradeButton;
		[SerializeField] private Button _leaveButton;
		[Space]
		[SerializeField] private TMP_Text _nextButtonTMP;
		
		[Header("Next Button Texts")]
		[SerializeField] private string _nextText = "NEXT PHRASE";
		[SerializeField] private string _skipText = "SKIP";

		[Header("Answers")]
		[SerializeField] private Button _positiveButton;
		[SerializeField] private Button _negativeButton;
		[SerializeField] private TMP_Text _positiveAnswer;
		[SerializeField] private TMP_Text _negativeAnswer;

		private bool _dialodueEnd;

		private DialogueSystem _dialogueSystem;

		[Inject]
		private void Construct(DialogueSystem dialogueSystem)
		{
			_dialogueSystem = dialogueSystem;
		}

		private void Awake()
		{
			_leaveButton.gameObject.SetActive(false);
		}
		
		protected override void OnEnable()
		{
			base.OnEnable();
			
			_uiInputs.OnBackKeyPressed += LeaveButton;
			_uiInputs.OnEnterKeyPressed += CallNextPhrase;
			_uiInputs.OnTradeKeyPressed += TradeButton;
			_uiInputs.OnDialogueAnswer += DialogueAnswer;
			
			_dialogueSystem.OnOpenWindow += SetDialogue;
			_dialogueSystem.OnShowPhrase += ShowPhrase;
			_dialogueSystem.OnWaitAnswer += WaitAnswer;
			_dialogueSystem.OnLastPhrase += LastPhrase;
			
			_dialogueAnimator.TextAnimationEnd += PhraseAnimationEnd;
			
			_positiveButton.onClick.AddListener(ButtonPositive);
			_negativeButton.onClick.AddListener(ButtonNegative);
			_nextButton.onClick.AddListener(NextButton);
			_tradeButton.onClick.AddListener(TradeButton);
			_leaveButton.onClick.AddListener(LeaveButton);
		}
		
		protected override void OnDisable()
		{	
			base.OnDisable();
			
			_uiInputs.OnBackKeyPressed -= LeaveButton;
			_uiInputs.OnEnterKeyPressed -= CallNextPhrase;
			_uiInputs.OnTradeKeyPressed -= TradeButton;
			_uiInputs.OnDialogueAnswer -= DialogueAnswer;
			
			_dialogueSystem.OnOpenWindow -= SetDialogue;
			_dialogueSystem.OnShowPhrase -= ShowPhrase;
			_dialogueSystem.OnWaitAnswer -= WaitAnswer;
			_dialogueSystem.OnLastPhrase -= LastPhrase;
			
			_dialogueAnimator.TextAnimationEnd -= PhraseAnimationEnd;
			
			_positiveButton.onClick.RemoveListener(ButtonPositive);
			_negativeButton.onClick.RemoveListener(ButtonNegative);
			_nextButton.onClick.RemoveListener(NextButton);
			_tradeButton.onClick.RemoveListener(TradeButton);
			_leaveButton.onClick.RemoveListener(LeaveButton);
		}
	
		private void ButtonPositive() { DialogueAnswer(true); }
		private void ButtonNegative() { DialogueAnswer(false); }
		private void NextButton() { _interactionSystem.TryStartInteract(); }
		private void LeaveButton() { if (_dialodueEnd) HideWindow(); }
		private void TradeButton() { if (!_dialogueAnimator.TextActive()) OpenWindow(_tradeHandler); }
		
		private void SetDialogue(string name, bool canTrade)
		{
			_dialogueAnimator.SetName(name);
			
			if (canTrade) _tradeButton.gameObject.SetActive(true);
			else _tradeButton.gameObject.SetActive(false);
		}
		
		private void CallNextPhrase()
		{
			_dialogueSystem.ShowNextPhrase();
		}
		
		private void ShowPhrase(string phrase)
		{
			if (_dialogueAnimator.TextActive())
			{
				_dialogueAnimator.ShowWholePhrase(phrase);
			}
			else
			{
				_nextButtonTMP.text = _skipText;
				_dialogueAnimator.PhraseAnimation(phrase);
			}
		}
		
		private void PhraseAnimationEnd()
		{
			_nextButtonTMP.text = _nextText;
			_dialogueSystem.OnPhraseShowEnd();
		}

		private void LastPhrase()
		{
			if (!_dialodueEnd)
			{
				_dialodueEnd = true;
				
				_nextButton.gameObject.SetActive(false);
				_leaveButton.gameObject.SetActive(true);
			}
		}

		private void WaitAnswer(string positive, string negative)
		{
			_positiveAnswer.text = positive;
			_negativeAnswer.text = negative;
			
			_dialogueAnimator.ShowAnswerKeys();
		}

		private void DialogueAnswer(bool answer)
		{
			_dialogueAnimator.HideAnswerKeys();
			_dialogueSystem.DialogueAnswer(answer);
		}
	}
}
