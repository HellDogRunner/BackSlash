using Scripts.Player;
using Scripts.Weapon.Models;
using System.Collections;
using UnityEngine;
using Zenject;
using System;
using FMOD.Studio;

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
        private AudioManager _audioManager;
        private WeaponTypeModel _weaponTypeModel;
        private EWeaponType _curentWeaponType;
        //audio
        private EventInstance _swordSlashSound;

        public EWeaponType CurrentWeaponType => _curentWeaponType;
        public event Action<int> OnAttack;
        public event Action<bool> IsAttacking;

        [Inject] private DiContainer _diContainer;

        [Inject]
        private void Construct(WeaponTypesDatabase weaponTypesDatabase, InputController inputService, AudioManager audioManager)
        {
            _weaponTypesDatabase = weaponTypesDatabase;
            _audioManager = audioManager;
            _inputService = inputService;
            _inputService.OnWeaponIdle += CancelAttack;

            _curentWeaponType = EWeaponType.None;
        }

        private void Start()
        {
            _weaponTypeModel = _weaponTypesDatabase.GetWeaponTypeModel(EWeaponType.Melee);
            _currentWeapon = _diContainer.InstantiatePrefab(_weaponTypeModel?.WeaponPrefab, _weaponOnBeltPivot.position, _weaponOnBeltPivot.rotation, _weaponOnBeltPivot.transform);

            _swordSlashSound = _audioManager.CreateEventInstance(FMODEvents.instance.SlashSword);
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
        }

        private void DrawWeapon() 
        {
            if (_currentWeapon == null)
            {
                return;
            }
            _audioManager.PlayGenericEvent(FMODEvents.instance.DrawSword);
            ChangeWeaponTransform(_weaponPivot);
            _curentWeaponType = EWeaponType.Melee;
        }

        private void SheathWeapon()
        {
            if (_currentWeapon == null)
            {
                return;
            }
            _audioManager.PlayGenericEvent(FMODEvents.instance.SneathSword);
            ChangeWeaponTransform(_weaponOnBeltPivot);
            _curentWeaponType = EWeaponType.None;
        }

        private void Attack()
        {
            if (_curentWeaponType == EWeaponType.None)
            {
                return;
            }
            if (_timeSinceAttack > 0.8f)
            {
                //_audioManager.PlayGenericEvent(FMODEvents.instance.SlashSword);
                _currentAttack++;

                if (_currentAttack > 3)
                {
                    _currentAttack = 1;
                }

                if (_timeSinceAttack > 1.0f)
                {
                    _currentAttack = 1;
                }
                _swordSlashSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                _swordSlashSound.setParameterByName("Combo", _currentAttack);
                _swordSlashSound.start();
                OnAttack?.Invoke(_currentAttack);
                _timeSinceAttack = 0;
            }          
        }

        private void CancelAttack()
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
        IEnumerator IdleTimeout()
        {
            yield return new WaitForSeconds(.5f);
            IsAttacking?.Invoke(false);
        }
    }
}
