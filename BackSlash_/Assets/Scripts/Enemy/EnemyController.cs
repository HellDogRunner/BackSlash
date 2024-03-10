using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace MyNamespace
{

    public class EnemyController : MonoBehaviour
    {
        private enum EEnemyState
        {
            Patrol,
            Chase,
            Attack,

        }

        [SerializeField] Transform Target;

        [SerializeField] private NavMeshAgent navMeshAgent;

        [SerializeField] float ProvokedRange;
        [SerializeField] float ForgetRange;

        [SerializeField] int Damage = 10;
        [SerializeField] int AttackDelay;
        [SerializeField] float TurningSpeed = 5f;

        private bool isProvoked = false;
        private bool _canAttack = true;

        private float _distanceToTarget = Mathf.Infinity;

        private HealhService _healhService;

        [Inject]
        private void Construct(HealhService healhService)
        {
            _healhService = healhService;
            _healhService.OnHealthChanged += Attack;
        }

        void Update()
        {
            //if (dead) { return; }
            _distanceToTarget = Vector3.Distance(Target.position, transform.position);
            if (isProvoked)
            {
                EngageTarget();
            }
            else if (_distanceToTarget <= ProvokedRange)
            { 
                isProvoked = true;
            }
        }

        private void EngageTarget()
        {
            LookToTarget();

            if (_distanceToTarget >= navMeshAgent.stoppingDistance)
            {
                navMeshAgent.SetDestination(Target.position);
            }
            if (_distanceToTarget <= navMeshAgent.stoppingDistance)
            {
                Attack(Damage);
            }
            if (_distanceToTarget >= ForgetRange)
            {
                isProvoked = false;
            }

        }

        private void Attack(int damage) 
        {
            if (_canAttack)
            {
                _canAttack = false;
                _healhService.TakeDamage(damage);
                Debug.Log("Attack " + damage);
                StartCoroutine(AttackDelayTimer(damage));
            }

        }

        private void LookToTarget()
        {
            Vector3 direction = (Target.position - transform.position).normalized;

            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * TurningSpeed);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, ProvokedRange);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, ForgetRange);
        }

        IEnumerator AttackDelayTimer(int damage)
        {
            yield return new WaitForSeconds(AttackDelay);
            _canAttack = true;
        }
    }

}