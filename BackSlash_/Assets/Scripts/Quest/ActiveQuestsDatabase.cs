using RedMoonGames.Basics;
using RedMoonGames.Database;
using Scripts.UI.Dialogue;
using UnityEditor;
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

		public string GetStateByQuest(QuestDatabase questData)
		{
			var model = GetModelByQuestData(questData); 
			if (model == null) return null;
			
			return model.State;
		}

		public void AddQuest(QuestDatabase questData, string state)
		{
			var model = new ActiveQuestsTypeModel();

			model.QuestData = questData;
			model.State = state;

			_activeQuests.Add(model);
		}
		
		private void ResetQuests()
		{
			foreach (var quest in _activeQuests)
			{
				quest.QuestData.Index = 0;
			}
			
			_activeQuests.Clear();
		}
		
		[CustomEditor(typeof(ActiveQuestsDatabase))]
		public class ActiveQuestsButton : Editor
		{
			public override void OnInspectorGUI()
			{
				base.OnInspectorGUI();
				
				var _data = (ActiveQuestsDatabase)target;
				
				if (GUILayout.Button("Reset Quests"))
				{
					_data.ResetQuests();
				}
			}   	
		}
	}
}
