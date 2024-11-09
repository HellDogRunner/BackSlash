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
        [SerializeField] private Button _close;

        private void Awake()
        {
            _gameplay.Select();

            _pauseInputs.OnBackKeyPressed += BackButton;

            _gameplay.onClick.AddListener(GameplayButton);
            _audio.onClick.AddListener(AudioButton);
            _video.onClick.AddListener(VideoButton);
            _management.onClick.AddListener(ManagementButton);
            _back.onClick.AddListener(BackButton);
            if (!_IsMainMenu) _close.onClick.AddListener(_windowService.Unpause);
        }

        private void GameplayButton() { OpenWindow(_gameplayHandler); }
        private void AudioButton() { OpenWindow(_audioHandler); }
        private void VideoButton() { OpenWindow(_videoHandler); }
        private void ManagementButton() { OpenWindow(_managementHandler); }
        private void BackButton() { OpenWindow(_pauseHandler); }

        private void OnDestroy()
        {
            _windowService.OnHideWindow -= DisablePause;
            _pauseInputs.OnBackKeyPressed -= BackButton;

            _gameplay.onClick.RemoveListener(GameplayButton);
            _audio.onClick.RemoveListener(AudioButton);
            _video.onClick.RemoveListener(VideoButton);
            _management.onClick.RemoveListener(ManagementButton);
            _back.onClick.RemoveListener(BackButton);
            if (!_IsMainMenu) _close.onClick.RemoveListener(_windowService.Unpause);
        }
    }
}