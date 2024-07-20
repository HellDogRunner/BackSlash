using Scripts.Player;
using System;
using UnityEngine;
using Zenject;

namespace RedMoonGames.Window
{
    public class GameWindowsManager : MonoBehaviour
    {
        [SerializeField] private WindowHandler _pauseWindowHandler;

        private UIController _uiController;
        private InputController _controller;
        private WindowService _windowService;

        [SerializeField] private WindowHandler _currentWindow;

        private Vector2 _lastMousePosition;

        public event Action OnUnpausing;
        public event Action OnPausing;

        public event Action OnHUDHide;
        public event Action OnHUDShow;

        [Inject]
        private void Construct(UIController uiController, InputController inputController, WindowService windowService)
        {
            _uiController = uiController;
            _controller = inputController;
            _windowService = windowService;

            _uiController.enabled = false;

            _currentWindow = _pauseWindowHandler;

            _controller.OnMenuKeyPressed += PauseSwitch;
            _uiController.OnEscapeKeyPressed += PauseSwitch;
        }

        public void PauseSwitch()
        {
            var currentWindow = _windowService.ReturnWindow(_currentWindow);

            if (currentWindow == null)
            {
                OpenWindow(_pauseWindowHandler);
                OnPausing?.Invoke();
                OnHUDHide?.Invoke();

                Time.timeScale = 0f;
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
                _controller.enabled = false;
                _uiController.enabled = true;
            }
            else
            {
                OnUnpausing?.Invoke();
                OnHUDShow?.Invoke();
                _currentWindow = _pauseWindowHandler;

                Time.timeScale = 1f;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
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

        private void DisableCursor()
        {
            Cursor.visible = false;
        }

        private void EnableCursor()
        {
            Cursor.visible = true;
        }

        private void OnDestroy()
        {
            _controller.OnMenuKeyPressed -= PauseSwitch;
        }
    }
}