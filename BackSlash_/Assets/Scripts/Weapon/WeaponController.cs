using Scripts.Player;
using Scripts.Weapon.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UnityEditor.Animations;
using Scripts.Animations;
using System;

namespace Scripts.Weapon 
{
    public class WeaponController : MonoBehaviour
    {
        [SerializeField] private Transform _weaponPivot;
        [SerializeField] private Transform _crossHairTarget;
        [SerializeField] private Transform _weaponOnBeltPivot;

        private int _currentAttack = 0;
        private float _timeSinceAttack;

        protected WeaponTypesDatabase _weaponTypesDatabase;

        private GameObject _currentWeapon;
        private InputService _inputService;
        private PlayerAnimationService _playerAnimationService;
        private RaycastWeapon _raycastWeapon;
        private EWeaponType _curentWeaponType;

        private bool _isAttack;

        public EWeaponType CurrentWeaponType => _curentWeaponType;
        public event Action<int> OnAttack;

        [Inject]
        private void Construct(WeaponTypesDatabase weaponTypesDatabase, InputService inputService, PlayerAnimationService playerAnimationService)
        {
            _weaponTypesDatabase = weaponTypesDatabase;
            _inputService = inputService;
            _playerAnimationService = playerAnimationService;

            _curentWeaponType = EWeaponType.None;
        }

        private void Start()
        {
            var weaponModel = _weaponTypesDatabase.GetWeaponTypeModel(EWeaponType.Melee);
            _currentWeapon = Instantiate(weaponModel?.WeaponPrefab, _weaponOnBeltPivot.position, _weaponOnBeltPivot.rotation);
            _currentWeapon.transform.parent = _weaponOnBeltPivot.transform;
        }

        private void Update()
        {
            _timeSinceAttack += Time.deltaTime;

            if (_inputService.WeaponStateContainer.State == WeaponState.EWeaponState.Attack)
            {
                Attack();
            }

            if (_inputService.WeaponStateContainer.State == WeaponState.EWeaponState.Block)
            {
                Block();
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

        private void DrawWeapon() 
        {
            if (_currentWeapon == null)
            {
                return;
            }

            ChangeWeaponTransform(_weaponPivot);
            _curentWeaponType = EWeaponType.Melee;
        }

        private void SheathWeapon()
        {
            if (_currentWeapon == null)
            {
                return;
            }
  
            ChangeWeaponTransform(_weaponOnBeltPivot);
            _curentWeaponType = EWeaponType.None;
        }

        private void Attack()
        {
            if (_timeSinceAttack > 0.8f)
            {
                _currentAttack++;

                if (_currentAttack > 3)
                {
                    _currentAttack = 1;
                }

                if (_timeSinceAttack > 1.0f)
                {
                    _currentAttack = 1;
                }

                OnAttack?.Invoke(_currentAttack);
                _timeSinceAttack = 0;
            }
        }

        private void Block()
        {

        }

        private void ChangeWeaponTransform(Transform target)
        {
            _currentWeapon.transform.parent = target.transform;
            _currentWeapon.transform.position = target.position;
            _currentWeapon.transform.rotation = target.rotation;
        }
    }
}
