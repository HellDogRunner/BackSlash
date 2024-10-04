using RedMoonGames.Window;
using Scripts.Player;
using System.Collections;
using UnityEngine;
using Zenject;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private WindowHandler _startWindow;
    [SerializeField] private WindowHandler _mainWindow;
    [Space]
    [SerializeField] private float _inputDelay = 1f;

    private WindowService _windowService;
    private UIPauseInputs _pauseInputs;

    private bool _mainWindowOpened;
    private bool _windowOpening;

    [Inject]
    private void Construct(WindowService windowService, UIPauseInputs pauseInputs)
    {
        _windowService = windowService;

        _pauseInputs = pauseInputs;
        _pauseInputs.OnHideCursor += SwitchVisible;
        _pauseInputs.OnAnyKeyboardKeyPressed += ShowMainWindow;
        _pauseInputs.OnEscapeKeyPressed += ShowStartWindow;
    }

    private void Awake()
    {
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
    }
}