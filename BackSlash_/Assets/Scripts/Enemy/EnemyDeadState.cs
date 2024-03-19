using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Scripts.Enemy {
    public class EnemyDeadState : BaseEnemyState
    {
        private HealhService _health;
        private NavMeshAgent _agent;
        private Animator _animator;
        public override void EnterState(EnemyStateManager enemy, Transform player)
        {
            _health = enemy.GetComponent<HealhService>();
            _agent = enemy.GetComponent<NavMeshAgent>();
            _animator = enemy.GetComponent<Animator>();

            _agent.enabled = false;
            _animator.enabled = false;
        }

        public override void UpdateState(EnemyStateManager enemy)
        {
 
        }
    }
}
