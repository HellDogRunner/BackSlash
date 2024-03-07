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
        public Vector3 MoveDirection => _moveDirection;
        public PlayerState StateContainer => _playerState;

        [Inject]
        private void Construct(MovementService movementService)
        {
            PlayerControls = new GameControls();
            _playerState = new PlayerState();
            _movementService = movementService;

            PlayerControls.Gameplay.WASD.performed += ChangeDirection;
            PlayerControls.Gameplay.Sprint.performed += RunAndSprint;
            PlayerControls.Gameplay.Attack.performed += Attack;
            PlayerControls.Gameplay.Dodge.performed += Dodge;
            PlayerControls.Gameplay.Jump.performed += Jump;
        }

        private void ChangeDirection(InputAction.CallbackContext context)
        {
            var direction = PlayerControls.Gameplay.WASD.ReadValue<Vector3>();
            _moveDirection = new Vector3(direction.x, direction.z, direction.y);
            if (MoveDirection == Vector3.zero)
            {
                OnAirDisabler();
                _playerState.State = PlayerState.EPlayerState.Idle;
                OnPlayerIdle?.Invoke();
            }
            else
            {
                OnAirDisabler();
                _playerState.State = PlayerState.EPlayerState.Run;
                OnPlayerIdle?.Invoke();
            }
        }

        private void RunAndSprint(InputAction.CallbackContext context)
        {
            OnAirDisabler();
            if (PlayerControls.Gameplay.Sprint.triggered && _playerState.State == PlayerState.EPlayerState.Run)
            {
                _playerState.State = PlayerState.EPlayerState.Sprint;
                OnSprintKeyPressed?.Invoke();
            }
            else if (PlayerControls.Gameplay.Sprint.triggered && _playerState.State == PlayerState.EPlayerState.Sprint)
            {
                _playerState.State = PlayerState.EPlayerState.Run;
                OnSprintKeyPressed?.Invoke();
            }
        }

        private void Dodge(InputAction.CallbackContext context)
        {
            OnAirDisabler();
            Debug.Log("Dodge");
        }

        private void Jump(InputAction.CallbackContext context)
        {
            if (PlayerControls.Gameplay.Jump.triggered && _movementService.IsGrounded())
            {
                _playerState.State = PlayerState.EPlayerState.Jump;
                OnJumpKeyPressed?.Invoke();
            }
        }

        private void Walking(InputAction.CallbackContext context)
        {
            OnAirDisabler();
            if (_playerState.State != PlayerState.EPlayerState.Walk)
            {

            }
        }

        private void Attack(InputAction.CallbackContext contex)
        {
            OnAirDisabler();
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

        private void OnAirDisabler()
        {
            if (_movementService.IsGrounded())
            {
                Debug.Log("OnAirDisabler");
                OnAirEnding?.Invoke();
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

