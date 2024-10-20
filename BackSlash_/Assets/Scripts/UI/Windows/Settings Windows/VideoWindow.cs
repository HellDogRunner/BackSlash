using UnityEngine;
using UnityEngine.UI;

namespace RedMoonGames.Window
{
    public class VideoWindow : GameBasicWindow
    {
        [Header("Handlers")]
        [SerializeField] private WindowHandler _videoHandler;
        [SerializeField] private WindowHandler _settingsHandler;

        [Header("Tab Keys")]
        [SerializeField] Button _displayButton;
        [SerializeField] Button _graphicsButton;

        [Header("Tabs")]
        [SerializeField] private GameObject _displayTab;
        [SerializeField] private GameObject _graphicsTab;

        [Header("Navigation Keys")]
        [SerializeField] private Button _switch;
        [SerializeField] private Button _back;
        [SerializeField] private Button _close;

        [Header("Tabs Animation")]
        [SerializeField] private TabAnimationService _displayAnimation;
        [SerializeField] private TabAnimationService _graphicsAnimation;

        private GameObject _currentTab;

        private void Awake()
        {
            _currentTab = _displayTab;
            _currentTab.SetActive(true);

            _pauseInputs.OnBackKeyPressed += BackButton;
            _pauseInputs.OnSwitchTabPressed += OnTabAction;

            _displayButton.onClick.AddListener(DisplayButton);
            _graphicsButton.onClick.AddListener(GraphicsButton);
            _switch.onClick.AddListener(OnTabAction);
            _back.onClick.AddListener(BackButton);
            if (!_IsMainMenu) _close.onClick.AddListener(_windowService.Unpause);
        }

        private void DisplayButton() { SwitchTab(_displayTab); }
        private void GraphicsButton() { SwitchTab(_graphicsTab); }
        private void BackButton() { SwitchWindows(_videoHandler, _settingsHandler); }

        private void OnTabAction()
        {
            if (_currentTab == _displayTab)
            {
                SwitchTab(_graphicsTab);
            }
            else
            {
                SwitchTab(_displayTab);
            }
        }

        private void SwitchTab(GameObject tab)
        {
            if (_currentTab != tab)
            {
                PlayClickSound();

                _displayAnimation.SwitchTab();
                _graphicsAnimation.SwitchTab();

                _currentTab.SetActive(false);
                _currentTab = tab; 
                _currentTab.SetActive(true);
            }
        }

        private void OnDestroy()
        {
            _windowService.OnHideWindow -= DisablePause;

            _pauseInputs.OnBackKeyPressed -= BackButton;
            _pauseInputs.OnSwitchTabPressed -= OnTabAction;

            _displayButton.onClick.RemoveListener(DisplayButton);
            _graphicsButton.onClick.RemoveListener(GraphicsButton);
            _switch.onClick.RemoveListener(OnTabAction);
            _back.onClick.RemoveListener(BackButton);
            if (!_IsMainMenu) _close.onClick.RemoveListener(_windowService.Unpause);
        }
    }
}