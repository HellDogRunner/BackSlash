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

        public event Action<List<AttackModel>> OnEnemySet;
        public event Action<AttackModel> OnSetAttack;
        public event Action<AttackModel> OnHitTaken;
        public event Action<Vector3> OnSetTarget;
        public event Action OnStartAttack;
        public event Action OnEndAttack;
        public event Action OnStun;
        public event Action OnDeath;
        
        private void Awake()
        {
            _healthController = GetComponent<HealthController>();
            _stabilityController = GetComponent<StabilityController>();
            _defenseController = GetComponent<DefenseController>();
            
            _healthController.OnAwake(Setup.GetHealth());
            _stabilityController.OnAwake(Setup.GetStability());
            _defenseController.OnAwake(Setup.GetDefense());
        }

        private void OnEnable()
        {
            _stabilityController.OnStun += Stunning;
            _healthController.OnDeath += Death;
        }

        private void OnDisable()
        {
            _stabilityController.OnStun -= Stunning;
            _healthController.OnDeath -= Death;
        }
        
        private void Start()
        {
            OnEnemySet?.Invoke(Setup.GetAttacksList());
        }
        
        public void StunEnd()
        {
            _stabilityController.FillUp();
        }
        
        public void HitTaken(AttackModel attack)
        {
            OnHitTaken?.Invoke(attack);
        }
        
        public void RegisterAttack(AttackModel attack)
        {
            var damage = _defenseController.CalculateDamage(attack);
            
            //Debug.Log(gameObject.name + " => " + damage + " damage taken.");
            
            _stabilityController.Damage(attack.StabilityDamage);
            _healthController.TakeDamage(damage);
        }
        
        public void SetAttack(AttackModel attack) => OnSetAttack?.Invoke(attack);
        public void SetTarget(Vector3 target) => OnSetTarget?.Invoke(target);
        public void EndAttack() => OnEndAttack?.Invoke();
        public void StartAttack() => OnStartAttack?.Invoke();
        private void Stunning() => OnStun?.Invoke();
        private void Death() => OnDeath?.Invoke();
    }
}
