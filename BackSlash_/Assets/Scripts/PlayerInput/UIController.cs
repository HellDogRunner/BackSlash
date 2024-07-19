using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;
using static UnityEngine.InputSystem.InputAction;

namespace Scripts.Player
{
    public class UIController : MonoBehaviour
    {
        private UIControls _playerControls;

        public event Action OnEnterKeyPressed;
        public event Action OnEscapeKeyPressed;
        public event Action<string> OnTabPressed;
        public event Action OnAnyKeyPressed;
        public event Action OnMousePoint;

        [Inject]
        private void Construct()
        {
            _playerControls = new UIControls();

            _playerControls.UI.Enter.performed += Enter;
            _playerControls.UI.Escape.performed += Escape;
            _playerControls.UI.TabsNavigation.performed += TabsNavigation;

            _playerControls.UI.Enter.performed += AnyKeyboard;
            _playerControls.UI.Navigate.performed += AnyKeyboard;
            _playerControls.UI.TabsNavigation.performed += AnyKeyboard;

            _playerControls.UI.Point.performed += MousePointChange;
        }

        private void Enter(InputAction.CallbackContext context)
        {
            OnEnterKeyPressed?.Invoke();
        }

        private void Escape(InputAction.CallbackContext context)
        {
            OnEscapeKeyPressed?.Invoke();
        }

        private void TabsNavigation(InputAction.CallbackContext context)
        {
            OnTabPressed?.Invoke(context.control.name);
        }

        private void AnyKeyboard(InputAction.CallbackContext context)
        {
            OnAnyKeyPressed?.Invoke();
        }

        private void MousePointChange(InputAction.CallbackContext context)
        {
            OnMousePoint?.Invoke();
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
