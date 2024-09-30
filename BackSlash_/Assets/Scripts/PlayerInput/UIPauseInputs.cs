using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Scripts.Player
{
    public class UIPauseInputs : MonoBehaviour
    {
        private UIControls _playerControls;

        public event Action OnEnterKeyPressed;
        public event Action OnEscapeKeyPressed;
        public event Action OnTabPressed;
        public event Action<bool> OnHideCursor;
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
            OnHideCursor?.Invoke(false);
        }

        private void MousePointChange(InputAction.CallbackContext context)
        {
            OnHideCursor?.Invoke(true);
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
            _playerControls.Common.Enter.performed += Enter;
            _playerControls.Common.Escape.performed += Escape;
            _playerControls.Common.Back.performed += Back;
            _playerControls.Common.Point.performed += MousePointChange;

            _playerControls.Common.Enter.performed += AnyUIKey;
            _playerControls.Common.Navigate.performed += AnyUIKey;
            _playerControls.Pause.SwitchTab.performed += AnyUIKey;

            _playerControls.Pause.SwitchTab.performed += TabsNavigation;
            _playerControls.Pause.AnyInput.performed += AnyKeyboardKey;
        }

        private void UnsubscribeToActions()
        {
            _playerControls.Common.Enter.performed -= Enter;
            _playerControls.Common.Escape.performed -= Escape;
            _playerControls.Common.Back.performed -= Back;
            _playerControls.Common.Point.performed -= MousePointChange;

            _playerControls.Common.Enter.performed -= AnyUIKey;
            _playerControls.Common.Navigate.performed -= AnyUIKey;
            _playerControls.Pause.SwitchTab.performed -= AnyUIKey;

            _playerControls.Pause.SwitchTab.performed -= TabsNavigation;
            _playerControls.Pause.AnyInput.performed -= AnyKeyboardKey;
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