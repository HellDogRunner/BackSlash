using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Scripts.Player
{
    public class UIController : MonoBehaviour
    {
        private UIControls _playerControls;

        public event Action OnEnterKeyPressed;
        public event Action OnEscapeKeyPressed;
        public event Action OnTabPressed;
        public event Action<bool> OnAnyUIKeyPressed;
        public event Action<bool> OnMousePoint;
        public event Action OnBackKeyPressed;
        public event Action OnAnyKeyboardKeyPressed;
        public event Action OnInventoryKeyPressed;

        [Inject]
        private void Construct()
        {
            _playerControls = new UIControls();

            _playerControls.UI.Enter.performed += Enter;
            _playerControls.UI.Escape.performed += Escape;
            _playerControls.UI.TabsNavigation.performed += TabsNavigation;
            _playerControls.UI.Point.performed += MousePointChange;
            _playerControls.UI.Back.performed += Back;

            _playerControls.UI.AnyInput.performed += AnyKeyboardKey;

            _playerControls.UI.Enter.performed += AnyUIKey;
            _playerControls.UI.Navigate.performed += AnyUIKey;
            _playerControls.UI.TabsNavigation.performed += AnyUIKey;

            _playerControls.UI.Inventory.performed += Inventory;
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
            OnTabPressed?.Invoke();
        }

        private void AnyUIKey(InputAction.CallbackContext context)
        {
            OnAnyUIKeyPressed?.Invoke(true);
        }

        private void MousePointChange(InputAction.CallbackContext context)
        {
            OnMousePoint?.Invoke(true);
        }

        private void Back(InputAction.CallbackContext context)
        {
            OnBackKeyPressed?.Invoke();
        }

        private void AnyKeyboardKey(InputAction.CallbackContext context)
        {
            OnAnyKeyboardKeyPressed?.Invoke();
        }

        private void Inventory(InputAction.CallbackContext context)
        {
            OnInventoryKeyPressed?.Invoke();
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
