using System.Collections;
using UnityEngine;

namespace RedMoonGames.Window
{
    public class MainMenuController : BasicMenuController
    {
        [SerializeField] private WindowHandler _startWindow;
        [SerializeField] private WindowHandler _mainWindow;
        [Space]
        [SerializeField] private float _inputDelay = 1f;

        private bool _mainWindowOpened;
        private bool _windowOpening;

        private void Awake()
        {
            _windowAnimation.OnAnimationComplete += CloseWindow;

            _sceneTransition.OnWindowHide += SceneTransitionHide;

            _pauseInputs.ShowCursor += SwitchVisible;
            _pauseInputs.OnAnyKeyPressed += ShowMainWindow;
            _pauseInputs.OnEscapeKeyPressed += ShowStartWindow;

            _sceneTransition.gameObject.SetActive(true);

            _pauseInputs.enabled = true;

            UnpauseGame();
        }

        private void Start()
        {
            _mainWindowOpened = true;
            ShowStartWindow();
        }

        private void ShowStartWindow()
        {
            if (!_windowOpening && _mainWindowOpened) StartCoroutine(ShowWindowDelay(_startWindow));
        }

        private void ShowMainWindow()
        {
            if (!_windowOpening && !_mainWindowOpened) StartCoroutine(ShowWindowDelay(_mainWindow));
        }

        IEnumerator ShowWindowDelay(WindowHandler window)
        {
            _windowOpening = true;

            _windowService.HideWindow();

            yield return new WaitForSeconds(_inputDelay);

            _mainWindowOpened = window == _mainWindow;
            _windowService.TryOpenWindow(window);
            _windowService.ShowWindow();

            _windowOpening = false;
        }

        private void UnpauseGame()
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            Time.timeScale = 1;
        }

        private void OnDestroy()
        {
            _pauseInputs.ShowCursor -= SwitchVisible;
            _pauseInputs.OnAnyKeyPressed -= ShowMainWindow;
            _pauseInputs.OnEscapeKeyPressed -= ShowStartWindow;

            _sceneTransition.OnWindowHide -= SceneTransitionHide;

            _windowAnimation.OnAnimationComplete -= CloseWindow;
        }
    }
}