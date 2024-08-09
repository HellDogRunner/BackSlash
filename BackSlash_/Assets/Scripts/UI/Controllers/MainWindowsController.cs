using Scripts.Player;
using UnityEngine;
using Zenject;

namespace RedMoonGames.Window
{
    public class MainWindowsController : MonoBehaviour
    {
        [SerializeField] private WindowHandler _startHandler;

        private UIController _uiController;
        private WindowService _windowService;

        private WindowHandler _currentWindow;

        [Inject]
        private void Construct(UIController uiController, WindowService windowService)
        {
            _uiController = uiController;
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