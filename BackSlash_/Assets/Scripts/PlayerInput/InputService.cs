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

        private Vector3 _moveDirection;

        public event Action<Vector3> OnDirectionChanged;
        public event Action OnPlayerIdle;
        public event Action OnPlayerWalking;
        public event Action OnSprintKeyPressed;
        public event Action OnJumpKeyPressed;
        public event Action OnDogdeKeyPressed;
        public event Action OnLightAttackPressed;
        public event Action OnHardAttackPressed;
        public Vector3 MoveDirection => _moveDirection;
        public PlayerState StateContainer => _playerState;

        [Inject]
        private void Construct()
        {
            _playerControls = new GameControls();
            _playerState = new PlayerState();

            _playerControls.Gameplay.WASD.performed += ChangeDirection;
            _playerControls.Gameplay.Sprint.performed += Run;
            _playerControls.Gameplay.Sprint.started += Sprint;
            _playerControls.Gameplay.Sprint.canceled += Run;
            _playerControls.Gameplay.Attack.performed += Attack;
            _playerControls.Gameplay.Dodge.performed += Dodge;
        }

        private void ChangeDirection(InputAction.CallbackContext context)
        {
            var direction = _playerControls.Gameplay.WASD.ReadValue<Vector3>();
            _moveDirection = new Vector3(direction.x, direction.z, direction.y);
            if (_moveDirection == Vector3.zero)
            {
                _playerState.State = PlayerState.EPlayerState.Idle;
                OnPlayerIdle?.Invoke();
                OnDirectionChanged?.Invoke(_moveDirection);
            }
            if (_moveDirection.y > 0)
            {
                _playerState.State = PlayerState.EPlayerState.Jumping;
                OnJumpKeyPressed?.Invoke();
            }
            if (_moveDirection != Vector3.zero && _moveDirection.y < 1) 
            {
                _playerState.State = PlayerState.EPlayerState.Run;
                OnPlayerIdle?.Invoke();
            }
            OnDirectionChanged?.Invoke(_moveDirection);
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
            OnPlayerWalking?.Invoke();
        }

        private void Attack(InputAction.CallbackContext contex)
        {
            var attackType = _playerControls.Gameplay.Attack.ReadValue<float>();
            if (attackType == -1)
            {
                OnLightAttackPressed?.Invoke();
   
            }
            if (attackType == 1)
            {
                OnHardAttackPressed?.Invoke();
            }
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

