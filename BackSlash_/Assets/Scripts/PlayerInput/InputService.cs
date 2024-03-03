using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;
using System;

namespace Scripts.Player
{
    public class InputService : MonoBehaviour
    {
        private InputAction Movement;
        [SerializeField] private InputActionAsset PlayerControls;

        public Action<Vector3> OnDirectionChanged;

        public Vector3 MoveDirection;

        [Inject]
        private void Construct()
        {
            var gamePlyayActionMap = PlayerControls.FindActionMap("Gameplay");
            Movement = gamePlyayActionMap.FindAction("WASD");
            Movement.performed += ChagneDirection;
            Movement.canceled += ChagneDirection;
        }

        private void ChagneDirection(InputAction.CallbackContext context)
        {
            var direction = context.ReadValue<Vector3>();
            MoveDirection = new Vector3(direction.x, direction.z, direction.y);
            OnDirectionChanged?.Invoke(MoveDirection);
        }

        private void OnEnable()
        {
            Movement.Enable();
        }

        private void OnDisable()
        {
            Movement.Disable();
        }
    }
}

