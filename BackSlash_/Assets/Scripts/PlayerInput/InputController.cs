using System;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
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
        public event Action OnDodgeKeyPressed;
        public event Action OnAttackPressed;
        public event Action OnShowWeaponPressed;
        public event Action OnHideWeaponPressed;
        public event Action OnBlockPressed;
        public event Action OnAttackFinished;
        public event Action OnLockKeyPressed;
        public event Action OnPauseKeyPressed;
        public event Action OnInventoryKeyPressed;

        public event Action<int> OnMouseButtonsPressed;
        public Vector3 MoveDirection => _moveDirection;


        private void Awake()
        {
            _playerControls = new GameControls();
        }
        private void OnEnable()
        {           
            _playerControls.Enable();
            SubscribeToActions();
            _moveDirection = Vector3.zero;
        }

        private void OnDisable()
        {
            _playerControls.Disable();
            UnsubscribeToActions();
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
            OnDodgeKeyPressed?.Invoke();
        }

        private void AttackPressed(InputAction.CallbackContext context)
        {
            OnAttackPressed?.Invoke();
            OnMouseButtonsPressed?.Invoke(1);
        }

        private void AttackFinished(InputAction.CallbackContext context)
        {
            OnAttackFinished?.Invoke();
        }

        private void Jump(InputAction.CallbackContext context)
        {
            OnJumpKeyPressed?.Invoke();
        }

        private void BlockPressed(InputAction.CallbackContext context)
        {
            OnBlockPressed?.Invoke();
            OnMouseButtonsPressed?.Invoke(2);
        }
        private void Lock(InputAction.CallbackContext context)
        {
            OnLockKeyPressed?.Invoke();
        }

        private void PauseMenu(InputAction.CallbackContext context)
        {
            OnPauseKeyPressed?.Invoke();
        }

        private void Inventory(InputAction.CallbackContext context)
        {
            OnInventoryKeyPressed?.Invoke();
        }

        private void SubscribeToActions() 
        {
            _playerControls.Gameplay.WASD.performed += ChangeDirection;
            _playerControls.Gameplay.Dodge.performed += Dodge;

            _playerControls.Gameplay.LightAttack.started += AttackPressed;
            _playerControls.Gameplay.LightAttack.performed += AttackFinished;
            _playerControls.Gameplay.LightAttack.canceled += AttackFinished;

            _playerControls.Gameplay.Sprint.started += Sprint;
            _playerControls.Gameplay.Sprint.performed += Run;
            _playerControls.Gameplay.Sprint.canceled += Run;

            _playerControls.Gameplay.Block.started += BlockPressed;
            _playerControls.Gameplay.Block.performed += AttackFinished;
            _playerControls.Gameplay.Block.canceled += AttackFinished;

            _playerControls.Gameplay.Jump.performed += Jump;

            _playerControls.Gameplay.ShowWeapon.performed += ShowWeapon;
            _playerControls.Gameplay.HideWeapon.performed += HideWeapon;

            _playerControls.Gameplay.TargetLock.performed += Lock;

            _playerControls.Gameplay.Escape.performed += PauseMenu;

            _playerControls.Gameplay.Inventory.performed += Inventory;
        }

        private void UnsubscribeToActions() 
        {
            _playerControls.Gameplay.WASD.performed -= ChangeDirection;
            _playerControls.Gameplay.Dodge.performed -= Dodge;

            _playerControls.Gameplay.LightAttack.started -= AttackPressed;
            _playerControls.Gameplay.LightAttack.performed -= AttackFinished;
            _playerControls.Gameplay.LightAttack.canceled -= AttackFinished;

            _playerControls.Gameplay.Sprint.started -= Sprint;
            _playerControls.Gameplay.Sprint.performed -= Run;
            _playerControls.Gameplay.Sprint.canceled -= Run;

            _playerControls.Gameplay.Block.started -= BlockPressed;
            _playerControls.Gameplay.Block.performed -= AttackFinished;
            _playerControls.Gameplay.Block.canceled -= AttackFinished;

            _playerControls.Gameplay.Jump.performed -= Jump;

            _playerControls.Gameplay.ShowWeapon.performed -= ShowWeapon;
            _playerControls.Gameplay.HideWeapon.performed -= HideWeapon;

            _playerControls.Gameplay.TargetLock.performed -= Lock;

            _playerControls.Gameplay.Escape.performed -= PauseMenu;

            _playerControls.Gameplay.Inventory.performed -= Inventory;
        }
    }
}
