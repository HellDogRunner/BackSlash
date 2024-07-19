using Scripts.Player;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace RedMoonGames.Window
{
    public class SettingsWindow : GameBasicWindow
    {
        [Header("Handlers")]
        [SerializeField] private WindowHandler _settingsHandler;
        [SerializeField] private WindowHandler _pauseHandler;

        [Header("Tabs")]
        [SerializeField] private GameObject _gameplayTab;
        [SerializeField] private GameObject _managementTab;
        [SerializeField] private GameObject _soundTab;
        [SerializeField] private GameObject _videoTab;

        [Header("Buttons")]
        [SerializeField] private Button _prevTabButton;
        [SerializeField] private Button _nextTabButton;
        [SerializeField] private Button _gameplayButton;
        [SerializeField] private Button _managementButton;
        [SerializeField] private Button _soundButton;
        [SerializeField] private Button _videoButton;
        [SerializeField] private Button _backButton;
        [Space]
        [SerializeField]  private List<GameObject> _tabs = new List<GameObject>();

        private UIController _controller;

        private GameObject _currentTab;
        private int _currentTabIndex;

        [Inject]
        private void Build(UIController controller)
        {
            _controller = controller;

            _controller.OnTabPressed += SelectingTab;

            _tabs.Add(_gameplayTab);
            _tabs.Add(_managementTab);
            _tabs.Add(_soundTab);
            _tabs.Add(_videoTab);

            foreach (var tab in _tabs)
            {
                tab.SetActive(false);
            }

            _currentTabIndex = 0;
            _currentTab = _tabs[_currentTabIndex];
            _currentTab.SetActive(true);
        }

        private void OnEnable()
        {
            _prevTabButton.onClick.AddListener(() => SelectingTab("q"));
            _nextTabButton.onClick.AddListener(() => SelectingTab("e"));
            _gameplayButton.onClick.AddListener(() => SwitchActiveTab(_gameplayTab, 0));
            _managementButton.onClick.AddListener(() => SwitchActiveTab(_managementTab, 1));
            _soundButton.onClick.AddListener(() => SwitchActiveTab(_soundTab, 2));
            _videoButton.onClick.AddListener(() => SwitchActiveTab(_videoTab, 3));
            _backButton.onClick.AddListener(CloseWindow);
        }

        private void OnDisable()
        {
            _prevTabButton.onClick.RemoveListener(() => SelectingTab("q"));
            _nextTabButton.onClick.RemoveListener(() => SelectingTab("e"));
            _gameplayButton.onClick.RemoveListener(() => SwitchActiveTab(_gameplayTab, 0));
            _managementButton.onClick.RemoveListener(() => SwitchActiveTab(_managementTab, 1));
            _soundButton.onClick.RemoveListener(() => SwitchActiveTab(_soundTab, 2));
            _videoButton.onClick.RemoveListener(() => SwitchActiveTab(_videoTab, 3));
            _backButton.onClick.RemoveListener(CloseWindow);
        }

        private void SelectingTab(string key)
        {
            int tabIndex;
            if (key == "q")
            {
                tabIndex = (_currentTabIndex - 1) % _tabs.Count;
                if (tabIndex < 0) tabIndex = _tabs.Count - 1;
            }
            else
            {
                tabIndex = (_currentTabIndex + 1) % _tabs.Count;
            }
            SwitchActiveTab(_tabs[tabIndex], tabIndex);
        }

        private void SwitchActiveTab(GameObject tab, int tabIndex)
        {
            _currentTabIndex = tabIndex;
            _currentTab.SetActive(false);
            _currentTab = tab;
            _currentTab.SetActive(true);
        }
        private void CloseWindow()
        {
            _windowManager.CloseWindow(_settingsHandler);
            _windowManager.OpenWindow(_pauseHandler);
        }

        protected override void DisablePause()
        {
            _animationController.HideWindowAnimation(_canvasGroup, _settingsHandler);
        }

        protected override void OnDestroy()
        {
            _controller.OnTabPressed -= SelectingTab;
            _windowManager.OnUnpausing -= DisablePause;
            _windowManager.OnPausing -= EnablePause;
        }
    }
}