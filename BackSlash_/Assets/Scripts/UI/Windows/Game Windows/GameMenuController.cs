using Scripts.Player;
using UnityEngine;
using Zenject;

namespace RedMoonGames.Window
{
    public class GameMenuController : MonoBehaviour
    {
        [SerializeField] private WindowHandler _pauseWindow;
        [SerializeField] private WindowHandler _menuWindow;

        private bool _inPlayerMenu;

        private SceneTransitionService _sceneTransition;
        private WindowService _windowService;
        private HUDAnimationService _hUDAnimation;
        private ComboSystem _comboSystem;

        private InputController _gameInputs;
        private UIPauseInputs _pauseInputs;
        private UIMenuInputs _menuInputs;

        [Inject]
        private void Construct(SceneTransitionService sceneTransition, WindowService windowService, ComboSystem comboSystem, HUDAnimationService hUDAnimation, InputController gameInputs, UIPauseInputs pauseInputs, UIMenuInputs menuInputs)
        {
            _hUDAnimation = hUDAnimation;
            _comboSystem = comboSystem;
            _pauseInputs = pauseInputs;

            _sceneTransition = sceneTransition;
            _sceneTransition.OnWindowHide += SceneTransitionHide;

            _windowService = windowService;
            _windowService.OnSwitchScene += ChangeScene;

            _gameInputs = gameInputs;
            _gameInputs.OnPauseKeyPressed += OpenPause;

            _menuInputs = menuInputs;
            _menuInputs.OnInventoryPressed += OpenMenu;
            _menuInputs.OnCombosPressed += OpenMenu;
            _menuInputs.OnAbilitiesPressed += OpenMenu;
            _menuInputs.OnSkillsPressed += OpenMenu;
            _menuInputs.OnJournalPressed += OpenMenu;
            _menuInputs.OnMapPressed += OpenMenu;
        }

        private void Awake()
        {
            _sceneTransition.gameObject.SetActive(true);
            _hUDAnimation.gameObject.SetActive(true);

            _gameInputs.enabled = true;
            _menuInputs.enabled = true;
            _pauseInputs.enabled = false;

            UnpauseGame();
        }

        private void OpenPause()
        {
            _windowService.OnUnpause += ClosePause;
            _pauseInputs.OnEscapeKeyPressed += ClosePause;
            _pauseInputs.OnHideCursor += SwitchVisible;

            _gameInputs.enabled = false;
            _menuInputs.enabled = false;
            _pauseInputs.enabled = true;

            PauseGame();

            _windowService.ShowWindow(_pauseWindow);
        }

        private void ClosePause()
        {
            _windowService.OnUnpause -= ClosePause;
            _pauseInputs.OnEscapeKeyPressed -= ClosePause;
            _pauseInputs.OnHideCursor -= SwitchVisible;

            _gameInputs.enabled = true;
            _menuInputs.enabled = true;
            _pauseInputs.enabled = false;

            UnpauseGame();

            _windowService.HideWindow();
        }

        private void OpenMenu(int index)
        {
            if (!_inPlayerMenu)
            {
                _inPlayerMenu = true;

                _menuInputs.OnEscapePressed += CloseMenu;
                _menuInputs.OnBackPressed += CloseMenu;
                _menuInputs.OnHideCursor += SwitchVisible;

                _menuInputs.enabled = true;
                _gameInputs.enabled = false;
                _pauseInputs.enabled = false;

                PauseGame();

                _windowService.ShowWindow(_menuWindow);
            }

            //_menuController.OpenTab(index);
        }

        private void CloseMenu()
        {
            _inPlayerMenu = false;

            _menuInputs.OnEscapePressed -= CloseMenu;
            _menuInputs.OnBackPressed -= CloseMenu;
            _menuInputs.OnHideCursor -= SwitchVisible;

            _menuInputs.enabled = true;
            _gameInputs.enabled = true;
            _pauseInputs.enabled = false;

            UnpauseGame();

            _windowService.HideWindow();
        }

        private void PauseGame()
        {
            _comboSystem.IsPause = true;

            _hUDAnimation.HideOnPause();

            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            Time.timeScale = 0;
        }

        private void UnpauseGame()
        {
            _comboSystem.IsPause = false;

            _hUDAnimation.ShowOnUnpause();

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1;
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
            _gameInputs.enabled = false;
            _menuInputs.enabled = false;
            _pauseInputs.enabled = false;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void SwitchVisible(bool visible)
        {
            Cursor.visible = visible;
        }

        private void OnDestroy()
        {
            _gameInputs.OnPauseKeyPressed -= OpenPause;
            _menuInputs.OnInventoryPressed -= OpenMenu;
            _menuInputs.OnCombosPressed -= OpenMenu;
            _menuInputs.OnAbilitiesPressed -= OpenMenu;
            _menuInputs.OnSkillsPressed -= OpenMenu;
            _menuInputs.OnJournalPressed -= OpenMenu;
            _menuInputs.OnMapPressed -= OpenMenu;

            _menuInputs.OnEscapePressed -= CloseMenu;
            _menuInputs.OnBackPressed -= CloseMenu;
            _menuInputs.OnHideCursor -= SwitchVisible;

            _windowService.OnUnpause -= ClosePause;
            _pauseInputs.OnEscapeKeyPressed -= ClosePause;
            _pauseInputs.OnHideCursor -= SwitchVisible;

            _windowService.OnSwitchScene -= ChangeScene;
            _sceneTransition.OnWindowHide -= SceneTransitionHide;
        }
    }
}