using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Scripts.Player
{
    public class InputService : MonoBehaviour
    {
        private GameControls PlayerControls;
        private PlayerState _playerState;
        private MovementService _movementService;

        private Vector3 _moveDirection;

        public event Action<Vector3> OnDirectionChanged;
        public event Action OnPlayerIdle;
        public event Action OnPlayerWalking;
        public event Action OnSprintKeyPressed;
        public event Action OnJumpKeyPressed;
        public event Action OnAirEnding;
        public event Action OnLightAttackPressed;
        public event Action OnHardAttackPressed;
        public event Action OnPlayerRunning;
        public Vector3 MoveDirection => _moveDirection;
        public PlayerState StateContainer => _playerState;

        [Inject]
        private void Construct()
        {
            PlayerControls = new GameControls();
            _playerState = new PlayerState();

            PlayerControls.Gameplay.WASD.performed += ChangeDirection;
            PlayerControls.Gameplay.Sprint.performed += Run;
            PlayerControls.Gameplay.Sprint.started += Sprint;
            PlayerControls.Gameplay.Sprint.canceled += Run;
            PlayerControls.Gameplay.Attack.performed += Attack;
            PlayerControls.Gameplay.Dodge.performed += Dodge;
        }

        private void ChangeDirection(InputAction.CallbackContext context)
        {
            var direction = PlayerControls.Gameplay.WASD.ReadValue<Vector3>();
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
                OnDirectionChanged?.Invoke(_moveDirection);
            }
            if (_moveDirection != Vector3.zero && _moveDirection.y < 1) 
            {
                _playerState.State = PlayerState.EPlayerState.Run;
                OnPlayerIdle?.Invoke();
                OnDirectionChanged?.Invoke(_moveDirection);
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
            Debug.Log("Dodge");
        }
        private void Walking(InputAction.CallbackContext context)
        {
            if (_playerState.State != PlayerState.EPlayerState.Walk)
            {

            }
        }

        private void Attack(InputAction.CallbackContext contex)
        {
            var attackType = PlayerControls.Gameplay.Attack.ReadValue<float>();
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
            PlayerControls.Enable();
        }

        private void OnDisable()
        {
            PlayerControls.Disable();
        }
    }
}

