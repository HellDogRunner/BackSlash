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
        [SerializeField] private Button _switch;

        [Header("Tabs Animation")]
        [SerializeField] private TabAnimationService _displayAnimation;
        [SerializeField] private TabAnimationService _graphicsAnimation;

        private GameObject _currentTab;

        private void Awake()
        {
            _currentTab = _displayTab;
            _currentTab.SetActive(true);

            _uIController.OnBackKeyPressed += Back;
            _uIController.OnTabPressed += OnTabPressed;

            _displayButton.onClick.AddListener(() => SwitchTab(_displayTab));
            _graphicsButton.onClick.AddListener(() => SwitchTab(_graphicsTab));
            _switch.onClick.AddListener(OnTabPressed);
            _back.onClick.AddListener(() => SwitchWindows(_videoHandler, _settingsHandler));
            _close.onClick.AddListener(_windowsController.PausePressed);
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

                _displayAnimation.SwitchTab();
                _graphicsAnimation.SwitchTab();

                _currentTab.SetActive(false);
                _currentTab = tab; 
                _currentTab.SetActive(true);
            }
        }

        private void OnDestroy()
        {
            _windowsController.OnUnpausing -= DisablePause;

            _uIController.OnBackKeyPressed -= Back;
            _uIController.OnTabPressed -= OnTabPressed;

            _displayButton.onClick.RemoveAllListeners();
            _graphicsButton.onClick.RemoveAllListeners();
            _switch.onClick.RemoveAllListeners();
            _back.onClick.RemoveAllListeners();
            _close.onClick.RemoveAllListeners();
        }
    }
}