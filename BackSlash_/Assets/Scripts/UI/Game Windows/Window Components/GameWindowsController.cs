using System;
using UnityEngine;
using Zenject;

namespace RedMoonGames.Window
{
    public class GameWindowsController : MonoBehaviour
    {
        [SerializeField] private WindowHandler _pauseHandler;

        private WindowHandler _currentWindow;

        private WindowService _windowService;

        public event Action<WindowHandler> OnUnpausing;
        public event Action OnPausing;
        public event Action OnPauseHide;

        [Inject]
        private void Construct(WindowService windowService)
        {
            _windowService = windowService;
        }

        private void Awake()
        {
            _currentWindow = _pauseHandler;
        }

        public void Unpause()
        {
            OnPauseHide?.Invoke();
        }

        public void ShowPauseMenu()
        {
            OpenWindow(_currentWindow);
            OnPausing?.Invoke();
        }

        public void HidePauseMenu()
        {
            OnUnpausing?.Invoke(_currentWindow);
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
            _currentWindow = _pauseHandler;
        }
    }
}