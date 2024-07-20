using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Scripts.Player
{
    public class InputController : MonoBehaviour
    {
        private GameControls _playerControls;

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
        public event Action OnMenuKeyPressed;
        public Vector3 MoveDirection => _moveDirection;

        [Inject]
        private void Construct()
        {
            _playerControls = new GameControls();

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

            _playerControls.Gameplay.Escape.performed += PauseMenu;
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
            OnShowWeaponPressed?.Invoke();
        }

        private void HideWeapon(InputAction.CallbackContext context)
        {
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

        private void AttackStarted(InputAction.CallbackContext contex)
        {
            OnAttackPressed?.Invoke();
        }

        private void WeaponIdle(InputAction.CallbackContext contex)
        {
            OnWeaponIdle?.Invoke();
        }

        private void Jump(InputAction.CallbackContext contex)
        {
            OnJumpKeyPressed?.Invoke();
        }

        private void Block(InputAction.CallbackContext contex)
        {
            OnBlockPressed?.Invoke();
        }
        private void Lock(InputAction.CallbackContext contex)
        {
            OnLockKeyPressed?.Invoke();
        }

        private void PauseMenu(InputAction.CallbackContext contex)
        {
            OnMenuKeyPressed?.Invoke();
        }

        private void OnEnable()
        {
            _playerControls.Enable();

            _moveDirection = Vector3.zero;
        }

        private void OnDisable()
        {
            _playerControls.Disable();
        }
    }
}
