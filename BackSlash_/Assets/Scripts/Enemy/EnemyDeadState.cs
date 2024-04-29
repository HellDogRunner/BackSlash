using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Scripts.Enemy {
    public class EnemyDeadState : BaseEnemyState
    {
        private HealthService _health;
        private NavMeshAgent _agent;
        private Animator _animator;
        public override void EnterState(EnemyStateManager enemy)
        {
            _health = enemy.EnemyHealth;
            _agent = enemy.Agent;
            _animator = enemy.Animator;

            _agent.enabled = false;
            _animator.enabled = false;
        }

        public override void OnAnimationTrigger(EnemyStateManager enemy)
        {
            //TOTDO some logic here
        }

        public override void UpdateState(EnemyStateManager enemy)
        {
            //TOTDO some logic here
        }
    }
}
