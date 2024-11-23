using RedMoonGames.Window;
using Scripts.UI.Dialogue;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class DialogueSystem : MonoBehaviour
{
	[SerializeField] private WindowHandler _dialogueWindow;
	
	private QuestDatabase _data;

	private QuestQuestionsModel _currentQuestion;
	private bool _waitAnswer;
	private bool _dialogueGone;

	private List<string> _phrases;
	private List<QuestQuestionsModel> _questions;
	private List<QuestEndingsModel> _endings;
	private int _index;

	private InteractionSystem _interactionSystem;
	private WindowService _windowService;
	private QuestSystem _questSystem;

	public event Action<string, bool> OnOpenWindow;
	public event Action<string> OnShowPhrase;
	public event Action<string, string> OnWaitAnswer;
	public event Action OnLastPhrase;
	public event Action OnDialogueEnd;

	[Inject]
	private void Construct(WindowService windowService, QuestSystem questSystem, InteractionSystem interactionSystem)
	{
		_interactionSystem = interactionSystem;
		_windowService = windowService;
		_questSystem = questSystem;
	}

	private void Awake()
	{
		_questSystem.SetData += SetDialogue;
		_interactionSystem.OnResetDialogue += ResetDialogue;
		_interactionSystem.OnStartInteract += OpenDialogue;
	}
	
	private void OnDestroy()
	{
		_questSystem.SetData -= SetDialogue;
		_interactionSystem.OnResetDialogue -= ResetDialogue;
		_interactionSystem.OnStartInteract -= OpenDialogue;
	}

	private void SetDialogue(QuestDatabase data, string state)
	{
		_data = data;
		var model = data.GetModelByState(state);

		_index = _data.Index;
		_phrases = model.Phrases.ToList();
		_questions = model.Questions.ToList();
		_endings = model.Endings.ToList();
	}

	private void OpenDialogue(string name, bool canTrade)
	{
		_windowService.TryOpenWindow(_dialogueWindow);
		_windowService.ShowWindow();
		
		OnOpenWindow?.Invoke(name, canTrade);
		SetDefaultState();
		ShowNextPhrase();
	}

	private void ResetDialogue()
	{
		SetDefaultState();
		
		_data = null;
		_phrases = null;
		_questions = null;
		_endings = null;
	}

	public void ShowNextPhrase()
	{
		if (_waitAnswer) return;

		if (_dialogueGone)
		{
			DialogueEnd();
			return;
		}
		_data.Index = _index;
		OnShowPhrase?.Invoke(_phrases[_index]);
	}	

	public void OnPhraseShowEnd()
	{		
		foreach (var end in _endings) 
		{
			if (end.Index == _index)
			{
				_dialogueGone = true;

				OnLastPhrase?.Invoke();
				_questSystem.ChangeQuestState(_data, end.State);
				return;
			}
		}

		for (int index = 0; index < _questions.Count; index ++)
		{
			if (_questions[index].PhraseIndex == _index)
			{
				_waitAnswer = true;

				var positive = _questions[index].Answer1;  
				var negative = _questions[index].Answer2;
				_currentQuestion = _questions[index];

				OnWaitAnswer?.Invoke(positive, negative);
				return;
			}
		}
		_index++;
	}

	public void DialogueAnswer(bool answer)
	{
		if (_waitAnswer)
		{
			_waitAnswer = false;

			if (answer) _index = _currentQuestion.Index1;
			else _index = _currentQuestion.Index2;

			ShowNextPhrase();
		}
	}

	private void SetDefaultState()
	{
		_waitAnswer = false;
		_dialogueGone = false;
	}

	public void SaveDialogueIndex()
	{
		_data.Index = 0;
	}

	public void DialogueEnd()
	{
		SetDefaultState();

		OnDialogueEnd?.Invoke();
		_questSystem.UpdateData(_data);
	}
}
