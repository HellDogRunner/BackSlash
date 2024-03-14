using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Scripts.Player
{
    public class InputService : MonoBehaviour
    {
        private GameControls _playerControls;
        private PlayerState _playerState;
        private WeaponState _weaponState;

        private Vector3 _moveDirection;

        public event Action OnSprintKeyPressed;
        public event Action OnJumpKeyPressed;
        public event Action OnDogdeKeyPressed;

        public Vector3 MoveDirection => _moveDirection;
        public PlayerState PlayerStateContainer => _playerState;
        public WeaponState WeaponStateContainer => _weaponState;

        public bool IsAttackPressed => _isAttackPressed;
        public bool IsJumpPressed => _isJumpPressed;

        private bool _isAttackPressed;
        private bool _isJumpPressed;


        [Inject]
        private void Construct()
        {
            _playerControls = new GameControls();
            _playerState = new PlayerState();
            _weaponState = new WeaponState();

            _weaponState.State = WeaponState.EWeaponState.Idle;

            _playerControls.Gameplay.WASD.performed += ChangeDirection;           
            _playerControls.Gameplay.Dodge.performed += Dodge;

            _playerControls.Gameplay.Attack.performed += AttackPerformed;
            _playerControls.Gameplay.Attack.canceled += AttackCanceled;

            _playerControls.Gameplay.Sprint.started += Sprint;
            _playerControls.Gameplay.Sprint.performed += Run;
            _playerControls.Gameplay.Sprint.canceled += Run;
        }

        private void ChangeDirection(InputAction.CallbackContext context)
        {
            var direction = _playerControls.Gameplay.WASD.ReadValue<Vector3>();
            _moveDirection = new Vector3(direction.x, direction.z, direction.y);
            if (_moveDirection == Vector3.zero)
            {
                _playerState.State = PlayerState.EPlayerState.Idle;
            }
            if (_moveDirection.y > 0)
            {
                _playerState.State = PlayerState.EPlayerState.Jumping;
                OnJumpKeyPressed?.Invoke();
            }
            if (_moveDirection != Vector3.zero && _moveDirection.y < 1) 
            {
                _playerState.State = PlayerState.EPlayerState.Run;
            }
        }

        private void Run(InputAction.CallbackContext context)
        {
            _playerState.State = PlayerState.EPlayerState.Run;
            OnSprintKeyPressed?.Invoke();
        }

        private void Sprint(InputAction.CallbackContext context)
        {
            _playerState.State = PlayerState.EPlayerState.Sprint;
            OnSprintKeyPressed?.Invoke();
        }

        private void Dodge(InputAction.CallbackContext context)
        {
            _playerState.State = PlayerState.EPlayerState.Dodge;
            OnDogdeKeyPressed?.Invoke();
        }
        private void Walking(InputAction.CallbackContext context)
        {
            _playerState.State = PlayerState.EPlayerState.Walk;
        }

        private void AttackPerformed(InputAction.CallbackContext contex)
        {
            _weaponState.State = WeaponState.EWeaponState.Attack;
        }

        private void AttackCanceled(InputAction.CallbackContext contex)
        {
            _weaponState.State = WeaponState.EWeaponState.Idle;
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

