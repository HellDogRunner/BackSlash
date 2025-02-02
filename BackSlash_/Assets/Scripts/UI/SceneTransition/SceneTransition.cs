using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RedMoonGames.Window
{
	public class SceneTransition : MonoBehaviour
	{
		[SerializeField] private LoadingSceneAnimation _animations;
		[Space]
		[SerializeField] private static bool _playOpening = false;

		private string _sceneName;

		private AsyncOperation _loadingScene;

		public event Action OnWindowHide; 
		
		private void OnEnable()
		{
			_animations.OnLoadingEnd += ChangeScene;
			_animations.OnOpeningEnd += CloseWindow;
			_animations.OnClosingEnd += LoadScene;
		}
		
		private void OnDisable()
		{
			_animations.OnLoadingEnd -= ChangeScene;
			_animations.OnOpeningEnd -= CloseWindow;
			_animations.OnClosingEnd -= LoadScene;
		}

		private void Awake()
		{
			if (_playOpening)
			{
				_animations.AnimateOpening();
			}
		}
		
		private void Start()
		{
			if (!_playOpening)
			{
				OnWindowHide?.Invoke();
			}
		}

		private void CloseWindow()
		{
			OnWindowHide?.Invoke();
		}

		public void SwichToScene(string sceneName)
		{
			PrepareToChangeScene();
			_sceneName = sceneName;
			_animations.AnimateClosing();
		}

		private void LoadScene()
		{
			_loadingScene = SceneManager.LoadSceneAsync(_sceneName);
			_loadingScene.allowSceneActivation = false;
		}

		private void ChangeScene()
		{
			_playOpening = true;
			_loadingScene.allowSceneActivation = true;
		}

		private void PrepareToChangeScene()
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
			Time.timeScale = 0;
		}
	}
}
