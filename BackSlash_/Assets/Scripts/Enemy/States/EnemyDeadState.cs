using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

namespace Scripts.Enemy {
    public class EnemyDeadState : BaseEnemyState
    {
        private NavMeshAgent _agent;
        private Animator _animator;

        public override void EnterState(EnemyStateManager enemy)
        {
            _agent = enemy.Agent;
            _animator = enemy.Animator;

            _agent.enabled = false;
            _animator.enabled = false;
        }

        public override void UpdateState(EnemyStateManager enemy)
        {
            //TODO some logic here
        }
    }
}
