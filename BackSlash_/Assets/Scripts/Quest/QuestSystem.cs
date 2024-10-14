using Scripts.UI.Dialogue;
using Scripts.UI.Quest;
using System;
using UnityEngine;
using Zenject;

public class QuestSystem : MonoBehaviour
{
    [SerializeField] private ActiveQuestsDatabase _questData;

    private InteractionSystem _interactionSystem;

    public event Action<QuestDatabase, string> SetData;

    [Inject]
    private void Construct(InteractionSystem interactionSystem)
    {
        _interactionSystem = interactionSystem;
    }

    private void Awake()
    {
        _interactionSystem.SetData += UpdateData;
    }

    public void UpdateData(QuestDatabase dialogueData)
    {
        var model = _questData.GetModelByQuestData(dialogueData);

        if (model == null)
        {
            _questData.AddQuest(dialogueData, "Give");
            model = _questData.GetModelByQuestData(dialogueData);
        }

        SetData?.Invoke(model.QuestData, model.State);
    }

    public void ChangeQuestState(QuestDatabase dialogueData, bool result)
    {
        var interactionModel = _questData.GetModelByQuestData(dialogueData);
        var state = dialogueData.GetAnswerByState(interactionModel.State);

        if (result) interactionModel.State = state.Positive;
        else interactionModel.State = state.Negative;
    }

    public void ChangeQuestState(QuestDatabase dialogueData, string state)
    {
        var iterModel = _questData.GetModelByQuestData(dialogueData);
        iterModel.State = state;
    }

    private void OnDestroy()
    {
        _interactionSystem.SetData -= UpdateData;  
    }
}