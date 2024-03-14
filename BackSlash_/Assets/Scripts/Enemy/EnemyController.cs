using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace MyNamespace
{

    public class EnemyController : MonoBehaviour
    {
        [SerializeField] Transform target;

        [SerializeField] private NavMeshAgent navMeshAgent;

        [SerializeField] float provokedRange;
        [SerializeField] float forgetRange;
        [SerializeField] float turningSpeed = 5f;
        [Header("MeleeSettings")]
        [SerializeField] int damage = 10;
        [SerializeField] int attackDistance = 2;
        [SerializeField] int attackDelay;

        [SerializeField] private LayerMask attackLayer;

        private bool isProvoked = false;
        private bool _canAttack = true;

        private float _distanceToTarget = Mathf.Infinity;

        [Inject]
        private void Construct()
        {

        }

        void Update()
        {
            _distanceToTarget = Vector3.Distance(target.position, transform.position);
            if (isProvoked)
            {
                EngageTarget();
            }
            else if (_distanceToTarget <= provokedRange)
            {
                isProvoked = true;
            }
        }

        private void EngageTarget()
        {
            LookToTarget();

            if (_distanceToTarget >= navMeshAgent.stoppingDistance)
            {
                navMeshAgent.SetDestination(target.position);
            }
            if (_distanceToTarget <= attackDistance)
            {
                Attack(damage);
            }
            if (_distanceToTarget >= forgetRange)
            {
                isProvoked = false;
            }
        }

        private void Attack(int damage)
        {
            if (_canAttack)
            {
                _canAttack = false;
                RangeAttack(damage);
                StartCoroutine(AttackDelayTimer());
            }

        }

        private void LookToTarget()
        {
            Vector3 direction = (target.position - transform.position).normalized;

            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turningSpeed);
        }

        private void MeleeAttack(int damage)
        {
            var attackOrigin = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
            if (Physics.Raycast(attackOrigin, transform.forward, out RaycastHit hit, attackDistance, attackLayer))
            {
                if (hit.transform.tag == "Player")
                {
                    if (hit.transform.TryGetComponent<HealhService>(out HealhService T))
                    {
                        T.TakeDamage(damage);
                    }
                }
            }
        }

        private void RangeAttack(int damage)
        {
            var attackOrigin = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
            if (Physics.Raycast(attackOrigin, transform.forward, out RaycastHit hit, attackDistance, attackLayer))
            {
 
            }
        }
        IEnumerator AttackDelayTimer()
        {
            yield return new WaitForSeconds(attackDelay);
            _canAttack = true;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, provokedRange);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, forgetRange);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackDistance);
        }
    }
}