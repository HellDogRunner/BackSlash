using RedMoonGames.Basics;
using RedMoonGames.Database;
using UnityEngine;

namespace Scripts.UI.Dialogue
{
	[CreateAssetMenu(fileName = "Quest", menuName = "[RMG] Scriptable/Quests/QuestDatabase")]
	public class QuestDatabase : ScriprtableDatabase<QuestTypeModel>
	{
		public int Index;
		
		public QuestTypeModel GetModelByState(string state)
		{
			if (state == "") return null;

			return _data.GetBy(model => model.State == state);
		}

		public string GetDefaultState() 
		{
			return _data[0].State;
		}
	}
}
