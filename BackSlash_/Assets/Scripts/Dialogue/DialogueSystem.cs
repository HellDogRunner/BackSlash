using Scripts.UI.Dialogue;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class DialogueSystem : MonoBehaviour
{
	private QuestDatabase _data;

	private QuestQuestionsModel _currentQuestion;
	private bool _waitAnswer;
	private bool _dialogueGone;
	private bool _textShowing;

	private List<string> _phrases;
	private List<QuestQuestionsModel> _questions;
	private List<QuestEndingsModel> _endings;
	private int _index;

	private InteractionSystem _interactionSystem;
	private QuestSystem _questSystem;

	public event Action<string> OnShowPhrase;
	public event Action<string, string> OnWaitAnswer;
	public event Action OnDialogueGone;

	[Inject]
	private void Construct(QuestSystem questSystem, InteractionSystem interactionSystem)
	{
		_interactionSystem = interactionSystem;
		_questSystem = questSystem;
	}

	private void Awake()
	{
		_questSystem.SetData += SetDialogue;

		_interactionSystem.OnInteracting += ShowNextPhrase;
		_interactionSystem.OnResetDialogue += ResetDialogue;
		_interactionSystem.EndInteracting += DialogueEnd;
	}

	private void SetDialogue(QuestDatabase data, string state)
	{
		_data = data;
		var model = data.GetModelByState(state);

		_index = 0;
		_phrases = model.Phrases.ToList();
		_questions = model.Questions.ToList();
		_endings = model.Endings.ToList();
	}

	private void ResetDialogue()
	{
		_waitAnswer = false;
		_dialogueGone = false;
		_textShowing = true;

		_data = null;
		_phrases = null;
		_questions = null;
		_endings = null;
	}

	public void UpdateDialogue()
	{
		_waitAnswer = false;
		_dialogueGone = false;
		_textShowing = true;
	}

	public void ShowNextPhrase(bool next)
	{
		if (_waitAnswer) return;

		if (_dialogueGone)
		{
			DialogueEnd();
			return;
		}

		if (!_textShowing) _index += next ? 1 : 0;
		_textShowing = true;
		OnShowPhrase?.Invoke(_phrases[_index]);
	}	

	public void OnPhraseShowEnd()
	{
		_textShowing = false;
		
		foreach (var end in _endings) 
		{
			if (end.Index == _index)
			{
				_dialogueGone = true;

				OnDialogueGone?.Invoke();
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
	}

	public void DialogueAnswer(bool answer)
	{
		if (_waitAnswer)
		{
			_waitAnswer = false;

			if (answer) _index = _currentQuestion.Index1;
			else _index = _currentQuestion.Index2;

			ShowNextPhrase(false);
		}
	}

	public void DialogueEnd()
	{
		_index = 0;
		_waitAnswer = false;
		_dialogueGone = false;

		_interactionSystem.TryStopInteract();
		_questSystem.UpdateData(_data);
	}

	private void OnDestroy()
	{
		_questSystem.SetData -= SetDialogue;

		_interactionSystem.OnInteracting -= ShowNextPhrase;
		_interactionSystem.OnResetDialogue -= ResetDialogue;
		_interactionSystem.EndInteracting -= DialogueEnd;
	}
}