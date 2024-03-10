using Scripts.Animations;
using Scripts.Player.Camera;
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
        [SerializeField] private LayerMask attackLayer;
        [SerializeField] private Transform playerModel;

        private InputService _inputService;
        private WeaponTypesDatabase _weaponTypesDatabase;

        [Inject]
        private void Construct(InputService inputService, WeaponTypesDatabase weaponTypesDatabase)
        {
            _inputService = inputService;
            _weaponTypesDatabase = weaponTypesDatabase;

            _inputService.OnLightAttackPressed += LightAttack;
            _inputService.OnHardAttackPressed += HardAttack;
            Debug.DrawRay(transform.position, transform.forward * 3, Color.green);
        }

        private void OnDestroy()
        {
            _inputService.OnLightAttackPressed -= LightAttack;
            _inputService.OnHardAttackPressed -= HardAttack;
        }

        private void LightAttack() 
        {
            var weaponType = _weaponTypesDatabase.GetWeaponTypeModel(EWeaponType.BasicSword);
            var weaponDamage = weaponType.LightAttackDamage;
            AttackRaycast(weaponDamage);

        }

        private void HardAttack()
        {
            var weaponType = _weaponTypesDatabase.GetWeaponTypeModel(EWeaponType.BasicSword);
            var weaponDamage = weaponType.HardAttackDamage;
            AttackRaycast(weaponDamage);
        }

        private void AttackRaycast(int damage) 
        {
            var weaponType = _weaponTypesDatabase.GetWeaponTypeModel(EWeaponType.BasicSword);
            var attackOrigin = new Vector3(playerModel.position.x, playerModel.position.y + 1f, playerModel.position.z);
            if (Physics.Raycast(attackOrigin, playerModel.transform.forward, out RaycastHit hit, weaponType.AttackDistance, attackLayer))
            {
                Debug.DrawRay(attackOrigin, playerModel.transform.forward * weaponType.AttackDistance, Color.red, 5);
                if (hit.transform.tag == "Enemy")
                {
                    if (hit.transform.TryGetComponent<HealhService>(out HealhService T))
                    {
                        T.TakeDamage(damage);
                    }
                }
            }
        }
    }
}
