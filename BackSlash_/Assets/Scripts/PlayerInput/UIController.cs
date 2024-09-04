using System;
using UnityEngine;
using UnityEngine.InputSystem;

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

        private void Awake()
        {
            _playerControls = new UIControls();
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

        private void SubscribeToActions()
        {
            _playerControls.UI.Enter.performed += Enter;
            _playerControls.UI.Escape.performed += Escape;
            _playerControls.UI.TabsNavigation.performed += TabsNavigation;
            _playerControls.UI.Point.performed += MousePointChange;
            _playerControls.UI.Back.performed += Back;

            _playerControls.UI.AnyInput.performed += AnyKeyboardKey;

            _playerControls.UI.Enter.performed += AnyUIKey;
            _playerControls.UI.Navigate.performed += AnyUIKey;
            _playerControls.UI.TabsNavigation.performed += AnyUIKey;
        }

        private void UnsubscribeToActions()
        {
            _playerControls.UI.Enter.performed -= Enter;
            _playerControls.UI.Escape.performed -= Escape;
            _playerControls.UI.TabsNavigation.performed -= TabsNavigation;
            _playerControls.UI.Point.performed -= MousePointChange;
            _playerControls.UI.Back.performed -= Back;

            _playerControls.UI.AnyInput.performed -= AnyKeyboardKey;

            _playerControls.UI.Enter.performed -= AnyUIKey;
            _playerControls.UI.Navigate.performed -= AnyUIKey;
            _playerControls.UI.TabsNavigation.performed -= AnyUIKey;
        }

        private void OnEnable()
        {
            _playerControls.Enable();
            SubscribeToActions();
        }

        private void OnDisable()
        {
            _playerControls.Disable();
            UnsubscribeToActions();
        }
    }
}