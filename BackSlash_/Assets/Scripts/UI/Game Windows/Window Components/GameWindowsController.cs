using Scripts.Player;
using System;
using UnityEngine;
using Zenject;

namespace RedMoonGames.Window
{
    public class GameWindowsController : MonoBehaviour
    {
        [SerializeField] private WindowHandler _pauseHandler;
        [SerializeField] private WindowHandler _playerMenuHandler;

        [SerializeField] private WindowHandler _currentWindow;

        private WindowService _windowService;
        private UIController _uiController;
        private InputController _controller;

        public event Action<WindowHandler> OnUnpausing;
        public event Action OnHUDHide;
        public event Action OnHUDShow;

        [Inject]
        private void Construct(UIController uiController, InputController inputController, WindowService windowService)
        {
            _uiController = uiController;
            _uiController.OnEscapeKeyPressed += SwitchPause;
            _uiController.OnAnyUIKeyPressed += DisableCursor;
            _uiController.OnMousePoint += EnableCursor;
            _uiController.enabled = false;

            _controller = inputController;
            _controller.OnPauseKeyPressed += PausePressed;
            _controller.OnInventoryKeyPressed += InventoryPressed;

            _windowService = windowService;

            DisableCursor(false);
        }

        private void InventoryPressed()
        {
            _currentWindow = _playerMenuHandler;
            SwitchPause();
        }

        private void PausePressed()
        {
            _currentWindow = _pauseHandler;
            SwitchPause();
        }

        public void SwitchPause()
        {
            var currentWindow = _windowService.ReturnWindow(_currentWindow);

            if (currentWindow == null)
            {
                OpenWindow(_currentWindow);
                OnHUDHide?.Invoke();

                Time.timeScale = 0f;
                EnableCursor(false);
                _controller.enabled = false;
                _uiController.enabled = true;
            }
            else
            {
                OnUnpausing?.Invoke(_currentWindow);
                OnHUDShow?.Invoke();
                //_currentWindow = _pauseHandler;

                Time.timeScale = 1f;
                DisableCursor(false);
                _controller.enabled = true;
                _uiController.enabled = false;
            }
        }

        public void OpenWindow(WindowHandler handler)
        {
            _windowService.TryShowWindow(handler);
            _currentWindow = handler;
        }

        public void CloseWindow(WindowHandler handler)
        {
            var currentWindow = _windowService.ReturnWindow(handler);
            currentWindow?.Close();
        }

        private void DisableCursor(bool isEvent)
        {
            if (!isEvent)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
            Cursor.visible = false;
        }

        private void EnableCursor(bool isEvent)
        {
            if (!isEvent)
            {
                Cursor.lockState = CursorLockMode.Confined;
            }
            Cursor.visible = true;
        }

        private void OnDestroy()
        {
            _uiController.OnInventoryKeyPressed += InventoryPressed;
            _uiController.OnEscapeKeyPressed -= SwitchPause;
            _uiController.OnAnyUIKeyPressed -= DisableCursor;
            _uiController.OnMousePoint -= EnableCursor;

            _controller.OnPauseKeyPressed -= SwitchPause;
        }
    }
}