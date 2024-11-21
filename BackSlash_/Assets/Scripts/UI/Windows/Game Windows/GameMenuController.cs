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
			_playerState.OnExplore += EnableGameInputs;
			
			_animator.OnWindowHided += CloseWindow;
			_sceneTransition.OnWindowHide += SceneTransitionHide;

			_pauseInputs.OnMenuTabKeyPressed += OpenMenu;
			_pauseInputs.OnEscapeKeyPressed += OpenPause;
		}

		private void OnDisable()
		{
			_playerState.OnPause -= DisableGameInputs;
			_playerState.OnExplore -= EnableGameInputs;

			_animator.OnWindowHided -= CloseWindow;
			_sceneTransition.OnWindowHide -= SceneTransitionHide;

			_windowService.OnUnpause -= HideWindow;
			_pauseInputs.OnEscapeKeyPressed -= HideWindow;
			_pauseInputs.OnMenuTabKeyPressed -= OpenMenu;
		}

		private void OpenPause()
		{
			_playerState.Pause();
			EventsOnPause();
			
			TryOpenWindow(_pauseWindow);
		}

		private void OpenMenu(int index)
		{
			if (_playerState.CurrentState != EState.Pause)
			{
				_playerState.Pause();
				EventsOnPause();

				TryOpenWindow(_menuWindow);
			}

			OpenTab?.Invoke(index);
		}
		
		private void HideWindow()
		{	
			if (_animator.GetCanOpenWindow())
			{
				_playerState.Explore();
				
				var window = _windowService.GetActiveWindow();
				_windowService.HideWindow(window);

				EventsOnGame();
			}
		}

		private void TryOpenWindow(WindowHandler window) 
		{
			if (_animator.GetCanOpenWindow())
			{
				_windowService.CloseActiveWindow();
				_windowService.TryOpenWindow(window);
				_windowService.ShowWindow();
			}
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
	}
}
