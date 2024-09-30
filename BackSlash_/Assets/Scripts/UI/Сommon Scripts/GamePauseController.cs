using RedMoonGames.Window;
using Scripts.Player;
using UnityEngine;
using Zenject;

public class GamePauseController : MonoBehaviour
{
    private GameWindowsController _pauseWindowsController;
    private SceneTransitionService _sceneTransition;
    private PlayerMenuWindow _menuWindowController;
    private HUDAnimationService _hUDAnimation;
    private InputController _gameInput;
    private UIPauseInputs _pauseInput;
    private ComboSystem _comboSystem;
    private UIMenuInputs _menuInput;

    [Inject]
    private void Construct(SceneTransitionService sceneTransition, ComboSystem comboSystem, HUDAnimationService hUDAnimation, InputController gameInput, UIPauseInputs pauseInput, UIMenuInputs menuInput, GameWindowsController pauseController, PlayerMenuWindow menuController)
    {
        _pauseWindowsController = pauseController;
        _menuWindowController = menuController;
        _hUDAnimation = hUDAnimation;
        _comboSystem = comboSystem;
        _pauseInput = pauseInput;

        _sceneTransition = sceneTransition;
        _sceneTransition.OnLoading += DisableAllInputs;

        _gameInput = gameInput;
        _gameInput.OnPauseKeyPressed += OpenPause;

        _menuInput = menuInput;
        _menuInput.OnInventoryPressed += OpenMenu;
        _menuInput.OnCombosPressed += OpenMenu;
        _menuInput.OnAbilitiesPressed += OpenMenu;
        _menuInput.OnSkillsPressed += OpenMenu;
        _menuInput.OnJournalPressed += OpenMenu;
        _menuInput.OnMapPressed += OpenMenu;
    }

    private void Awake()
    {
        _gameInput.enabled = true;
        _menuInput.enabled = true;
        _pauseInput.enabled = false;

        UnpauseGame();
    }

    private void OpenPause()
    {
        _pauseWindowsController.OnPauseHide += ClosePause;
        _pauseInput.OnEscapeKeyPressed += ClosePause;
        _pauseInput.OnHideCursor += SwitchVisible;

        _gameInput.enabled = false;
        _menuInput.enabled = false;
        _pauseInput.enabled = true;

        PauseGame();

        _pauseWindowsController.ShowPauseMenu();
    }

    private void ClosePause()
    {
        _pauseWindowsController.OnPauseHide -= ClosePause;
        _pauseInput.OnEscapeKeyPressed -= ClosePause;
        _pauseInput.OnHideCursor -= SwitchVisible;

        _gameInput.enabled = true;
        _menuInput.enabled = true;
        _pauseInput.enabled = false;

        UnpauseGame();

        _pauseWindowsController.HidePauseMenu();
    }

    private void OpenMenu(int index)
    {
        _menuInput.OnEscapePressed += CloseMenu;
        _menuInput.OnBackPressed += CloseMenu;
        _menuInput.OnHideCursor += SwitchVisible;

        _gameInput.enabled = false;

        PauseGame();

        _menuWindowController.ShowPlayerMenu(index);
    }

    private void CloseMenu()
    {
        _menuInput.OnEscapePressed -= CloseMenu;
        _menuInput.OnBackPressed -= CloseMenu;
        _menuInput.OnHideCursor -= SwitchVisible;

        _gameInput.enabled = true;

        UnpauseGame();

        _menuWindowController.HidePlayerMenu();
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

    private void DisableAllInputs()
    {
        _gameInput.enabled = false;
        _menuInput.enabled = false;
        _pauseInput.enabled = false;
    }

    private void SwitchVisible(bool visible)
    {
        Cursor.visible = visible;
    }

    private void OnDestroy()
    {
        _gameInput.OnPauseKeyPressed -= OpenPause;
        _menuInput.OnInventoryPressed -= OpenMenu;
        _menuInput.OnCombosPressed -= OpenMenu;
        _menuInput.OnAbilitiesPressed -= OpenMenu;
        _menuInput.OnSkillsPressed -= OpenMenu;
        _menuInput.OnJournalPressed -= OpenMenu;
        _menuInput.OnMapPressed -= OpenMenu;

        _menuInput.OnEscapePressed -= CloseMenu;
        _menuInput.OnBackPressed -= CloseMenu;
        _menuInput.OnHideCursor -= SwitchVisible;

        _pauseWindowsController.OnPauseHide -= ClosePause;
        _pauseInput.OnEscapeKeyPressed -= ClosePause;
        _pauseInput.OnHideCursor -= SwitchVisible;

        _sceneTransition.OnLoading += DisableAllInputs;
    }
}