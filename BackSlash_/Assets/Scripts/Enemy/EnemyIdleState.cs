using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Scripts.Enemy
{
    public class EnemyIdleState : BaseEnemyState
    {
        private float _provokedRange = 8f;
        private Transform _playerTransform;

        private float _distanceToTarget = Mathf.Infinity;

        private HealhService _health;
        public override void EnterState(EnemyStateManager enemy)
        {
            _playerTransform = enemy.PlayerTransform;
            _health = enemy.EnemyHealth;
        }

        public override void OnAnimationTrigger(EnemyStateManager enemy)
        {
            throw new System.NotImplementedException();
        }

        public override void UpdateState(EnemyStateManager enemy)
        {
            _distanceToTarget = Vector3.Distance(_playerTransform.position, enemy.transform.position);
            if (_distanceToTarget < _provokedRange)
            {
                enemy.SwitchState(enemy.ChaseState);
            }
            if (_health.Health <= 0)
            {
                enemy.SwitchState(enemy.DeadState);
            }
        }
    }
}
