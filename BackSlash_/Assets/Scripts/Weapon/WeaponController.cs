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
        [SerializeField] private int _currentAttack = 0;

        private float _timeSinceAttack;
        private bool _isAttacking;

        protected WeaponTypesDatabase _weaponTypesDatabase;

        private GameObject _currentWeapon;
        private InputController _inputController;
        private WeaponTypeModel _weaponTypeModel;
        private EWeaponType _curentWeaponType;

        public EWeaponType CurrentWeaponType => _curentWeaponType;

        public event Action<int> OnAttack;
        public event Action<bool> IsAttacking;
        public event Action<bool> IsBlocking;
        public event Action OnDrawWeapon;
        public event Action OnSneathWeapon;

        [Inject] private DiContainer _diContainer;

        [Inject]
        private void Construct(WeaponTypesDatabase weaponTypesDatabase, InputController inputController)
        {
            _weaponTypesDatabase = weaponTypesDatabase;
            _inputController = inputController;
            _inputController.OnAttackFinished += AttackFinished;
            _inputController.OnAttackPressed += AttackPressed;
            _inputController.OnBlockPressed += Block;
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
            if (_curentWeaponType != EWeaponType.None && _isAttacking)
            {
                AttackCombo();
            }         
        }

        private void AttackPressed()
        {
            if (_curentWeaponType != EWeaponType.None)
            {
                _isAttacking = true;
                IsAttacking?.Invoke(true);
            }
        }

        private void AttackFinished()
        {
            _isAttacking = false;
            IsBlocking?.Invoke(false);
        }

        private void OnAttackAnimationEnd()
        {
            IsAttacking?.Invoke(_isAttacking);
        }

        private void AttackCombo()
        {
            if (_timeSinceAttack > 0.8f)
            {
                _currentAttack++;

                if (_currentAttack > 3)
                {
                    _currentAttack = 1;
                }

                if (_timeSinceAttack > 1f)
                {
                    _currentAttack = 1;
                }

                OnAttack?.Invoke(_currentAttack);
                _timeSinceAttack = 0;
            }          
        }

        private void Block()
        {
            IsBlocking?.Invoke(true);
        }

        private void DrawWeapon() 
        {
            //TODO: попытатся уйти от animation event чтобы анимация не проигрывала по 2 раза
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

        private void OnDestroy()
        {
            _inputController.OnAttackFinished -= AttackFinished;
            _inputController.OnAttackPressed -= AttackPressed;
            _inputController.OnBlockPressed -= Block;
        }
    }
}
