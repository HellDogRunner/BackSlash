using System.Collections.Generic;
using UnityEngine;
using System;
using RedMoonGames.Basics;

namespace RedMoonGames.Window
{
    [Serializable]
    public class WindowSettings
    {
        public WindowHandler Window;
        [Space]
        public CachedBehaviour WindowPrefab;
        public bool IsSingle;
    }
    [Serializable][CreateAssetMenu(fileName = "WindowServiceSettings", menuName = "[RMG] Scriptable/Windows/Settings/WindowServiceSettings", order = 1)]
    public class WindowServiceSettings : ScriptableObject
    {
        public List<WindowSettings> Windows = new List<WindowSettings>();

        public WindowSettings GetWindowSettings(WindowHandler windowHandler)
        {
            return Windows.GetBy(windowSettings => windowSettings.Window == windowHandler);
        }

        public TryResult TryGetWindowSettings(WindowHandler windowHandler, out WindowSettings windowSettings)
        {
            windowSettings = GetWindowSettings(windowHandler);
            return windowHandler != null;
        }
    }
}
