using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Entity
{
    [RequireComponent(typeof(HealthController))]
    [RequireComponent(typeof(StabilityController))]
    [RequireComponent(typeof(DefenseController))]
    public class Entity : MonoBehaviour
    {
        [SerializeField] private EntityDatabase _setup;
        
        private HealthController _healthController;
        private StabilityController _stabilityController;
        private DefenseController _defenseController;

        public event Action<List<AttackModel>> OnSetAttack;

        private void Awake()
        {
            _healthController = GetComponent<HealthController>();
            _stabilityController = GetComponent<StabilityController>();
            _defenseController = GetComponent<DefenseController>();
            
            _healthController.OnAwake(_setup.Health);
            _stabilityController.OnAwake(_setup.Stability);
            _defenseController.OnAwake(_setup.Defense);
        }

        private void Start()
        {
            OnSetAttack?.Invoke(_setup.Attack);
        }
        
        public void RegisterAttack(AttackModel attack)
        {
            var damage = _defenseController.CalculateDamage(attack);
            
            _stabilityController.Damage(attack.StabilityDamage);
            _healthController.TakeDamage(damage);
        }
    }
}
