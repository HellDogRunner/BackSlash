using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Scripts.Player
{
    public class InputController : MonoBehaviour
    {
        private GameControls _playerControls;

        private Vector3 _moveDirection;

        public event Action<bool> OnSprintKeyPressed;
        public event Action OnJumpKeyPressed;
        public event Action OnDodgeKeyPressed;
        public event Action OnLightAttackStarted;
        public event Action OnLightAttackFinished;
        public event Action OnHeavyAtttackStarted;
        public event Action OnHeavyAttackFinished;
        public event Action OnShowWeaponPressed;
        public event Action OnHideWeaponPressed;
        public event Action OnBlockPressed;
        public event Action OnLockKeyPressed;
        public event Action OnPauseKeyPressed;

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
        }

        private void ShowWeapon(InputAction.CallbackContext context) 
        {
            OnShowWeaponPressed?.Invoke();
        }

        private void HideWeapon(InputAction.CallbackContext context)
        {
            OnHideWeaponPressed?.Invoke();
        }

        private void Sprint(InputAction.CallbackContext context)
        {
            var isPressed = _playerControls.Gameplay.Sprint.IsPressed();
            OnSprintKeyPressed?.Invoke(isPressed);
        }

        private void Dodge(InputAction.CallbackContext context)
        {
            OnDodgeKeyPressed?.Invoke();
        }

        private void LightAttackPressed(InputAction.CallbackContext context)
        {
            OnLightAttackStarted?.Invoke();
        }

        private void LightAttackFinished(InputAction.CallbackContext context)
        {
            OnLightAttackFinished?.Invoke();
        }

        private void HeavyAttackPressed(InputAction.CallbackContext context)
        {
            OnHeavyAtttackStarted?.Invoke();
        }

        private void HeavytAttackFinished(InputAction.CallbackContext context)
        {
            OnHeavyAttackFinished?.Invoke();
        }

        private void Jump(InputAction.CallbackContext context)
        {
            OnJumpKeyPressed?.Invoke();
        }

        private void BlockPressed(InputAction.CallbackContext context)
        {
            OnBlockPressed?.Invoke();
        }
        private void Lock(InputAction.CallbackContext context)
        {
            OnLockKeyPressed?.Invoke();
        }

        private void PauseMenu(InputAction.CallbackContext context)
        {
            OnPauseKeyPressed?.Invoke();
        }

        private void SubscribeToActions() 
        {
            _playerControls.Gameplay.WASD.performed += ChangeDirection;
            _playerControls.Gameplay.Dodge.performed += Dodge;

            _playerControls.Gameplay.LightAttack.started += LightAttackPressed;
            _playerControls.Gameplay.LightAttack.performed += LightAttackFinished;
            _playerControls.Gameplay.LightAttack.canceled += LightAttackFinished;

            _playerControls.Gameplay.HeavyAttack.started += HeavyAttackPressed;

            _playerControls.Gameplay.Sprint.performed += Sprint;

            _playerControls.Gameplay.Block.started += BlockPressed;
            _playerControls.Gameplay.Block.performed += LightAttackFinished;
            _playerControls.Gameplay.Block.canceled += LightAttackFinished;

            _playerControls.Gameplay.Jump.performed += Jump;

            _playerControls.Gameplay.ShowWeapon.performed += ShowWeapon;
            _playerControls.Gameplay.HideWeapon.performed += HideWeapon;

            _playerControls.Gameplay.TargetLock.performed += Lock;

            _playerControls.Gameplay.Escape.performed += PauseMenu;
        }

        private void UnsubscribeToActions() 
        {
            _playerControls.Gameplay.WASD.performed -= ChangeDirection;
            _playerControls.Gameplay.Dodge.performed -= Dodge;

            _playerControls.Gameplay.LightAttack.started -= LightAttackPressed;
            _playerControls.Gameplay.LightAttack.performed -= LightAttackFinished;
            _playerControls.Gameplay.LightAttack.canceled -= LightAttackFinished;

            _playerControls.Gameplay.HeavyAttack.started -= HeavyAttackPressed;

            _playerControls.Gameplay.Sprint.performed -= Sprint;

            _playerControls.Gameplay.Block.started -= BlockPressed;
            _playerControls.Gameplay.Block.performed -= LightAttackFinished;
            _playerControls.Gameplay.Block.canceled -= LightAttackFinished;

            _playerControls.Gameplay.Jump.performed -= Jump;

            _playerControls.Gameplay.ShowWeapon.performed -= ShowWeapon;
            _playerControls.Gameplay.HideWeapon.performed -= HideWeapon;

            _playerControls.Gameplay.TargetLock.performed -= Lock;

            _playerControls.Gameplay.Escape.performed -= PauseMenu;
        }
    }
}
