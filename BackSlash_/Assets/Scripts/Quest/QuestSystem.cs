using Scripts.UI.Dialogue;
using Scripts.UI.Quest;
using System;
using UnityEngine;
using Zenject;

public class QuestSystem : MonoBehaviour
{
	[SerializeField] private ActiveQuestsDatabase _activeQuests;

	private InteractionSystem _interactionSystem;

	public event Action<QuestDatabase, string> SetData;

	[Inject]
	private void Construct(InteractionSystem interactionSystem)
	{
		_interactionSystem = interactionSystem;
	}

	private void Awake()
	{
		_interactionSystem.SetQuest += UpdateData;
	}

	public void UpdateData(QuestDatabase dialogueData)
	{
		var model = _activeQuests.GetModelByQuestData(dialogueData);

		if (model == null)
		{
			_activeQuests.AddQuest(dialogueData, dialogueData.GetDefaultState());
			model = _activeQuests.GetModelByQuestData(dialogueData);
		}

		SetData?.Invoke(model.QuestData, model.State);
	}

	public void ChangeQuestState(QuestDatabase questData, string state)
	{
		_activeQuests.GetModelByQuestData(questData).State = state;
	}
	
	public void ChangeQuestState(QuestDatabase questData)
	{
		var state = _activeQuests.GetStateQuest(questData);
		var model = questData.GetModelByState(state);
		
		_activeQuests.GetModelByQuestData(questData).State = model.OnCompleteNextState;
	}
	
	private void OnDestroy()
	{
		_interactionSystem.SetQuest -= UpdateData;  
	}
}