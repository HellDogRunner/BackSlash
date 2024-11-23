using Scripts.Player;
using System;
using System.Collections;
using UnityEngine;
using Zenject;
using static PlayerStates;

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
		private PlayerStateMachine _playerState;

		public event Action<int> OpenTab;

		[Inject]
		private void Construct(PlayerStateMachine playerState, InputController gameInputs)
		{
			_gameInputs = gameInputs;
			_playerState = playerState;
		}

		private void Awake()
		{
			_sceneTransition.gameObject.SetActive(true);
			_playerHUD.SetActive(true);
			
			_playerState.Explore();
			StartCoroutine(EnableInputsDelay());
		}

		private void OnEnable()
		{
			_playerState.OnPause += DisableGameInputs;
			_playerState.OnInteract += DisableGameInputs;
			_playerState.OnExplore += EnableGameInputs;
			
			_sceneTransition.OnWindowHide += SceneTransitionHide;
			
			_pauseInputs.OnEscapeKeyPressed += OpenPause;
			_pauseInputs.OnMenuTabKeyPressed += OpenMenu;
			_windowService.OnUnpause += SetExplore;
			_windowService.OnPause += SetPause;
		}

		private void OnDisable()
		{
			_playerState.OnPause -= DisableGameInputs;
			_playerState.OnInteract -= DisableGameInputs;
			_playerState.OnExplore -= EnableGameInputs;

			_sceneTransition.OnWindowHide -= SceneTransitionHide;

			_pauseInputs.OnEscapeKeyPressed -= OpenPause;
			_pauseInputs.OnMenuTabKeyPressed -= OpenMenu;
			_windowService.OnUnpause -= SetExplore;
			_windowService.OnPause -= SetPause;
			
		}

		private void OpenPause()
		{
			if (_playerState.State != EState.Pause)
			{
				TryOpenWindow(_pauseWindow);
			}
		}

		private void OpenMenu(int index)
		{
			if (_playerState.State != EState.Pause)
			{
				TryOpenWindow(_menuWindow);
			}

			OpenTab?.Invoke(index);
		}
		
		private void TryOpenWindow(WindowHandler window) 
		{
			_windowService.TryOpenWindow(window);
			_windowService.ShowWindow(true);
		}
		
		private void SetExplore()
		{	
			_playerState.Explore();
		}

		private void SetPause()
		{
			_playerState.Pause();
		}

		private void DisableGameInputs()
		{
			_gameInputs.enabled = false;
		}

		private void EnableGameInputs()
		{
			_gameInputs.enabled = true;
		}

		IEnumerator EnableInputsDelay()
		{
			_gameInputs.enabled = false;
			_pauseInputs.enabled = false;

			yield return new WaitForSeconds(_inputsDelay);

			_gameInputs.enabled = true;
			_pauseInputs.enabled = true;
		}
	}
}
