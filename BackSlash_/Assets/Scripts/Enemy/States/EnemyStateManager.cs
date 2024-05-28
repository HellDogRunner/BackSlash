using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Scripts.Enemy
{
    public class EnemyStateManager : MonoBehaviour
    {
        private BaseEnemyState _currentState;

        public EnemyChaseState ChaseState = new EnemyChaseState();
        public EnemyIdleState IdleState = new EnemyIdleState();
        public EnemyDeadState DeadState = new EnemyDeadState();
        public EnemyAttackState AttackState = new EnemyAttackState();

        //variables
        public Animator Animator;
        public NavMeshAgent Agent;
        public Transform PlayerTransform;
        public HealthController EnemyHealth;

        private void Start()
        {
            _currentState = IdleState;
            _currentState.EnterState(this);
        }

        private void Update()
        {
            _currentState.UpdateState(this);
        }

        public void SwitchState(BaseEnemyState state)
        {
            _currentState = state;
            state.EnterState(this);
        }

    }
}