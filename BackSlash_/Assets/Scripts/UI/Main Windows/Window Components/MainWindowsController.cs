using Scripts.Player;
using UnityEngine;
using Zenject;

namespace RedMoonGames.Window
{
    public class MainWindowsController : MonoBehaviour
    {
        [SerializeField] private WindowHandler _startHandler;

        private WindowHandler _currentWindow;

        private SceneTransitionService _sceneTransition;
        //private WindowService _windowService;
        private UIPauseInputs _inputs;

        [Inject]
        private void Construct(SceneTransitionService sceneTransition, UIPauseInputs inputs)
        {
            //_windowService = windowService;

            _sceneTransition = sceneTransition;
            _sceneTransition.OnLoading += DisableInput;
            _inputs = inputs;
        }

        private void Start()
        {
            OpenWindow(_startHandler);
        }

        public void SwitchWindows(WindowHandler close, WindowHandler open)
        {
            CloseWindow(close);
            OpenWindow(open);
        }

        public void OpenWindow(WindowHandler handler)
        {
            //_windowService.TryShowWindow(handler);
            _currentWindow = handler;
        }

        public void CloseWindow(WindowHandler handler)
        {
            //var currentWindow = ReturnWindow(handler);
            //currentWindow?.Close();
        }

        private void DisableInput()
        {
            _inputs.enabled = false;
        }
    }
}