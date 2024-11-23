using UnityEngine;
using UnityEngine.UI;

namespace RedMoonGames.Window
{
	public class SettingsWindow : GameBasicWindow
	{
		[Header("Handlers")]
		[SerializeField] private WindowHandler _pauseHandler;
		[SerializeField] private WindowHandler _gameplayHandler;
		[SerializeField] private WindowHandler _audioHandler;
		[SerializeField] private WindowHandler _videoHandler;
		[SerializeField] private WindowHandler _managementHandler;

		[Header("Buttons")]
		[SerializeField] private Button _gameplay;
		[SerializeField] private Button _audio;
		[SerializeField] private Button _video;
		[SerializeField] private Button _management;

		[Header("Navigation Keys")]
		[SerializeField] private Button _back;

		private void OnEnable()
		{
			_gameplay.Select();

			_uiInputs.OnBackKeyPressed += BackButton;

			_gameplay.onClick.AddListener(GameplayButton);
			_audio.onClick.AddListener(AudioButton);
			_video.onClick.AddListener(VideoButton);
			_management.onClick.AddListener(ManagementButton);
			_back.onClick.AddListener(BackButton);
		}

		private void GameplayButton() { ReplaceWindow(this, _gameplayHandler); }
		private void AudioButton() { ReplaceWindow(this, _audioHandler); }
		private void VideoButton() { ReplaceWindow(this, _videoHandler); }
		private void ManagementButton() { ReplaceWindow(this, _managementHandler); }
		private void BackButton() { ReplaceWindow(this, _pauseHandler); }

		private void OnDisable()
		{
			_uiInputs.OnBackKeyPressed -= BackButton;

			_gameplay.onClick.RemoveListener(GameplayButton);
			_audio.onClick.RemoveListener(AudioButton);
			_video.onClick.RemoveListener(VideoButton);
			_management.onClick.RemoveListener(ManagementButton);
			_back.onClick.RemoveListener(BackButton);
		}
	}
}
