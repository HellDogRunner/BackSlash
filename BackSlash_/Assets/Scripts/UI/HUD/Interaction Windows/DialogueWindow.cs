using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace RedMoonGames.Window
{
	public class DialogueWindow : GameBasicWindow
	{	
		[SerializeField] private WindowHandler _tradeHandler;
		[SerializeField] private DialogueAnimator _dialogueAnimator;
		
		[Header("Buttons")]
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

		private bool _waitAnswer;

		private InteractionSystem _interactionSystem;
		private DialogueSystem _dialogueSystem;

		[Inject]
		private void Construct(InteractionSystem interactionSystem, DialogueSystem dialogueSystem)
		{
			_interactionSystem = interactionSystem;
			_dialogueSystem = dialogueSystem;
		}

		protected override void Awake()
		{
			_interactionSystem._windows.Add(gameObject);

			base.Awake();
		}
		
		private void OnEnable()
		{
			_uiInputs.OnEscapeKeyPressed -= Hide;
			
			_uiInputs.OnBackKeyPressed += StopInteract;
			_uiInputs.OnEnterKeyPressed += CallNextPhrase;
			_uiInputs.OnTradeKeyPressed += TradeButton;
			_uiInputs.OnDialogueAnswer += DialogueAnswer;
			
			_dialogueSystem.OnOpenWindow += SetDialogue;
			_dialogueSystem.OnShowPhrase += ShowPhrase;
			_dialogueSystem.OnWaitAnswer += WaitAnswer;
			_dialogueSystem.OnLastPhrase += LastPhrase;
			_dialogueSystem.OnDialogueEnd += StopInteract;
			
			_dialogueAnimator.TextAnimationEnd += PhraseAnimationEnd;
			
			_positiveButton.onClick.AddListener(ButtonPositive);
			_negativeButton.onClick.AddListener(ButtonNegative);
			_nextButton.onClick.AddListener(NextButton);
			_tradeButton.onClick.AddListener(TradeButton);
		}
		
		private void OnDisable()
		{	
			_uiInputs.OnBackKeyPressed -= StopInteract;
			_uiInputs.OnEnterKeyPressed -= CallNextPhrase;
			_uiInputs.OnTradeKeyPressed -= TradeButton;
			_uiInputs.OnDialogueAnswer -= DialogueAnswer;
			
			_dialogueSystem.OnOpenWindow -= SetDialogue;
			_dialogueSystem.OnShowPhrase -= ShowPhrase;
			_dialogueSystem.OnWaitAnswer -= WaitAnswer;
			_dialogueSystem.OnLastPhrase -= LastPhrase;
			_dialogueSystem.OnDialogueEnd -= StopInteract;
			
			_dialogueAnimator.TextAnimationEnd -= PhraseAnimationEnd;
			
			_positiveButton.onClick.RemoveListener(ButtonPositive);
			_negativeButton.onClick.RemoveListener(ButtonNegative);
			_nextButton.onClick.RemoveListener(NextButton);
			_tradeButton.onClick.RemoveListener(TradeButton);
		}
	
		private void ButtonPositive() { DialogueAnswer(true); }
		private void ButtonNegative() { DialogueAnswer(false); }
		private void NextButton() { _interactionSystem.TryStartInteract(); }
		
		private void TradeButton()
		{
			_windowService.TryOpenWindow(_tradeHandler);
			SwitchView();
		}

		private void StopInteract()
		{
			if (!_dialogueAnimator.TextActive())
			{
				_interactionSystem.SetExplore();
				Hide();
			}
		}
		
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
				_nextTMP.text = _skipText;
				_dialogueAnimator.PhraseAnimation(phrase);
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
			_waitAnswer = true;
			_positiveAnswer.text = positive;
			_negativeAnswer.text = negative;
			
			_dialogueAnimator.ShowAnswerKeys();
		}

		private void DialogueAnswer(bool answer)
		{
			if (_waitAnswer)
			{
				_dialogueAnimator.HideAnswerKeys();
				_dialogueSystem.DialogueAnswer(answer);
			}
		}
	}
}
