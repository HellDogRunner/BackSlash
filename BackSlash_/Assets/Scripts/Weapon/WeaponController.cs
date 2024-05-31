using Scripts.Player;
using Scripts.Weapon.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Scripts.Animations;
using System;

namespace Scripts.Weapon 
{
    public class WeaponController : MonoBehaviour
    {
        [SerializeField] private Transform _weaponPivot;
        [SerializeField] private Transform _weaponOnBeltPivot;

        private int _currentAttack = 0;
        private float _timeSinceAttack;

        protected WeaponTypesDatabase _weaponTypesDatabase;

        private GameObject _currentWeapon;
        private InputController _inputService;
        private RaycastWeapon _raycastWeapon;
        private EWeaponType _curentWeaponType;
        private WeaponTypeModel _weaponTypeModel;

        private bool _isAttack;

        public EWeaponType CurrentWeaponType => _curentWeaponType;
        public event Action<int> OnAttack;
        public event Action<WeaponTypeModel> OnAttackPerformed;

        [Inject]
        private void Construct(WeaponTypesDatabase weaponTypesDatabase, InputController inputService)
        {
            _weaponTypesDatabase = weaponTypesDatabase;
            _inputService = inputService;

            _curentWeaponType = EWeaponType.None;
        }

        private void Start()
        {
            _weaponTypeModel = _weaponTypesDatabase.GetWeaponTypeModel(EWeaponType.Melee);
            _currentWeapon = Instantiate(_weaponTypeModel?.WeaponPrefab, _weaponOnBeltPivot.position, _weaponOnBeltPivot.rotation);
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
                OnAttackPerformed?.Invoke(_weaponTypeModel);
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
