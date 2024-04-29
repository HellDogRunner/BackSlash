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
        [SerializeField] private Transform _weaponPivot;
        [SerializeField] private Transform _crossHairTarget;
        [Space]
        [SerializeField] private Transform _weaponParent;
        [SerializeField] private Transform _weaponLeftGrip;
        [SerializeField] private Transform _weaponRightGrip;

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
                    _raycastWeapon.UpdateFiring(Time.deltaTime, _crossHairTarget.position);
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

            var weaponModel = _weaponTypesDatabase.GetWeaponTypeModel(EWeaponType.Melee);

            _currentWeapon = Instantiate(weaponModel?.WeaponPrefab, _weaponPivot.position, _weaponPivot.rotation);
            _currentWeapon.transform.parent = _weaponPivot.transform;

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
            recorder.BindComponentsOfType<Transform>(_weaponParent.gameObject, false);
            recorder.BindComponentsOfType<Transform>(_weaponRightGrip.gameObject, false);
            recorder.BindComponentsOfType<Transform>(_weaponLeftGrip.gameObject, false);
            recorder.TakeSnapshot(0f);
            recorder.SaveToClip(_raycastWeapon.WeaponAnimation);
            UnityEditor.AssetDatabase.SaveAssets();
        }
    }
}
