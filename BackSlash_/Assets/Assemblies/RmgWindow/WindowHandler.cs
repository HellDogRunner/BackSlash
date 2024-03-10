using System;
using UnityEngine;

namespace RedMoonGames.Window
{
    [Serializable][CreateAssetMenu(fileName = "WindowHandler", menuName = "[RMG] Scriptable/Windows/WindowHandler", order = 1)]
    public class WindowHandler : ScriptableObject
    {
        public string WindowId = Guid.NewGuid().ToString();

        public override bool Equals(object other)
        {
            if(other is null)
            {
                return false;
            }

            if(other is not WindowHandler windowHandler)
            {
                return false;
            }

            return WindowId == windowHandler.WindowId;
        }

        public override int GetHashCode()
        {
            return WindowId.GetHashCode();
        }

        public static bool operator ==(WindowHandler windowHandler1, WindowHandler windowHandler2)
        {
            if (ReferenceEquals(windowHandler1, windowHandler2))
            {
                return true;
            }

            if (((object)windowHandler1 == null) || ((object)windowHandler2 == null))
            {
                return false;
            }

            return windowHandler1.Equals(windowHandler2);
        }

        public static bool operator !=(WindowHandler windowHandler1, WindowHandler windowHandler2)
        {
            return !(windowHandler1 == windowHandler2);
        }
    }
}
