using UnityEngine;

namespace Scripts.Combat
{
    public interface IDamageable
    {
        void TakeHit(int damage, Vector3 hitPoint, Vector3 attackerPos);
    }
}
