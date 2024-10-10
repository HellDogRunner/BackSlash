using Scripts.UI.Dialogue;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class DialogueSystem : MonoBehaviour
{
    private DialogueDatabase _data;

    private bool _waitAnswer;
    private bool _dialogueGone;

    private int _index;
    private Vector3 _currentConnect;
    private List<string> _phrases;
    private List<Vector3> _phraseConnect;
    private int _positiveIndex;
    private int _negativeIndex;

    private InteractionSystem _interactionSystem;
    private QuestSystem _questSystem;
    private HUDController _hudController;

    public event Action<string> OnSetModel;
    public event Action<string> OnShowPhrase;
    public event Action OnWaitAnswer;

    [Inject]
    private void Construct(QuestSystem questSystem, HUDController hudController, InteractionSystem interactionSystem)
    {
        _interactionSystem = interactionSystem;
        _questSystem = questSystem;
        _hudController = hudController;
    }

    private void Awake()
    {
        _questSystem.SetData += SetDialogueModel;

        _interactionSystem.OnInteract += ShowNextPhrase;
        _interactionSystem.OnExitTrigger += SetDefaultState;
    }

    private void SetDialogueModel(DialogueDatabase data, string state)
    {
        _data = data;
        OnSetModel?.Invoke(_data.GetName());

        var model = data.GetModelByState(state);
        _index = 0;
        _phrases = model.Phrases.ToList();
        _phraseConnect = model.Indexes.ToList();
        _positiveIndex = model.PositiveEnd;
        _negativeIndex = model.NegativeEnd;
    }

    private void SetDefaultState()
    {
        _waitAnswer = false;
        _dialogueGone = false;

        _data = null;

        _index = 0;
        _phrases = null;
        _phraseConnect = null;
        _positiveIndex = 0;
        _negativeIndex = 0;
    }

    public void ShowNextPhrase()
    {
        if (_waitAnswer) return;


        if (_dialogueGone)
        {
            OnDialogueEnd();
            return;
        }

        OnShowPhrase?.Invoke(_phrases[_index]);

        if (_positiveIndex == _index || _negativeIndex == _index)
        {
            _dialogueGone = true;
            _questSystem.ChangeQuestState(_data, _positiveIndex == _index);
            return;
        }

        foreach (var connect in _phraseConnect)
        {
            if (connect.x == _index)
            {
                _currentConnect = connect;
                _waitAnswer = true;
                OnWaitAnswer?.Invoke();
                return;
            }
        }

        _index++;
    }

    public void DialogueAnswer(bool answer)
    {
        _waitAnswer = false;
        if (answer) _index = (int)_currentConnect.y;
        else _index = (int)_currentConnect.z;

        ShowNextPhrase();
    }

    public void OnDialogueEnd()
    {
        _index = 0;
        _waitAnswer = false;
        _dialogueGone = false;

        _questSystem.UpdateData(_data);
        _interactionSystem.EndInteraction();
    }

    private void OnDestroy()
    {
        _questSystem.SetData -= SetDialogueModel;

        _interactionSystem.OnInteract -= ShowNextPhrase;
        _interactionSystem.OnExitTrigger -= SetDefaultState;
    }
}