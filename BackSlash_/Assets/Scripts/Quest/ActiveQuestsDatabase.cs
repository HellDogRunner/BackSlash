using RedMoonGames.Basics;
using RedMoonGames.Database;
using Scripts.UI.Dialogue;
using UnityEngine;

namespace Scripts.UI.Quest
{
	[CreateAssetMenu(fileName = "ActiveQuests", menuName = "[RMG] Scriptable/Quests/ActiveQuestsDatabase")]
	public class ActiveQuestsDatabase : QuestScriptableDatabase<ActiveQuestsTypeModel>
	{
		public ActiveQuestsTypeModel GetModelByQuestData(QuestDatabase questData)
		{
			if (questData == null) return null;

			return _activeQuests.GetBy(model => model.QuestData == questData);
		}

		public string GetStateQuest(QuestDatabase questData)
		{
			if (questData == null) return null;
			
			return _activeQuests.GetBy(model => model.QuestData == questData).State;
		}

		public void AddQuest(QuestDatabase questData, string state)
		{
			var model = new ActiveQuestsTypeModel();

			model.QuestData = questData;
			model.State = state;

			_activeQuests.Add(model);
		}
	}
}