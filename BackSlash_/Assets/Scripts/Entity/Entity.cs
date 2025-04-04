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
        [field: SerializeField] public EntityDatabase Setup { get; private set; }
        
        private HealthController _healthController;
        private StabilityController _stabilityController;
        private DefenseController _defenseController;

        public event Action<List<AttackModel>> OnSetAttack;
        public event Action OnStun;

        private void Awake()
        {
            _healthController = GetComponent<HealthController>();
            _stabilityController = GetComponent<StabilityController>();
            _defenseController = GetComponent<DefenseController>();
            
            _healthController.OnAwake(Setup.Health);
            _stabilityController.OnAwake(Setup.Stability);
            _defenseController.OnAwake(Setup.Defense);
        }

        void OnEnable()
        {
            _stabilityController.OnStun += Stunning;
        }

        void OnDisable()
        {
            _stabilityController.OnStun -= Stunning;
        }
        
        private void Start()
        {
            OnSetAttack?.Invoke(Setup.Attack);
        }
        
        public void StunEnd()
        {
            _stabilityController.FillUp();
        }
        
        public void RegisterAttack(AttackModel attack)
        {
            var damage = _defenseController.CalculateDamage(attack);
            
            _stabilityController.Damage(attack.StabilityDamage);
            _healthController.TakeDamage(damage);
        }
        
        private void Stunning() => OnStun?.Invoke();
    }
}
