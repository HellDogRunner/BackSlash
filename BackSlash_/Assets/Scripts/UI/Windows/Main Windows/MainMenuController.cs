using UnityEngine;

namespace RedMoonGames.Window
{
	public class MainMenuController : BasicMenuController
	{
		[SerializeField] private WindowHandler _startHandler;
		[SerializeField] private WindowHandler _mainHandler;

		private void Awake()
		{
			_sceneTransition.gameObject.SetActive(true);

			_uiInputs.enabled = true;

			UnpauseGame();
			_windowService.TryOpenWindow(_startHandler);
		}

		private void OnEnable()
		{
			_sceneTransition.OnWindowHide += SceneTransitionHide;
			_uiInputs.OnEscapeKeyPressed += OpenStartWindow;
		}

		private void OnDisable()
		{
			_sceneTransition.OnWindowHide -= SceneTransitionHide;
			_uiInputs.OnEscapeKeyPressed -= OpenStartWindow;
		}

		private void OpenStartWindow()
		{
			var window = _windowService.GetWindowByHandler(_mainHandler);
			if (window == null)
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
}
