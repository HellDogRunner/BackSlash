using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Scripts.Player
{
    public class InputController : MonoBehaviour
    {
        private GameControls _playerControls;
        private WeaponState _weaponState;

        private Vector3 _moveDirection;

        public event Action OnSprintKeyPressed;
        public event Action OnSprintKeyRealesed;
        public event Action OnJumpKeyPressed;
        public event Action OnDogdeKeyPressed;
        public event Action OnAttackPressed;
        public event Action OnShowWeaponPressed;
        public event Action OnHideWeaponPressed;
        public event Action OnBlockPressed;
        public event Action OnWeaponIdle;
        public event Action OnLockKeyPressed;
        public Vector3 MoveDirection => _moveDirection;
        public WeaponState WeaponStateContainer => _weaponState;

        [Inject]
        private void Construct()
        {
            _playerControls = new GameControls();
            _weaponState = new WeaponState();

            _weaponState.State = WeaponState.EWeaponState.Idle;

            _playerControls.Gameplay.WASD.performed += ChangeDirection;           
            _playerControls.Gameplay.Dodge.performed += Dodge;

            _playerControls.Gameplay.Attack.started += AttackStarted;
            _playerControls.Gameplay.Attack.canceled += WeaponIdle;

            _playerControls.Gameplay.Sprint.started += Sprint;
            _playerControls.Gameplay.Sprint.performed += Run;
            _playerControls.Gameplay.Sprint.canceled += Run;

            _playerControls.Gameplay.Block.started += Block;
            _playerControls.Gameplay.Block.performed += WeaponIdle;
            _playerControls.Gameplay.Block.canceled += WeaponIdle;

            _playerControls.Gameplay.Jump.performed += Jump;

            _playerControls.Gameplay.ShowWeapon.performed += ShowWeapon;
            _playerControls.Gameplay.HideWeapon.performed += HideWeapon;

            _playerControls.Gameplay.TargetLock.performed += Lock;
        }

        private void ChangeDirection(InputAction.CallbackContext context)
        {
            var direction = _playerControls.Gameplay.WASD.ReadValue<Vector3>();
            _moveDirection = new Vector3(direction.x, direction.z, direction.y);

            if (_moveDirection.y > 0)
            {           
                OnJumpKeyPressed?.Invoke();
            }
        }

        private void ShowWeapon(InputAction.CallbackContext context) 
        {
            _weaponState.State = WeaponState.EWeaponState.Show;
            OnShowWeaponPressed?.Invoke();
        }

        private void HideWeapon(InputAction.CallbackContext context)
        {
            _weaponState.State = WeaponState.EWeaponState.Hide;
            OnHideWeaponPressed?.Invoke();
        }

        private void Run(InputAction.CallbackContext context)
        {
            OnSprintKeyRealesed?.Invoke();
        }

        private void Sprint(InputAction.CallbackContext context)
        {
            OnSprintKeyPressed?.Invoke();
        }

        private void Dodge(InputAction.CallbackContext context)
        {
            OnDogdeKeyPressed?.Invoke();
        }

        private void Walking(InputAction.CallbackContext context)
        {
        }

        private void AttackStarted(InputAction.CallbackContext contex)
        {
            _weaponState.State = WeaponState.EWeaponState.Attack;
            OnAttackPressed?.Invoke();
        }

        private void WeaponIdle(InputAction.CallbackContext contex)
        {
            _weaponState.State = WeaponState.EWeaponState.Idle;
            OnWeaponIdle?.Invoke();
        }

        private void Jump(InputAction.CallbackContext contex)
        {
            OnJumpKeyPressed?.Invoke();
        }

        private void Block(InputAction.CallbackContext contex)
        {
            _weaponState.State = WeaponState.EWeaponState.Block;
            OnBlockPressed?.Invoke();
        }
        private void Lock(InputAction.CallbackContext contex)
        {
            OnLockKeyPressed?.Invoke();
        }

        private void OnEnable()
        {
            _playerControls.Enable();
        }

        private void OnDisable()
        {
            _playerControls.Disable();
        }
    }
}

