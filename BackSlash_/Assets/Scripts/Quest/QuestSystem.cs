using Scripts.UI.Dialogue;
using Scripts.UI.Quest;
using System;
using UnityEngine;
using Zenject;

public class QuestSystem : MonoBehaviour
{
    [SerializeField] private QuestDatabase _questData;

    private InteractionSystem _interactionSystem;

    public event Action<DialogueDatabase, string> SetData;

    [Inject]
    private void Construct(InteractionSystem interactionSystem)
    {
        _interactionSystem = interactionSystem;
    }

    private void Awake()
    {
        _interactionSystem.SetData += UpdateData;
    }

    public void UpdateData(DialogueDatabase dialogueData)
    {
        var model = _questData.GetModelByDialogueData(dialogueData);

        if (model == null)
        {
            _questData.AddQuest(dialogueData, "Give");
            model = _questData.GetModelByDialogueData(dialogueData);
        }

        SetData?.Invoke(model.DialogueData, model.State);
    }

    public void ChangeQuestState(DialogueDatabase dialogueData, bool result)
    {
        var interactionModel = _questData.GetModelByDialogueData(dialogueData);
        var state = dialogueData.GetAnswerByState(interactionModel.State);

        if (result) interactionModel.State = state.Positive;
        else interactionModel.State = state.Negative;
    }

    public void ChangeQuestState(DialogueDatabase dialogueData, string state)
    {
        var iterModel = _questData.GetModelByDialogueData(dialogueData);
        iterModel.State = state;
    }

    private void OnDestroy()
    {
        _interactionSystem.SetData -= UpdateData;  
    }
}