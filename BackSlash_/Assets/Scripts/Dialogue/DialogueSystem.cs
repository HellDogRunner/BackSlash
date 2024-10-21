using Scripts.UI.Dialogue;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class DialogueSystem : MonoBehaviour
{
    private QuestDatabase _data;

    private List<int> _currentQuestion;
    private bool _waitAnswer;
    private bool _dialogueGone;

    private List<string> _phrases;
    private List<QuestListModel> _questions;
    private List<QuestListModel> _answers;
    private Vector2 _endings;
    private int _index;

    private InteractionSystem _interactionSystem;
    private QuestSystem _questSystem;

    public event Action<string> OnShowPhrase;
    public event Action<string, string> OnWaitAnswer;
    public event Action OnDialogueEnd;
    public event Action OnDialogueGone;

    [Inject]
    private void Construct(QuestSystem questSystem, InteractionSystem interactionSystem)
    {
        _interactionSystem = interactionSystem;
        _questSystem = questSystem;
    }

    private void Awake()
    {
        _questSystem.SetData += SetDialogueModel;

        _interactionSystem.OnInteract += ShowNextPhrase;
        _interactionSystem.OnExitTrigger += SetDefaultState;
    }

    private void SetDialogueModel(QuestDatabase data, string state)
    {
        _data = data;
        var model = data.GetModelByState(state);

        _index = 0;
        _phrases = model.Phrases.ToList();
        _questions = model.Questions.ToList();
        _answers = model.Answers.ToList();
        _endings = new Vector2(model.Endings.x, model.Endings.y);
    }

    private void SetDefaultState()
    {
        _waitAnswer = false;
        _dialogueGone = false;

        _data = null;

        _index = 0;
        _phrases = null;
        _questions = null;
        _answers = null;
        _endings = Vector2.zero;
    }

    public void ShowNextPhrase()
    {
        if (_waitAnswer) return;

        if (_dialogueGone)
        {
            DialogueEnd();
            OnDialogueEnd?.Invoke();
            return;
        }

        OnShowPhrase?.Invoke(_phrases[_index]);
    }

    public void OnPhraseShowEnd()
    {
        if (_endings.x == _index || _endings.y == _index)
        {
            _dialogueGone = true;

            OnDialogueGone?.Invoke();
            _questSystem.ChangeQuestState(_data, _endings.x == _index);
            return;
        }

        for (int index = 0; index < _questions.Count; index ++)
        {
            if (_questions[index].List[0] == _index)
            {
                _waitAnswer = true;

                var positive = _phrases[_answers[index].List[1]];  
                var negative = _phrases[_answers[index].List[2]];
                _currentQuestion = _questions[index].List;

                OnWaitAnswer?.Invoke(positive, negative);
                return;
            }
        }
        _index++;
    }

    public void DialogueAnswer(int answer)
    {
        _waitAnswer = false;

        _index = _currentQuestion[answer];
        //if (answer) _index = (int)_currentQuestion.y;
        //else _index = (int)_currentQuestion.z;

        ShowNextPhrase();
    }

    public void DialogueEnd()
    {
        _index = 0;
        _waitAnswer = false;
        _dialogueGone = false;

        _questSystem.UpdateData(_data);
    }

    private void OnDestroy()
    {
        _questSystem.SetData -= SetDialogueModel;

        _interactionSystem.OnInteract -= ShowNextPhrase;
        _interactionSystem.OnExitTrigger -= SetDefaultState;
    }
}