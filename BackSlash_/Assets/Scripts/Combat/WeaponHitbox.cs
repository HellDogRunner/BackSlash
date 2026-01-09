using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Combat
{
    public class WeaponHitbox : MonoBehaviour
    {
        [SerializeField] private LayerMask _hitMask;
        private readonly HashSet<Collider> _hit = new();
        private bool _active;

        private int _damage;
        private Transform _owner;

        public void SetOwner(Transform owner) => _owner = owner;

        public void Activate(int damage)
        {
            _damage = damage;
            _active = true;
            _hit.Clear();
        }

        public void Deactivate()
        {
            _active = false;
            _hit.Clear();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_active) return;
            if (((1 << other.gameObject.layer) & _hitMask) == 0) return;
            if (_hit.Contains(other)) return;

            _hit.Add(other);

            if (other.TryGetComponent<IDamageable>(out var dmg))
            {
                Vector3 p = other.ClosestPoint(transform.position);
                dmg.TakeHit(_damage, p, _owner != null ? _owner.position : transform.position);
            }
        }
    }
}
