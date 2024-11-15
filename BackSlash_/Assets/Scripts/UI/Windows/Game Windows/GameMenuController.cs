using Scripts.Player;
using System;
using System.Collections;
using UnityEngine;
using Zenject;

namespace RedMoonGames.Window
{
	public class GameMenuController : BasicMenuController
	{
		[SerializeField] private WindowHandler _pauseWindow;
		[SerializeField] private WindowHandler _menuWindow;
		[Space]
		[SerializeField] private float _inputsDelay = 1;
		[SerializeField] private bool _setLowPreset;

		private bool _inInteracting;

		private HUDController _hudController;
		private InputController _gameInputs;

		public event Action<int> OpenTab;
		public event Action<bool> OnGamePause;
		
		[Inject]
		private void Construct(HUDController hudController, InputController gameInputs)
		{
			_hudController = hudController;
			_gameInputs = gameInputs;
		}

		private void Awake()
		{
			_pauseInputs.ShowCursor += SwitchVisible;

			_animator.OnWindowHided += CloseWindow;
			_sceneTransition.OnWindowHide += SceneTransitionHide;

			_pauseInputs.OnMenuTabKeyPressed += OpenMenu;
			_pauseInputs.OnEscapeKeyPressed += OpenPause;

			_sceneTransition.gameObject.SetActive(true);
			_hudController.gameObject.SetActive(true);

			SwitchPause(false);
			StartCoroutine(EnableInputsDelay());

			if (_setLowPreset) SetLowPreset();
		}

		private void OpenPause()
		{
			EventsOnPause();
			SwitchPause(true);

			_windowService.CloseActiveWindow();
			_windowService.TryOpenWindow(_pauseWindow);
			_windowService.ShowWindow();
		}

		private void HideWindow()
		{
			var window = _windowService.GetActiveWindow();
			_windowService.HideWindow(window);
			
			EventsOnGame();
			SwitchPause(false);
		}

		private void OpenMenu(int index)
		{
			if (_windowService.GetActiveWindow() != _menuWindow)
			{
				SwitchPause(true);
				EventsOnPause(); 

				_windowService.CloseActiveWindow();
				_windowService.TryOpenWindow(_menuWindow);
				_windowService.ShowWindow();
			}

			OpenTab?.Invoke(index);
		}

		public void SwitchDialogue(bool interacting)
		{
			_inInteracting = interacting;
			_gameInputs.enabled = !interacting;

			SwitchInteraction(interacting);
		}

		private void SwitchInteraction(bool enable)
		{
			if (enable)
			{
				Cursor.lockState = CursorLockMode.Confined;
				Cursor.visible = true;
			}
			else
			{
				Cursor.lockState = CursorLockMode.Locked;
				Cursor.visible = false;
			}
			
			_gameInputs.enabled = !enable;
		}

		private void SwitchPause(bool enable)
		{
			_hudController.gameObject.SetActive(!enable);

			if (_inInteracting) OnGamePause?.Invoke(enable);
			else SwitchInteraction(enable);

			Time.timeScale = enable ? 0 : 1;
		}

		IEnumerator EnableInputsDelay()
		{
			_gameInputs.enabled = false;
			_pauseInputs.enabled = false;

			yield return new WaitForSeconds(_inputsDelay);

			_gameInputs.enabled = true;
			_pauseInputs.enabled = true;
		}

		private void SetLowPreset()
		{
			QualitySettings.SetQualityLevel(0);
		}

		private void EventsOnPause()
		{
			_pauseInputs.OnEscapeKeyPressed -= OpenPause;
			_pauseInputs.OnMenuTabKeyPressed -= OpenMenu;
			_pauseInputs.OnEscapeKeyPressed += HideWindow;
			_windowService.OnUnpause += HideWindow;
		}

		private void EventsOnGame()
		{
			_pauseInputs.OnEscapeKeyPressed += OpenPause;
			_pauseInputs.OnMenuTabKeyPressed += OpenMenu;
			_pauseInputs.OnEscapeKeyPressed -= HideWindow;
			_windowService.OnUnpause -= HideWindow;
		}

		private void OnDestroy()
		{
			_pauseInputs.ShowCursor -= SwitchVisible;

			_animator.OnWindowHided -= CloseWindow;
			_sceneTransition.OnWindowHide -= SceneTransitionHide;

			_windowService.OnUnpause -= HideWindow;
			_pauseInputs.OnEscapeKeyPressed -= HideWindow;

			_pauseInputs.OnMenuTabKeyPressed -= OpenMenu;
		}
	}
}