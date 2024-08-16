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

        private float _timeSinceAttack;
        private bool _isAttacking;
        private bool _isBlocking;

        protected WeaponTypesDatabase _weaponTypesDatabase;

        private GameObject _currentWeapon;
        private InputController _inputController;
        private WeaponTypeModel _weaponTypeModel;
        private EWeaponType _curentWeaponType;

        public EWeaponType CurrentWeaponType => _curentWeaponType;

        public event Action OnDrawWeapon;
        public event Action OnSneathWeapon;

        [Inject] private DiContainer _diContainer;

        [Inject]
        private void Construct(WeaponTypesDatabase weaponTypesDatabase, InputController inputController)
        {
            _weaponTypesDatabase = weaponTypesDatabase;
            _inputController = inputController;
            _curentWeaponType = EWeaponType.None;
        }

        private void Start()
        {
            _weaponTypeModel = _weaponTypesDatabase.GetWeaponTypeModel(EWeaponType.Melee);
            _currentWeapon = _diContainer.InstantiatePrefab(_weaponTypeModel?.WeaponPrefab, _weaponOnBeltPivot.position, _weaponOnBeltPivot.rotation, _weaponOnBeltPivot.transform);
        }

        private void DrawWeapon() 
        {
            if (_currentWeapon == null)
            {
                return;
            }
            ChangeWeaponTransform(_weaponPivot);
            _curentWeaponType = EWeaponType.Melee;
            OnDrawWeapon?.Invoke();
        }

        private void SheathWeapon()
        {
            if (_currentWeapon == null)
            {
                return;
            }
            ChangeWeaponTransform(_weaponOnBeltPivot);
            _curentWeaponType = EWeaponType.None;
            OnSneathWeapon?.Invoke();
        }

        private void ChangeWeaponTransform(Transform target)
        {
            _currentWeapon.transform.parent = target.transform;
            _currentWeapon.transform.position = target.position;
            _currentWeapon.transform.rotation = target.rotation;
        }
    }
}
