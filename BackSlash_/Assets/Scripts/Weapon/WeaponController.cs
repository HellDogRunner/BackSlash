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
        [SerializeField] private Transform WeaponPivot;
        [SerializeField] private Transform crossHairTarget;
        [SerializeField] private UnityEngine.Animations.Rigging.Rig _handIk;

        protected WeaponTypesDatabase _weaponTypesDatabase;
        private GameObject _currentWeapon;
        private InputService _inputService;
        private RaycastWeapon _raycastWeapon;

        [Inject]
        private void Construct(WeaponTypesDatabase weaponTypesDatabase, InputService inputService)
        {
            _weaponTypesDatabase = weaponTypesDatabase;
            _inputService = inputService;

            _handIk.weight = 0f;
        }

        private void OnDestroy()
        {

        }

        private void LateUpdate()
        {
            if (_inputService.WeaponStateContainer.State == WeaponState.EWeaponState.Show)
            {
                ShowWeapon();
            }
            if (_inputService.WeaponStateContainer.State == WeaponState.EWeaponState.Hide)
            {
                HideWeapon();
            }
            if (_raycastWeapon != null)
            {
                if (_inputService.WeaponStateContainer.State == WeaponState.EWeaponState.Attack)
                {
                    _raycastWeapon.StartFiring();
                }
                if (_raycastWeapon.IsFiring)
                {
                    _raycastWeapon.UpdateFiring(Time.deltaTime);

                }
                _raycastWeapon.UpdateBullets(Time.deltaTime);
                if (_inputService.WeaponStateContainer.State == WeaponState.EWeaponState.Idle)
                {
                    _raycastWeapon.StopFiring();

                }
            }
        }

        private void ShowWeapon() 
        {
            if (_currentWeapon != null)
            {
                return;
            }
            _handIk.weight = 1f;

            var weaponModel = _weaponTypesDatabase.GetWeaponTypeModel(EWeaponType.Range);
            _currentWeapon = Instantiate(weaponModel?.WeaponPrefab, WeaponPivot.position, WeaponPivot.rotation);
            _currentWeapon.transform.parent = WeaponPivot.transform;
            if (_currentWeapon.TryGetComponent<RaycastWeapon>(out RaycastWeapon raycastWeapon))
            {
                _raycastWeapon = raycastWeapon;
                _raycastWeapon.RaycastDestination = crossHairTarget;
            }
        }

        private void HideWeapon()
        {
            if (_currentWeapon == null)
            {
                return;
            }
            _handIk.weight = 0f;
            Destroy(_currentWeapon);
        }
    }
}
