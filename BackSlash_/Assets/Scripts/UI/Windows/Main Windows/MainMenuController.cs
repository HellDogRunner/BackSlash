using RedMoonGames.Window;
using UnityEngine;

public class MainMenuController : BasicMenuController
{
	[SerializeField] private WindowHandler _startHandler;
	[SerializeField] private WindowHandler _mainHandler;

	private void Awake()
	{
		UnpauseGame();
		_windowService.TryOpenWindow(_startHandler);
		_windowService.ShowWindow(_startHandler);
	}

	private void OnEnable()
	{
		_uiInputs.OnEscapeKeyPressed += OpenMainWindow;
	}

	private void OnDisable()
	{
		_uiInputs.OnEscapeKeyPressed -= OpenMainWindow;
	}

	private void OpenMainWindow()
	{
		var startWindow = _windowService.GetWindowByHandler(_startHandler);
		var mainWindow = _windowService.GetWindowByHandler(_mainHandler);

		if (startWindow == null && mainWindow == null)
		{
			_windowService.TryOpenWindow(_mainHandler);
		}
	}

	private void UnpauseGame()
	{
		Cursor.lockState = CursorLockMode.Confined;
		Cursor.visible = true;
		Time.timeScale = 1;
	}
}
