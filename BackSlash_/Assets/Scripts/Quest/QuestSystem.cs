using Scripts.UI.Dialogue;
using Scripts.UI.Quest;
using UnityEngine;
using Zenject;

public class QuestSystem : MonoBehaviour
{
	[SerializeField] private ActiveQuestsDatabase _activeQuests;

	private InteractionSystem _interactionSystem;

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
	
	private void OnDestroy()
	{
		_interactionSystem.SetQuest -= UpdateData;  
	}
}
