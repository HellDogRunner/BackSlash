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
        public event Action<bool> OnAnyKeyPressed;
        public event Action<bool> OnMousePoint;
        public event Action OnBackKeyPressed;

        [Inject]
        private void Construct()
        {
            _playerControls = new UIControls();

            _playerControls.UI.Enter.performed += Enter;
            _playerControls.UI.Escape.performed += Escape;
            _playerControls.UI.TabsNavigation.performed += TabsNavigation;
            _playerControls.UI.Point.performed += MousePointChange;
            _playerControls.UI.Back.performed += Back;

            _playerControls.UI.Enter.performed += AnyKeyboard;
            _playerControls.UI.Navigate.performed += AnyKeyboard;
            _playerControls.UI.TabsNavigation.performed += AnyKeyboard;
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

        private void AnyKeyboard(InputAction.CallbackContext context)
        {
            OnAnyKeyPressed?.Invoke(true);
        }

        private void MousePointChange(InputAction.CallbackContext context)
        {
            OnMousePoint?.Invoke(true);
        }

        private void Back(InputAction.CallbackContext context)
        {
            OnBackKeyPressed?.Invoke();
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
