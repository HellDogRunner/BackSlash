using RedMoonGames.Basics;
using RedMoonGames.Database;
using UnityEngine;

namespace Scripts.UI.PlayerMenu
{
    [CreateAssetMenu(fileName = "TabTypesDatabase", menuName = "[RMG] Scriptable/TabDatabase")]
    public class TabDatabase : ScriptableDatabase<TabTypeModel>
    {
        public TabTypeModel GetTypeByName(string name)
        {
            if (name == "") return null;

            return _data.GetBy(tabModel => tabModel.Name == name);
        }

        public TabTypeModel GetTypeByIndex(int index)
        {
            index = index % _data.Count;
            if (index < 0) index = _data.Count - 1;

            return _data.GetBy(tabModel => tabModel.Index == index);
        }
    }
}