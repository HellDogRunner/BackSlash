using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Scripts.UI
{
    public class UIController : MonoBehaviour
    {
        private UIControls _playerControls;

        public event Action OnEnterKeyPressed;
        public event Action OnEscapeKeyPressed;

        [Inject]
        private void Construct()
        {
            _playerControls = new UIControls();

            _playerControls.UI.Enter.performed += Enter;
            _playerControls.UI.Escape.performed += Escape;
        }

        private void Enter(InputAction.CallbackContext context)
        {
            OnEnterKeyPressed?.Invoke();
        }

        private void Escape(InputAction.CallbackContext context)
        {
            OnEscapeKeyPressed?.Invoke();
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
