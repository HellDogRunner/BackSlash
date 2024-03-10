using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace MyNamespace
{

    public class EnemyController : MonoBehaviour
    {
        [SerializeField] Transform Target;

        [SerializeField] private NavMeshAgent navMeshAgent;

        [SerializeField] float ProvokedRange;
        [SerializeField] float ForgetRange;
        [SerializeField] float TurningSpeed = 5f;

        [SerializeField] int Damage = 10;
        [SerializeField] int AttackDistance = 2 ;
        [SerializeField] int AttackDelay;

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
            if (_distanceToTarget <= AttackDistance)
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
                AttackRaycast(damage);
                StartCoroutine(AttackDelayTimer());
            }

        }

        private void LookToTarget()
        {
            Vector3 direction = (Target.position - transform.position).normalized;

            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * TurningSpeed);
        }

        private void AttackRaycast(int damage)
        {
            var attackOrigin = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
            if (Physics.Raycast(attackOrigin, transform.forward, out RaycastHit hit, AttackDistance, attackLayer))
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

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, ProvokedRange);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, ForgetRange);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, AttackDistance);
        }

        IEnumerator AttackDelayTimer()
        {
            yield return new WaitForSeconds(AttackDelay);
            _canAttack = true;
        }
    }

}