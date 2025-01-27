using Scripts.UI.Dialogue;
using Scripts.UI.Quest;
using UnityEngine;

public class QuestSystem : MonoBehaviour
{
	[SerializeField] private ActiveQuestsDatabase _activeQuests;

	public void TryUpdateData(QuestDatabase dialogueData)
	{
		var model = _activeQuests.GetModelByQuestData(dialogueData);

		if (model == null)
		{
			_activeQuests.AddQuest(dialogueData, dialogueData.GetDefaultState());
		}
	}

	public void ChangeQuestState(QuestDatabase questData, string state)
	{
		_activeQuests.GetModelByQuestData(questData).State = state;
	}
	
	public void ChangeQuestState(QuestDatabase questData)
	{
		var state = _activeQuests.GetStateByQuest(questData);
		if (state == null) return;
		var model = questData.GetModelByState(state);
		var quest = _activeQuests.GetModelByQuestData(questData);
		
		quest.State = model.OnCompleteNextState;
	}
	
	public string GetQuestState(QuestDatabase data)
	{
		return _activeQuests.GetStateByQuest(data);
	}
}
