using Scripts.Player;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace RedMoonGames.Window
{
    public class PlayerMenuWindow : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;

        [Header("Tabs")]
        [SerializeField] private GameObject _inventoryTab;
        [SerializeField] private GameObject _combosTab;
        [SerializeField] private GameObject _abilitiesTab;
        [SerializeField] private GameObject _skillsTab;
        [SerializeField] private GameObject _journalTab;
        [SerializeField] private GameObject _mapTab;

        [Header("Buttons")]
        [SerializeField] private Button _inventoryButton;
        [SerializeField] private Button _combosButton;
        [SerializeField] private Button _abilitiesButton;
        [SerializeField] private Button _skillsButton;
        [SerializeField] private Button _journalButton;
        [SerializeField] private Button _mapButton;

        [Header("Navigation Keys")]
        [SerializeField] private Button _prevKey;
        [SerializeField] private Button _nextKey;
        [SerializeField] private Button _selectButton;
        [SerializeField] private Button _backButton;
        [SerializeField] private Button _closeButton;

        [Header("Tabs Animation")]
        [SerializeField] private MenuTabAnimationService _inventotyAnimation;
        [SerializeField] private MenuTabAnimationService _combosAnimation;
        [SerializeField] private MenuTabAnimationService _abilitiesAnimation;
        [SerializeField] private MenuTabAnimationService _skillsAnimation;
        [SerializeField] private MenuTabAnimationService _journalAnimation;
        [SerializeField] private MenuTabAnimationService _mapAnimation;

        private List<GameObject> _tabs = new List<GameObject>();
        private List<MenuTabAnimationService> _animations = new List<MenuTabAnimationService>();
        private int _index;
        private int _animationIndex;

        private bool _menuActive;

        private UIMenuInputs _menuController;
        private PlayerMenuAnimationService _animationService;
        private AudioController _audioController;

        [Inject]
        private void Construct(UIMenuInputs menuController, PlayerMenuAnimationService animationService, AudioController audioController)
        {
            _animationService = animationService;
            _audioController = audioController;
            _menuController = menuController;
        }

        private void Awake()
        {
            _animationIndex = -1;

            _canvasGroup.alpha = 0;

            _tabs.Add(_inventoryTab);
            _tabs.Add(_combosTab);
            _tabs.Add(_abilitiesTab);
            _tabs.Add(_skillsTab);
            _tabs.Add(_journalTab);
            _tabs.Add(_mapTab);

            _animations.Add(_inventotyAnimation);
            _animations.Add(_combosAnimation);
            _animations.Add(_abilitiesAnimation);
            _animations.Add(_skillsAnimation);
            _animations.Add(_journalAnimation);
            _animations.Add(_mapAnimation);
        }

        public void ShowPlayerMenu(int index)
        {
            if (!_menuActive)
            {
                _menuActive = true;
                SubscribeToActions();
                _animationService.AnimateMenuShow(_canvasGroup);
            }

            OpenTab(index);
        }

        public void HidePlayerMenu()
        {
            if (_menuActive)
            {
                _menuActive = false;
                UnsubscribeToActions();
                CloseAllTabs();

                _animationService.AnimateMenuHide(_canvasGroup);
            }
        }

        private void SwitchTab(int index)
        {
            OpenTab(_index + index);
        }

        private void OpenTab(int index)
        {
            _index = index % _tabs.Count;
            if (_index < 0)
            {
                _index = _tabs.Count - 1;
            }
            if (_tabs[_index].activeSelf)
            {
                return;
            }

            CloseAllTabs();

            ShowActiveTab();

            _audioController.PlayGenericEvent(FMODEvents.instance.UIButtonClickEvent);
            _tabs[_index].SetActive(true);
        }

        private void ShowActiveTab()
        {
            if (_animationIndex > -1)
            {
                _animations[_animationIndex].DisableTab();
            }

            _animationIndex = _index;
            _animations[_index].EnableTab();
        }

        private void CloseAllTabs()
        {
            foreach (var _tab in _tabs)
            {
                _tab.SetActive(false);
            }
        }

        private void ShowCursor(bool _)
        {
            Cursor.visible = _;
        }

        private void SubscribeToActions()
        {
            _inventoryButton.onClick.AddListener(() => OpenTab(0));
            _combosButton.onClick.AddListener(() => OpenTab(1));
            _abilitiesButton.onClick.AddListener(() => OpenTab(2));
            _skillsButton.onClick.AddListener(() => OpenTab(3));
            _journalButton.onClick.AddListener(() => OpenTab(4));
            _mapButton.onClick.AddListener(() => OpenTab(5));

            _menuController.OnPrevPressed += SwitchTab;
            _menuController.OnNextPressed += SwitchTab;

            _prevKey.onClick.AddListener(() => SwitchTab(-1));
            _nextKey.onClick.AddListener(() => SwitchTab(+1));

            _backButton.onClick.AddListener(HidePlayerMenu);
            _closeButton.onClick.AddListener(HidePlayerMenu);
        }

        private void UnsubscribeToActions()
        {
            _inventoryButton.onClick.RemoveAllListeners();
            _combosButton.onClick.RemoveAllListeners();
            _abilitiesButton.onClick.RemoveAllListeners();
            _skillsButton.onClick.RemoveAllListeners();
            _journalButton.onClick.RemoveAllListeners();
            _mapButton.onClick.RemoveAllListeners();

            _menuController.OnPrevPressed -= SwitchTab;
            _menuController.OnNextPressed -= SwitchTab;

            _prevKey.onClick.RemoveAllListeners();
            _nextKey.onClick.RemoveAllListeners();

            _backButton.onClick.RemoveAllListeners();
            _closeButton.onClick.RemoveAllListeners();
        }

        private void OnDestroy()
        {
            UnsubscribeToActions();
        }
    }
}