using UnityEngine;

namespace RedMoonGames.Window
{
	public class MainMenuController : BasicMenuController
	{
		[SerializeField] private WindowHandler _startWindow;
		[SerializeField] private WindowHandler _mainWindow;

		private void Awake()
		{
			_sceneTransition.gameObject.SetActive(true);

			_pauseInputs.enabled = true;

			UnpauseGame();
			ShowStartWindow();
		}

		private void OnEnable()
		{
			_sceneTransition.OnWindowHide += SceneTransitionHide;
			_pauseInputs.OnEscapeKeyPressed += ShowStartWindow;
			_animator.OnWindowDelayShowed += SwitchEvents;
			_animator.OnWindowHided += CloseWindow;
		}

		private void OnDisable()
		{
			_sceneTransition.OnWindowHide -= SceneTransitionHide;
			_pauseInputs.OnAnyKeyPressed -= ShowMainWindow;
			_pauseInputs.OnEscapeKeyPressed -= ShowStartWindow;
			_animator.OnWindowDelayShowed -= SwitchEvents;
			_animator.OnWindowHided -= CloseWindow;
		}

		private void ShowStartWindow()
		{
			_windowService.CloseActiveWindow();
			_windowService.TryOpenWindow(_startWindow);
		}

		private void ShowMainWindow()
		{
			_windowService.CloseActiveWindow();
			_windowService.TryOpenWindow(_mainWindow);
		}

		private void SwitchEvents()
		{
			if (_windowService.GetActiveWindow() == _startWindow)
			{
				_pauseInputs.OnEscapeKeyPressed -= ShowStartWindow;
				_pauseInputs.OnAnyKeyPressed += ShowMainWindow;
			}
			
			if (_windowService.GetActiveWindow() == _mainWindow)
			{
				_pauseInputs.OnEscapeKeyPressed += ShowStartWindow;
				_pauseInputs.OnAnyKeyPressed -= ShowMainWindow;
			}
		}

		private void UnpauseGame()	// Remove from this script
		{
			Cursor.lockState = CursorLockMode.Confined;
			Cursor.visible = true;
			Time.timeScale = 1;
		}
	}
}