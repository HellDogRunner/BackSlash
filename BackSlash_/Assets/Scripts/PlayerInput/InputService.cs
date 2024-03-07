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

        public Action<Vector3> OnDirectionChanged;
        public Action OnPlayerIdle;
        public Action OnPlayerWalking;
        public Action OnSprintKeyPressed;
        public Action OnJumpKeyPressed;
        public Action OnAirEnding;
        public Action OnLightAttackPressed;
        public Action OnHardAttackPressed;
        public Vector3 MoveDirection;


        [Inject]
        private void Construct(PlayerState playerState, MovementService movementService)
        {
            PlayerControls = new GameControls();
            _playerState = playerState;
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
            MoveDirection = new Vector3(direction.x, direction.z, direction.y);
            if (MoveDirection == Vector3.zero)
            {
                OnAirDisabler();
                _playerState._state = PlayerState.EPlayerState.Idle;
                OnPlayerIdle?.Invoke();
            }
            else
            {
                OnAirDisabler();
                _playerState._state = PlayerState.EPlayerState.Run;
                OnPlayerIdle?.Invoke();
            }
        }

        private void RunAndSprint(InputAction.CallbackContext context)
        {
            OnAirDisabler();
            if (PlayerControls.Gameplay.Sprint.triggered && _playerState._state == PlayerState.EPlayerState.Run)
            {
                _playerState._state = PlayerState.EPlayerState.Sprint;
                OnSprintKeyPressed?.Invoke();
            }
            else if (PlayerControls.Gameplay.Sprint.triggered && _playerState._state == PlayerState.EPlayerState.Sprint)
            {
                _playerState._state = PlayerState.EPlayerState.Run;
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
                _playerState._state = PlayerState.EPlayerState.Jump;
                OnJumpKeyPressed?.Invoke();
            }
        }

        private void Walking(InputAction.CallbackContext context)
        {
            OnAirDisabler();
            if (_playerState._state != PlayerState.EPlayerState.Walk)
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

