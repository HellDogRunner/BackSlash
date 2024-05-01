using Scripts.Animations;
using Scripts.Player.camera;
using Scripts.Weapon;
using Scripts.Weapon.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
namespace Scripts.Player.Attack
{
    public class PlayerAttackService : MonoBehaviour
    {
        [SerializeField] private LayerMask _attackLayer;
        [SerializeField] private Transform _attackOrigin;
        [SerializeField] private float _attackRadius = 0.5f;

        private InputService _inputService;
        private WeaponTypesDatabase _weaponTypesDatabase;
        private WeaponController _weaponController;

        [Inject]
        private void Construct(InputService inputService, WeaponTypesDatabase weaponTypesDatabase, WeaponController weaponController)
        {
            _inputService = inputService;
            _weaponTypesDatabase = weaponTypesDatabase;
            _weaponController = weaponController;

            _inputService.OnAttackPressed += LightAttack;
        }

        private void OnDestroy()
        {
            _inputService.OnAttackPressed -= LightAttack;
        }

        private void LightAttack() 
        {         
            if (_weaponController.CurrentWeaponType != EWeaponType.None)
            {
                var weaponType = _weaponTypesDatabase.GetWeaponTypeModel(_weaponController.CurrentWeaponType);
                var weaponDamage = weaponType.LightAttackDamage;
                AttackRaycast(weaponDamage);
            }         
        }

        private void HardAttack()
        {
            if (_weaponController.CurrentWeaponType != EWeaponType.None)
            {
                var weaponType = _weaponTypesDatabase.GetWeaponTypeModel(_weaponController.CurrentWeaponType);
                var weaponDamage = weaponType.HardAttackDamage;
                AttackRaycast(weaponDamage);
            }
        }

        private void AttackRaycast(int damage) 
        {
            var weaponType = _weaponTypesDatabase.GetWeaponTypeModel(EWeaponType.Melee);
            if (Physics.SphereCast(_attackOrigin.position, _attackRadius, transform.forward * weaponType.AttackDistance, out RaycastHit hit, weaponType.AttackDistance, _attackLayer))
            {           
                if (hit.transform.tag == "Enemy")
                {
                    //if (hit.transform.TryGetComponent<HealthService>(out HealthService T))
                    //{
                    //    T.TakeDamage(damage);
                    //}
                    var enemyHealth = hit.transform.GetComponentInParent<HealthService>();
                    if (enemyHealth)
                    {
                        enemyHealth.TakeDamage(damage);
                    }
                }
            }
        }
        private void OnDrawGizmos()
        {
            
            var attackDistance = 6f;

            Gizmos.DrawWireSphere(_attackOrigin.position, attackDistance);

            RaycastHit hit;
            if (Physics.SphereCast(_attackOrigin.position, _attackRadius, transform.forward * attackDistance, out hit, attackDistance, _attackLayer))
            {
                Gizmos.color = Color.green;
                Vector3 sphereCastMidpoint = _attackOrigin.position + (transform.forward * hit.distance);
                Gizmos.DrawWireSphere(sphereCastMidpoint, _attackRadius);
                Gizmos.DrawSphere(hit.point, 0.1f);
                Debug.DrawLine(_attackOrigin.position, sphereCastMidpoint, Color.green);
                //Debug.Log(hit.transform.tag);
            }
            else
            {
                Gizmos.color = Color.red;
                Vector3 sphereCastMidpoint = _attackOrigin.position + (transform.forward * (attackDistance - _attackRadius));
                Gizmos.DrawWireSphere(_attackOrigin.position, _attackRadius);
                Debug.DrawLine(_attackOrigin.position, sphereCastMidpoint, Color.red);
            }
        }
    }
}
