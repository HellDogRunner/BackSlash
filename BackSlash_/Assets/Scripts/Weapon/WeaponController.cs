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

        protected WeaponTypesDatabase _weaponTypesDatabase;

        private GameObject _currentWeapon;
        private InputController _inputService;
        private WeaponTypeModel _weaponTypeModel;
        private EWeaponType _curentWeaponType;

        public EWeaponType CurrentWeaponType => _curentWeaponType;

        public event Action<int> OnAttack;
        public event Action<bool> IsAttacking;
        public event Action OnDrawWeapon;
        public event Action OnSneathWeapon;

        [Inject] private DiContainer _diContainer;

        [Inject]
        private void Construct(WeaponTypesDatabase weaponTypesDatabase, InputController inputService)
        {
            _weaponTypesDatabase = weaponTypesDatabase;
            _inputService = inputService;
            _inputService.OnWeaponIdle += FinishLastSwing;
            _inputService.OnAttackPressed += Attack;
            _inputService.OnBlockPressed += Block;
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

        private void Attack()
        {
            //TODO: Сделать нормальный метод обработки комбо 
            if (_curentWeaponType == EWeaponType.None)
            {
                return;
            }
            IsAttacking?.Invoke(true);
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

        private void FinishLastSwing()
        {
            StopCoroutine(IdleTimeout());
            StartCoroutine(IdleTimeout());
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

        private void OnDestroy()
        {
            _inputService.OnWeaponIdle -= FinishLastSwing;
            _inputService.OnAttackPressed -= Attack;
            _inputService.OnBlockPressed -= Block;
        }

        IEnumerator IdleTimeout()
        {
            yield return new WaitForSeconds(.5f);
            IsAttacking?.Invoke(false);
        }
    }
}
