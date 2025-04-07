using System;

namespace Scripts.Entity
{
    [Serializable]
    public enum EAttackType : int
    {
        None = 0,
        Punch = 1,
        Shoot = 2,
        StabbingAttack = 3
    }
}
