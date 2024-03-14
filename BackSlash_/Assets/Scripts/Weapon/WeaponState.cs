using UnityEngine;

namespace Scripts.Player
{
    public class WeaponState
    {
        public EWeaponState State;

        public enum EWeaponState
        {
            Attack,
            Idle
        }
    }
}
