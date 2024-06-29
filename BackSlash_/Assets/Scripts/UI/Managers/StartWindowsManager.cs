using Scripts.Player;
using System;
using UnityEngine;
using Zenject;

namespace RedMoonGames.Window
{
    public class StartWindowsManager : MonoBehaviour
    {
        [SerializeField] private WindowHandler _startWindowHandler;

        private UIController _uiController;
        private WindowService _windowService;

        private WindowHandler _currentWindow;

        //public event Action<WindowHandler> OnWindowClosed;

        [Inject]
        private void Construct(UIController uiController, WindowService windowService)
        {
            _uiController = uiController;
            _windowService = windowService;

            _uiController.OnEscapeKeyPressed += OpenStartWindow;
        }

        private void Start()
        {
            OpenStartWindow();
        }

        private void OpenStartWindow()
        {
            OpenWindow(_startWindowHandler);
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
            _uiController.OnEscapeKeyPressed -= OpenStartWindow;
        }
    }
}