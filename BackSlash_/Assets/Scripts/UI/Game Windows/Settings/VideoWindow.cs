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
        [SerializeField] private Button _back;
        [SerializeField] private Button _close;

        private GameObject _currentTab;
        private TabAnimationService _displayAnimation;
        private TabAnimationService _graphicsAnimation;

        private void Awake()
        {
            _displayAnimation = _displayButton.GetComponent<TabAnimationService>();
            _graphicsAnimation = _graphicsButton.GetComponent<TabAnimationService>();

            _currentTab = _displayTab;
            _currentTab.SetActive(true);

            _controller.OnBackKeyPressed += Back;
            _controller.OnTabPressed += OnTabPressed;

            _displayButton.onClick.AddListener(() => SwitchTab(_displayTab));
            _graphicsButton.onClick.AddListener(() => SwitchTab(_graphicsTab));
            _back.onClick.AddListener(() => SwitchWindows(_videoHandler, _settingsHandler));
            _close.onClick.AddListener(_windowManager.SwitchPause);
        }

        private void Back()
        {
            SwitchWindows(_videoHandler, _settingsHandler);
        }

        private void OnTabPressed()
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

                if (tab == _graphicsTab)
                {
                    _displayAnimation.DesableTab();
                    _graphicsAnimation.EnableTab();
                }
                else
                {
                    _graphicsAnimation.DesableTab();
                    _displayAnimation.EnableTab();
                }

                _currentTab.SetActive(false);
                _currentTab = tab; 
                _currentTab.SetActive(true);
            }
        }

        private void OnDestroy()
        {
            _windowManager.OnUnpausing -= DisablePause;

            _controller.OnBackKeyPressed -= Back;
            _controller.OnTabPressed -= OnTabPressed;

            _displayButton.onClick.RemoveAllListeners();
            _graphicsButton.onClick.RemoveAllListeners();
            _back.onClick.RemoveAllListeners();
            _close.onClick.RemoveAllListeners();
        }
    }
}