using Scripts.Player;
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
        private InputService _inputService;
        private RaycastWeapon _raycastWeapon;

        private bool _isAttack;
        [Inject]
        private void Construct(WeaponTypesDatabase weaponTypesDatabase, InputService inputService)
        {
            _weaponTypesDatabase = weaponTypesDatabase;
            _inputService = inputService;
        }

        private void OnDestroy()
        {

        }

        private void FixedUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                ShowWeapon();
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                HideWeapon();
            }
            if (_raycastWeapon != null)
            {
                if (_inputService.WeaponStateContainer.State == WeaponState.EWeaponState.Attack)
                {
                    _raycastWeapon.StartFiring();
                }
                else _raycastWeapon.StopFiring();
            }
        }

        private void ShowWeapon() 
        {
            if (_currentWeapon != null)
            {
                return;
            }
            var weaponModel = _weaponTypesDatabase.GetWeaponTypeModel(EWeaponType.Range);
            _currentWeapon = Instantiate(weaponModel?.WeaponPrefab, handTransform.position, handTransform.rotation);
            _currentWeapon.transform.parent = handTransform.transform;
            if (_currentWeapon.TryGetComponent<RaycastWeapon>(out RaycastWeapon raycastWeapon))
            {
                _raycastWeapon = raycastWeapon;
            }
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
