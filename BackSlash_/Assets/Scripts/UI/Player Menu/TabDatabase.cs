using RedMoonGames.Basics;
using RedMoonGames.Database;
using UnityEngine;

namespace Scripts.UI.PlayerMenu
{
	[CreateAssetMenu(fileName = "TabTypesDatabase", menuName = "[RMG] Scriptable/MenuTab")]
	public class TabDatabase : ScriprtableDatabase<TabTypeModel>
	{
		public TabTypeModel GetTypeByName(string name)
		{
			if (name == "") return null;

			return _data.GetBy(tabModel => tabModel.Prefab.name == name);
		}

		public int GetTabIndex(int index)
		{
			index = index % _data.Count;
			if (index < 0) index = _data.Count - 1;
			
			return index;
		}
	}
}
