using Scripts.Player;
using System;
using UnityEngine;
using Zenject;

namespace RedMoonGames.Window
{
    public class GameWindowsController : MonoBehaviour
    {
        [SerializeField] private WindowHandler _pauseWindowHandler;

        private UIController _uiController;
        private InputController _controller;
        private WindowService _windowService;

        private WindowHandler _currentWindow;

        public event Action OnUnpausing;
        public event Action OnPausing;

        public event Action OnHUDHide;
        public event Action OnHUDShow;

        [Inject]
        private void Construct(UIController uiController, InputController inputController, WindowService windowService)
        {
            _uiController = uiController;
            _uiController.OnEscapeKeyPressed += PauseSwitch;
            _uiController.OnAnyKeyPressed += DisableCursor;
            _uiController.OnMousePoint += EnableCursor;
            _uiController.enabled = false;

            _controller = inputController;
            _controller.OnMenuKeyPressed += PauseSwitch;

            _windowService = windowService;

            _currentWindow = _pauseWindowHandler;
            DisableCursor(false);
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
                EnableCursor(false);
                _controller.enabled = false;
                _uiController.enabled = true;
            }
            else
            {
                OnUnpausing?.Invoke();
                OnHUDShow?.Invoke();
                _currentWindow = _pauseWindowHandler;

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
            _controller.OnMenuKeyPressed -= PauseSwitch;
            _uiController.OnEscapeKeyPressed -= PauseSwitch;
            _uiController.OnAnyKeyPressed -= DisableCursor;
            _uiController.OnMousePoint -= EnableCursor;
        }
    }
}