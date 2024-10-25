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

		private bool _inPlayerMenu;
		private bool _inDialogue;

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

			_windowAnimation.OnAnimationComplete += CloseWindow;
			_sceneTransition.OnWindowHide += SceneTransitionHide;

			_pauseInputs.OnMenuTabPressed += OpenMenu;
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

			_windowService.TryOpenWindow(_pauseWindow);
			_windowService.ShowWindow();
		}

		private void HideWindow()
		{
			_inPlayerMenu = false;

			EventsOnGame();
			SwitchPause(false);

			_windowService.HideWindow();
		}

		private void OpenMenu(int index)
		{
			if (!_inPlayerMenu)
			{
				_inPlayerMenu = true;

				SwitchPause(true);
				EventsOnPause(); 

				_windowService.TryOpenWindow(_menuWindow);
				_windowService.ShowWindow();
			}

			OpenTab?.Invoke(index);
		}

		public void SwitchDialogue(bool inDialogue)
		{
			_inDialogue = inDialogue;
			_gameInputs.enabled = !inDialogue;

			SwitchInteraction(inDialogue);
		}

		private void SwitchInteraction(bool enable)
		{
			if (!_inDialogue) _gameInputs.enabled = !enable;

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
		}

		private void SwitchPause(bool enable)
		{
			_hudController.gameObject.SetActive(!enable);

			SwitchInteraction(enable);

			if (_inDialogue) OnGamePause?.Invoke(enable);

			if (enable) Time.timeScale = 0;
			else Time.timeScale = 1;
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