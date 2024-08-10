using UnityEngine;
using Zenject;

namespace RedMoonGames.Window
{
    public class MainWindowsController : MonoBehaviour
    {
        [SerializeField] private WindowHandler _startHandler;

        private WindowHandler _currentWindow;

        private WindowService _windowService;

        [Inject]
        private void Construct(WindowService windowService)
        {
            _windowService = windowService;

            OpenWindow(_startHandler);
        }

        public void SwitchWindows(WindowHandler close, WindowHandler open)
        {
            CloseWindow(close);
            OpenWindow(open);
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
    }
}