using Scripts.Player;
using System;
using UnityEngine;
using Zenject;

namespace RedMoonGames.Window
{
    public class GameWindowsController : MonoBehaviour
    {
        [SerializeField] private WindowHandler _pauseHandler;

        [SerializeField] private WindowHandler _currentWindow;

        private WindowService _windowService;
        private UIController _uiController;
        private InputController _controller;

        public event Action<WindowHandler> OnUnpausing;
        public event Action OnPausing;
        public event Action OnHUDHide;
        public event Action OnHUDShow;

        [Inject]
        private void Construct(UIController uiController, InputController inputController, WindowService windowService)
        {
            _uiController = uiController;
            _uiController.OnEscapeKeyPressed += PausePressed;
            _uiController.OnAnyUIKeyPressed += DisableCursor;
            _uiController.OnMousePoint += EnableCursor;
            _uiController.enabled = false;

            _controller = inputController;
            _controller.OnPauseKeyPressed += PausePressed;

            _windowService = windowService;
        }

        private void Awake()
        {
            _currentWindow = _pauseHandler;
            DisableCursor(false);
        }

        public void PausePressed()
        {
            var currentWindow = _windowService.ReturnWindow(_currentWindow);

            if (currentWindow == null)
            {
                OpenWindow(_currentWindow);
                OnPausing?.Invoke();
                OnHUDHide?.Invoke();

                SwitchPause(true);
            }
            else
            {
                OnUnpausing?.Invoke(_currentWindow);
                OnHUDShow?.Invoke();
                _currentWindow = _pauseHandler;

                SwitchPause(false);
            }
        }

        public void SwitchPause(bool isPausing)
        {
            if (isPausing)
            {
                Time.timeScale = 0f;
                EnableCursor(false);
                _controller.enabled = false;
            }
            else
            {
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
            _controller.OnPauseKeyPressed -= PausePressed;
            _uiController.OnEscapeKeyPressed -= PausePressed;
            _uiController.OnAnyUIKeyPressed -= DisableCursor;
            _uiController.OnMousePoint -= EnableCursor;
        }
    }
}