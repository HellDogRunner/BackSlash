using Scripts.Player;
using Scripts.Weapon.Models;
using System.Collections;
using UnityEngine;
using Zenject;
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
        private EWeaponType _curentWeaponType;
        private WeaponTypeModel _weaponTypeModel;

        public EWeaponType CurrentWeaponType => _curentWeaponType;
        public event Action<int> OnAttack;
        public event Action<bool> IsAttacking;

        [Inject] private DiContainer _diContainer;

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
            _currentWeapon = _diContainer.InstantiatePrefab(_weaponTypeModel?.WeaponPrefab, _weaponOnBeltPivot.position, _weaponOnBeltPivot.rotation, _weaponOnBeltPivot.transform);
        }

        private void Update()
        {
            _timeSinceAttack += Time.deltaTime;

            if (_inputService.WeaponStateContainer.State == WeaponState.EWeaponState.Attack)
            {
                Attack();
                IsAttacking?.Invoke(true);
            }

            if (_inputService.WeaponStateContainer.State == WeaponState.EWeaponState.Block)
            {
                Block();
            }
            if (_inputService.WeaponStateContainer.State == WeaponState.EWeaponState.Idle)
            {
                IsAttacking?.Invoke(false);
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
