using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Scripts.Player
{
    public class SlasherInput : MonoBehaviour
    {
        private GameControls _controls;

        public Vector2 Move { get; private set; }
        public bool SprintHeld { get; private set; }
        public bool JumpHeld { get; private set; }

        public event Action JumpPressed;
        public event Action JumpReleased;
        public event Action DodgePressed;
        public event Action SlidePressed;

        public event Action LightPressed;
        public event Action HeavyPressed;

        public event Action BootsTogglePressed;

        private void Awake()
        {
            _controls = new GameControls();
        }

        private void OnEnable()
        {
            _controls.Enable();

            _controls.Gameplay.WASD.performed += OnMove;
            _controls.Gameplay.WASD.canceled += OnMove;

            _controls.Gameplay.Sprint.started += _ => SprintHeld = true;
            _controls.Gameplay.Sprint.canceled += _ => SprintHeld = false;

            _controls.Gameplay.Jump.started += _ =>
            {
                JumpHeld = true;
                JumpPressed?.Invoke();
            };
            _controls.Gameplay.Jump.canceled += _ =>
            {
                JumpHeld = false;
                JumpReleased?.Invoke();
            };

            _controls.Gameplay.Dodge.performed += _ => DodgePressed?.Invoke();
            _controls.Gameplay.Block.performed += _ => SlidePressed?.Invoke();

            _controls.Gameplay.LightAttack.started += _ => LightPressed?.Invoke();
            _controls.Gameplay.HeavyAttack.started += _ => HeavyPressed?.Invoke();

            // Toggle boots (временно на ShowWeapon)
            _controls.Gameplay.ShowWeapon.started += _ => BootsTogglePressed?.Invoke();
        }

        private void OnDisable()
        {
            _controls.Disable();
            Move = Vector2.zero;
            SprintHeld = false;
            JumpHeld = false;
        }

        private void OnMove(InputAction.CallbackContext ctx)
        {
            Move = ctx.ReadValue<Vector2>();
        }
    }
}
