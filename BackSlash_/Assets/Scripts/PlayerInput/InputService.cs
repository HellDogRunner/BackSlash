using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Scripts.Player
{
    public class InputService : MonoBehaviour
    {
        private GameControls PlayerControls;
        private bool RunState;

        public Action<Vector3> OnDirectionChanged;
        public Action<bool> OnSprintKeyPressed;
        public Action OnLightAttackPressed;
        public Action OnHardAttackPressed;
        public Vector3 MoveDirection;


        [Inject]
        private void Construct()
        {
            PlayerControls = new GameControls();

            PlayerControls.Gameplay.WASD.performed += ChangeDirection;
            PlayerControls.Gameplay.Sprint.performed += WalkAndSprint;
            PlayerControls.Gameplay.Attack.performed += Attack;
        }

        private void ChangeDirection(InputAction.CallbackContext context)
        {
            var direction = PlayerControls.Gameplay.WASD.ReadValue<Vector3>();
            MoveDirection = new Vector3(direction.x, direction.z, direction.y);
            OnDirectionChanged?.Invoke(MoveDirection);
        }

        private void WalkAndSprint(InputAction.CallbackContext context) 
        {
            if (PlayerControls.Gameplay.Sprint.triggered && RunState == false)
            {
                OnSprintKeyPressed?.Invoke(true);
                RunState = true;
            }
            else if (PlayerControls.Gameplay.Sprint.triggered && RunState == true )
            {
                OnSprintKeyPressed?.Invoke(false);
                RunState = false;
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

