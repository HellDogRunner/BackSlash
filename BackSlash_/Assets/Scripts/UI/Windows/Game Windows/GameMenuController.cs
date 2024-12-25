using Scripts.Player;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using Zenject;

namespace RedMoonGames.Window
{
	public class GameMenuController : BasicMenuController
	{
		[SerializeField] private GameObject _playerHUD;
		[Space]
		[SerializeField] private WindowHandler _pauseWindow;
		[SerializeField] private WindowHandler _menuWindow;
		[Space]
		[SerializeField] private float _inputsDelay = 1;

		private InputController _gameInputs;
		private PlayerStateController _stateController;
		private CursorController _cursor;
		private TimeController _time;

		[Inject]
		private void Construct(CursorController cursor, TimeController time, PlayerStateController stateController, InputController gameInputs)
		{
			_stateController = stateController;
			_gameInputs = gameInputs;
			_cursor = cursor;
			_time = time;
		}

		private void Awake()
		{
			_sceneTransition.gameObject.SetActive(true);
			_playerHUD.SetActive(true);
			StartCoroutine(EnableInputsDelay());
			SetPause(false);
			_cursor.Pause(false);
		}

		private void OnEnable()
		{
			_time.OnPause += Pause;
			_stateController.OnInteract += Interact;
			
			_sceneTransition.OnWindowHide += SceneTransitionHide;
			
			_uiInputs.OnEscapeKeyPressed += OpenPause;
			_uiInputs.OnMenuKeyPressed += OpenMenu;
			_windowService.OnPause += SetPause;
		}

		private void OnDisable()
		{
			_time.OnPause -= Pause;
			_stateController.OnInteract -= Interact;

			_sceneTransition.OnWindowHide -= SceneTransitionHide;

			_uiInputs.OnEscapeKeyPressed -= OpenPause;
			_uiInputs.OnMenuKeyPressed -= OpenMenu;
			_windowService.OnPause -= SetPause;		
		}

		private void OpenPause()
		{
			if (!_time.Paused) TryOpenWindow(_pauseWindow);
		}

		private void OpenMenu()
		{
			// FIXME can the player open the menu during the dialogue?
			//if (_stateController.State != EState.Pause && _stateController.State != EState.Interact)
			if (!_time.Paused) TryOpenWindow(_menuWindow);
		}
		
		private void TryOpenWindow(WindowHandler window) 
		{
			_windowService.TryOpenWindow(window);
			_windowService.ShowWindow(true);
		}

		private void SetPause(bool pause)
		{
			_time.Pause(pause);
		}

		private void Pause(bool value)
		{
			if (value) _gameInputs.enabled = !value;
			else if (_stateController.State != EPlayerState.Interact) _gameInputs.enabled = !value;
		}

		private void Interact(bool value)
		{
			_gameInputs.enabled = !value;
		}
		
		IEnumerator EnableInputsDelay()
		{
			_gameInputs.enabled = false;
			_uiInputs.enabled = false;

			yield return new WaitForSeconds(_inputsDelay);

			_gameInputs.enabled = true;
			_uiInputs.enabled = true;
		}
	}
}
