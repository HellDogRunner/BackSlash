using Scripts.Player;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace RedMoonGames.Window
{
    public class GameSettingsWindow : GameBasicWindow
    {
        [Header("Handlers")]
        [SerializeField] private WindowHandler _settingsHandler;
        [SerializeField] private WindowHandler _pauseHandler;
        [Space]
        [SerializeField] private WindowHandler _gameplayHandler;
        [SerializeField] private WindowHandler _managementHandler;
        [SerializeField] private WindowHandler _soundHandler;
        [SerializeField] private WindowHandler _videoTabHandler;
        [Space]
        [SerializeField] private List<WindowHandler> _tabs = new List<WindowHandler>();

        [Header("Buttons")]
        [SerializeField] private Button _gameplayButton;
        [SerializeField] private Button _managementButton;
        [SerializeField] private Button _soundButton;
        [SerializeField] private Button _videoButton;
        [SerializeField] private Button _backButton;

        private UIController _controller;

        private WindowHandler _currentTab;
        private int _currentTabIndex;

        [Inject]
        private void Build(UIController controller)
        {
            _controller = controller;

            _tabs.Add(_gameplayHandler);
            _tabs.Add(_managementHandler);
            _tabs.Add(_soundHandler);
            _tabs.Add(_videoTabHandler);
        }
         
        private void Start()
        {
            _currentTabIndex = 0;
            _currentTab = _tabs[_currentTabIndex];
            _windowManager.OpenWindow(_currentTab);

            _controller.OnTabPressed += SwitchTab;
        }

        private void OnEnable()
        {
            _backButton.onClick.AddListener(BackClick);
            _gameplayButton.onClick.AddListener(GameplayTab);
            _managementButton.onClick.AddListener(ManagementTab);
            _soundButton.onClick.AddListener(SoundTab);
            _videoButton.onClick.AddListener(VideoTab);
        }

        private void OnDisable()
        {
            _controller.OnTabPressed -= SwitchTab;

            _backButton.onClick.RemoveListener(BackClick);
            _gameplayButton.onClick.RemoveListener(GameplayTab);
            _managementButton.onClick.RemoveListener(ManagementTab);
            _soundButton.onClick.RemoveListener(SoundTab);
            _videoButton.onClick.RemoveListener(VideoTab);            
        }

        private void GameplayTab()
        {
            _windowManager.CloseWindow(_currentTab);
            _currentTab = _gameplayHandler;
            _currentTabIndex = 0;
            _windowManager.OpenWindow(_gameplayHandler);
        }

        private void ManagementTab()
        {
            _windowManager.CloseWindow(_currentTab);
            _currentTab = _managementHandler;
            _currentTabIndex = 1;
            _windowManager.OpenWindow(_managementHandler);
        }

        private void SoundTab()
        {
            _windowManager.CloseWindow(_currentTab);
            _currentTab = _soundHandler;
            _currentTabIndex = 2;
            _windowManager.OpenWindow(_soundHandler);
        }

        private void VideoTab()
        {
            _windowManager.CloseWindow(_currentTab);
            _currentTab = _videoTabHandler;
            _currentTabIndex = 3;
            _windowManager.OpenWindow(_videoTabHandler);
        }

        private void BackClick()
        {
            _windowManager.CloseWindow(_settingsHandler);
            _windowManager.CloseWindow(_currentTab);
            _windowManager.OpenWindow(_pauseHandler);
        }

        private void SwitchTab(string key)
        {
            if (key == "q")
            {
                _currentTabIndex = (_currentTabIndex - 1) % _tabs.Count;
                if (_currentTabIndex < 0) _currentTabIndex = _tabs.Count - 1;
            }
            else
            {
                _currentTabIndex = (_currentTabIndex + 1) % _tabs.Count;
            }

            _windowManager.CloseWindow(_currentTab);
            _currentTab = _tabs[_currentTabIndex];
            _windowManager.OpenWindow(_currentTab);
        }

        protected override void HideCurrentWindow()
        {
            _animationController.HideWindowAnimation(_canvasGroup, _settingsHandler);
        }
    }
}