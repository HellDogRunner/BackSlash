using Scripts.Player;
using Scripts.Weapon.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UnityEditor.Animations;
using Scripts.Animations;

namespace Scripts.Weapon 
{
    public class WeaponController : MonoBehaviour
    {
        [SerializeField] private Transform WeaponPivot;
        [SerializeField] private Transform crossHairTarget;
        [Space]
        [SerializeField] private Transform weaponParent;
        [SerializeField] private Transform weaponLeftGrip;
        [SerializeField] private Transform weaponRightGrip;

        protected WeaponTypesDatabase _weaponTypesDatabase;

        private GameObject _currentWeapon;
        private InputService _inputService;
        private RaycastWeapon _raycastWeapon;
        private PlayerAnimationService _animationService;

        private bool _isAttack;

        [Inject]
        private void Construct(WeaponTypesDatabase weaponTypesDatabase, InputService inputService, PlayerAnimationService playerAnimationService)
        {
            _weaponTypesDatabase = weaponTypesDatabase;
            _inputService = inputService;
            _animationService = playerAnimationService;
        }

        private void OnDestroy()
        {

        }

        private void Update()
        {
            if (_inputService.WeaponStateContainer.State == WeaponState.EWeaponState.Show)
            {
                ShowWeapon();
            }
            if (_inputService.WeaponStateContainer.State == WeaponState.EWeaponState.Hide)
            {
                HideWeapon();
            }
            if (_raycastWeapon)
            {
                if (_inputService.WeaponStateContainer.State == WeaponState.EWeaponState.Attack && !_isAttack)
                {
                    _raycastWeapon.StartFiring();
                    _isAttack = true;
                }
                if (_raycastWeapon.IsFiring)
                {
                    _raycastWeapon.UpdateFiring(Time.deltaTime, crossHairTarget.position);
                }
                _raycastWeapon.UpdateBullets(Time.deltaTime);
                if (_inputService.WeaponStateContainer.State == WeaponState.EWeaponState.Idle)
                {
                    _raycastWeapon.StopFiring();
                    _isAttack = false;
                }
            }
        }

        private void ShowWeapon() 
        {
            if (_currentWeapon != null)
            {
                return;
            }

            var weaponModel = _weaponTypesDatabase.GetWeaponTypeModel(EWeaponType.Range);

            _currentWeapon = Instantiate(weaponModel?.WeaponPrefab, WeaponPivot.position, WeaponPivot.rotation);
            _currentWeapon.transform.parent = WeaponPivot.transform;

            if (_currentWeapon.TryGetComponent<RaycastWeapon>(out RaycastWeapon raycastWeapon))
            {
                _raycastWeapon = raycastWeapon;
                _animationService.ShowWeapon(_raycastWeapon);
            }
        }

        private void HideWeapon()
        {
            if (_currentWeapon == null)
            {
                return;
            }
            _animationService.HideWeapon();
            Destroy(_currentWeapon);
        }

        [ContextMenu("Save weapon pose")]
        private void SaveWeaponPose()
        {
            if (_raycastWeapon == null)
            {
                return;
            }
            GameObjectRecorder recorder = new GameObjectRecorder(gameObject);
            recorder.BindComponentsOfType<Transform>(weaponParent.gameObject, false);
            recorder.BindComponentsOfType<Transform>(weaponRightGrip.gameObject, false);
            recorder.BindComponentsOfType<Transform>(weaponLeftGrip.gameObject, false);
            recorder.TakeSnapshot(0f);
            recorder.SaveToClip(_raycastWeapon.WeaponAnimation);
            UnityEditor.AssetDatabase.SaveAssets();
        }
    }
}
