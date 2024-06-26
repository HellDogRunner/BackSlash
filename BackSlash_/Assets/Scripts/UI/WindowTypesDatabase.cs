using RedMoonGames.Basics;
using RedMoonGames.Database;
using UnityEngine;

namespace Scripts.UI
{
    [CreateAssetMenu(fileName = "WindowTypesDatabase", menuName = "[RMG] Scriptable/Window/WindowTypesDatabase", order = 1)]
    public class WindowTypesDatabase : ScriptableDatabase<WindowTypeModel>
    {
        public WindowTypeModel GetWindowTypeModel(EWindowType windowType)
        {
            return _data.GetBy(windowModel => windowModel.WindowType == windowType);
        }
    }
}
