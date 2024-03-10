using Scripts.Weapon.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Scripts.Weapon 
{
    public class WeaponController : MonoBehaviour
    {
        [SerializeField] private Transform handTransform;

        protected WeaponTypesDatabase _weaponTypesDatabase;
        private GameObject _currentWeapon;

        [Inject]
        private void Construct(WeaponTypesDatabase weaponTypesDatabase)
        {
            _weaponTypesDatabase = weaponTypesDatabase;
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                ShowWeapon();
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                HideWeapon();
            }
        }

        private void ShowWeapon() 
        {
            if (_currentWeapon != null)
            {
                return;
            }
            var weaponModel = _weaponTypesDatabase.GetWeaponTypeModel(EWeaponType.BasicSword);
            _currentWeapon = Instantiate(weaponModel?.WeaponPrefab, handTransform.position, handTransform.rotation);
            _currentWeapon.transform.parent = handTransform.transform;
        }

        private void HideWeapon()
        {
            if (_currentWeapon == null)
            {
                return;
            }
            Destroy(_currentWeapon);
        }
    }
}
