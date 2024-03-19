using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Scripts.Enemy
{
    public class EnemyStateManager : MonoBehaviour
    {
        [SerializeField] private Transform playerTransform;
        private BaseEnemyState _currentState;
        private Animator _animator;
        private NavMeshAgent _agent;

        public EnemyChaseState ChaseState = new EnemyChaseState();
        public EnemyIdleState IdleState = new EnemyIdleState();
        public EnemyDeadState DeadState = new EnemyDeadState();
        public EnemyAttackState AttackState = new EnemyAttackState();

        private void Start()
        {
            _currentState = IdleState;
            _currentState.EnterState(this, playerTransform);

            _animator = gameObject.GetComponent<Animator>();
            _agent = gameObject.GetComponent<NavMeshAgent>();
        }

        private void Update()
        {
            _currentState.UpdateState(this);
            _animator.SetFloat("Speed", _agent.velocity.magnitude);
        }

        public void SwitchState(BaseEnemyState state)
        {
            _currentState = state;
            state.EnterState(this, playerTransform);
        }

    }
}