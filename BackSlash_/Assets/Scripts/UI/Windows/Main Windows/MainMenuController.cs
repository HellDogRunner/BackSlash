using Scripts.Player;
using System.Collections;
using UnityEngine;
using Zenject;

namespace RedMoonGames.Window
{
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField] private WindowHandler _startWindow;
        [SerializeField] private WindowHandler _mainWindow;
        [Space]
        [SerializeField] private float _inputDelay = 1f;

        private SceneTransitionService _sceneTransition;
        private WindowService _windowService;
        private UIPauseInputs _pauseInputs;

        private bool _mainWindowOpened;
        private bool _windowOpening;

        [Inject]
        private void Construct(SceneTransitionService sceneTransition, WindowService windowService, UIPauseInputs pauseInputs)
        {
            _windowService = windowService;
            _windowService.OnSwitchScene += ChangeScene;

            _sceneTransition = sceneTransition;
            _sceneTransition.OnWindowHide += SceneTransitionHide;

            _pauseInputs = pauseInputs;
            _pauseInputs.OnHideCursor += SwitchVisible;
            _pauseInputs.OnAnyKeyboardKeyPressed += ShowMainWindow;
            _pauseInputs.OnEscapeKeyPressed += ShowStartWindow;
        }

        private void Awake()
        {
            _sceneTransition.gameObject.SetActive(true);

            _pauseInputs.enabled = true;

            UnpauseGame();
        }

        private void Start()
        {
            _windowService.ShowWindow(_startWindow);
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

            _windowService.ShowWindow(window);
            _mainWindowOpened = window == _mainWindow;

            _windowOpening = false;
        }

        private void SceneTransitionHide()
        {
            _sceneTransition.gameObject.SetActive(false);
        }

        private void ChangeScene(string sceneName)
        {
            PrepareToChangeScene();
            _sceneTransition.gameObject.SetActive(true);
            _sceneTransition.SwichToScene(sceneName);
        }

        private void PrepareToChangeScene()
        {
            _pauseInputs.enabled = false;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void SwitchVisible(bool visible)
        {
            Cursor.visible = visible;
        }

        private void UnpauseGame()
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            Time.timeScale = 1;
        }

        private void OnDestroy()
        {
            _pauseInputs.OnHideCursor -= SwitchVisible;
            _pauseInputs.OnAnyKeyboardKeyPressed -= ShowMainWindow;
            _pauseInputs.OnEscapeKeyPressed -= ShowStartWindow;

            _windowService.OnSwitchScene -= ChangeScene;
            _sceneTransition.OnWindowHide -= SceneTransitionHide;
        }
    }
}