using Scripts.Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Scripts.Enemy
{
    public class EnemyChaseState : BaseEnemyState
    {
        private NavMeshAgent _agent;
        private Transform _playerTransform;
        private HealthService _health;

        private float _forgotRagnge = 10;
        private float _distanceToTarget = Mathf.Infinity;

        public override void EnterState(EnemyStateManager enemy)
        {
            _agent = enemy.Agent;
            _playerTransform = enemy.PlayerTransform;
            _health = enemy.EnemyHealth;
        }
 
        public override void UpdateState(EnemyStateManager enemy)
        {
            if (!_agent.enabled)
            {
                return;
            }

            _distanceToTarget = Vector3.Distance(_playerTransform.position, enemy.transform.position);
            _agent.destination = _playerTransform.position;
            enemy.Animator.SetFloat("Speed", _agent.velocity.magnitude);

            if (_distanceToTarget >= _forgotRagnge)
            {
                enemy.SwitchState(enemy.IdleState);
            }
            if (_distanceToTarget <= _agent.stoppingDistance)
            {
                enemy.SwitchState(enemy.AttackState);
            }
            if (_health.Health <= 0)
            {
                enemy.SwitchState(enemy.DeadState);
            }
        }
    }
}