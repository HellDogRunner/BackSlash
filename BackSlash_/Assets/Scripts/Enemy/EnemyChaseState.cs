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

        private float _provokedRange = 8f;
        private float _forgotRagnge = 10;
        private float _distanceToTarget = Mathf.Infinity;

        public override void EnterState(EnemyStateManager enemy)
        {
            _agent = enemy.Agent;
            _playerTransform = enemy.PlayerTransform;
        }
        public override void OnAnimationTrigger(EnemyStateManager enemy)
        {
        }

        public override void UpdateState(EnemyStateManager enemy)
        {
            if (!_agent.enabled)
            {
                return;
            }
            enemy.Animator.SetFloat("Speed", _agent.velocity.magnitude);
            _distanceToTarget = Vector3.Distance(_playerTransform.position, enemy.transform.position);
            if (_distanceToTarget > _forgotRagnge)
            {
                enemy.SwitchState(enemy.IdleState);
            }
            if (_distanceToTarget > _provokedRange)
            {
               _agent.destination = _playerTransform.position;
            }
            if (_distanceToTarget <= _provokedRange)
            {
                enemy.SwitchState(enemy.AttackState);
            }
        }
    }
}