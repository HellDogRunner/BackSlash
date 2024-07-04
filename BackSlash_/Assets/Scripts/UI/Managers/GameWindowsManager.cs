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

        private WindowHandler _currentWindow;

        public event Action<WindowHandler> OnWindowClosed;
        public event Action OnPausing;

        public event Action OnHUDHide;
        public event Action OnHUDShow;

        [Inject]
        private void Construct(UIController uiController, InputController inputController, WindowService windowService)
        {
            _uiController = uiController;
            _controller = inputController;
            _windowService = windowService;

            _currentWindow = _pauseWindowHandler;

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
            }
            else
            {
                OnWindowClosed?.Invoke(_currentWindow);
                OnHUDShow?.Invoke();

                Time.timeScale = 1f;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                _controller.enabled = true;
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

        private void OnDestroy()
        {
            _controller.OnMenuKeyPressed -= PauseSwitch;
        }
    }
}