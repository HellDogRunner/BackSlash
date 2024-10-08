using Scripts.Player;
using System;
using UnityEngine;
using Zenject;

namespace RedMoonGames.Window
{
    public class GameMenuController : BasicMenuController
    {
        [SerializeField] private WindowHandler _pauseWindow;
        [SerializeField] private WindowHandler _menuWindow;

        private bool _inPlayerMenu;

        private HUDAnimationService _hUDAnimation;
        private InputController _gameInputs;

        public event Action<int> OpenTab;

        [Inject]
        private void Construct(InputController gameInputs, HUDAnimationService hUDAnimation)
        {
            Debug.Log("GameMenuController");
            _hUDAnimation = hUDAnimation;
            _gameInputs = gameInputs;
        }

        private void Awake()
        {
            _pauseInputs.ShowCursor += SwitchVisible;

            _windowAnimation.OnAnimationComplete += CloseWindow;
            _sceneTransition.OnWindowHide += SceneTransitionHide;

            _pauseInputs.OnMenuTabPressed += OpenMenu;
            _pauseInputs.OnEscapeKeyPressed += OpenPause;

            _sceneTransition.gameObject.SetActive(true);
            _hUDAnimation.gameObject.SetActive(true);

            _gameInputs.enabled = true;
            _pauseInputs.enabled = true;

            UnpauseGame();
        }

        private void OpenPause()
        {
            EventsOnPause(); 
            PauseGame();

            _windowService.TryOpenWindow(_pauseWindow);
            _windowService.ShowWindow();
        }

        private void HideWindow()
        {
            _inPlayerMenu = false;

            EventsOnGame();
            UnpauseGame();

            _windowService.HideWindow();
        }

        private void OpenMenu(int index)
        {
            if (!_inPlayerMenu)
            {
                _inPlayerMenu = true;

                PauseGame();
                EventsOnPause(); 

                _windowService.TryOpenWindow(_menuWindow);
                _windowService.ShowWindow();
            }

            OpenTab?.Invoke(index);
        }

        private void PauseGame()
        {
            _hUDAnimation.gameObject.SetActive(false);
            _gameInputs.enabled = false;

            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            Time.timeScale = 0;
        }

        private void UnpauseGame()
        {
            _hUDAnimation.gameObject.SetActive(true);
            _gameInputs.enabled = true;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1;
        }

        private void EventsOnPause()
        {
            _pauseInputs.OnEscapeKeyPressed -= OpenPause;
            _pauseInputs.OnMenuTabPressed -= OpenMenu;
            _pauseInputs.OnEscapeKeyPressed += HideWindow;
            _windowService.OnUnpause += HideWindow;
        }

        private void EventsOnGame()
        {
            _pauseInputs.OnEscapeKeyPressed += OpenPause;
            _pauseInputs.OnMenuTabPressed += OpenMenu;
            _pauseInputs.OnEscapeKeyPressed -= HideWindow;
            _windowService.OnUnpause -= HideWindow;
        }

        private void OnDestroy()
        {
            _pauseInputs.ShowCursor -= SwitchVisible;

            _windowAnimation.OnAnimationComplete -= CloseWindow;
            _sceneTransition.OnWindowHide -= SceneTransitionHide;

            _windowService.OnUnpause -= HideWindow;
            _pauseInputs.OnEscapeKeyPressed -= HideWindow;

            _pauseInputs.OnMenuTabPressed -= OpenMenu;
        }
    }
}