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
        [SerializeField] private TabAnimationService _inventotyAnimation;
        [SerializeField] private TabAnimationService _combosAnimation;
        [SerializeField] private TabAnimationService _abilitiesAnimation;
        [SerializeField] private TabAnimationService _skillsAnimation;
        [SerializeField] private TabAnimationService _journalAnimation;
        [SerializeField] private TabAnimationService _mapAnimation;

        private List<GameObject> _tabs = new List<GameObject>();
        private List<TabAnimationService> _animations = new List<TabAnimationService>();
        private int _index;
        private int _animationIndex;

        private UIMenuInputs _inputController;
        private GameWindowsController _windowsController;
        private PlayerMenuAnimationService _animationService;
        private AudioController _audioController;

        [Inject]
        private void Binding(UIMenuInputs inputController, GameWindowsController windowsController, PlayerMenuAnimationService animationService, AudioController audioController)
        {
            _windowsController = windowsController;
            _animationService = animationService;
            _audioController = audioController;

            _inputController = inputController;
            _inputController.OnInventoryPressed += ShowPlayerMenu;
            _inputController.OnCombosPressed += ShowPlayerMenu;
            _inputController.OnAbilitiesPressed += ShowPlayerMenu;
            _inputController.OnSkillsPressed += ShowPlayerMenu;
            _inputController.OnJournalPressed += ShowPlayerMenu;
            _inputController.OnMapPressed += ShowPlayerMenu;
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

        private void ShowPlayerMenu(int index)
        {
            if (_canvasGroup.alpha == 0)
            {
                SubscribeToActions();
                _animationService.AnimateMenuShow(_canvasGroup);
                _windowsController.SwitchPause(true);
            }

            OpenTab(index);
        }

        private void HidePlayerMenu()
        {
            if (_canvasGroup.alpha == 1)
            {
                UnsubscribeToActions();
                CloseAllTabs();

                _animationService.AnimateMenuHide(_canvasGroup);

                _windowsController.SwitchPause(false);
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

            ShowTab();

            _audioController.PlayGenericEvent(FMODEvents.instance.UIButtonClickEvent);
            _tabs[_index].SetActive(true);
        }

        private void ShowTab()
        {
            if (_animationIndex > -1)
            {
                _animations[_animationIndex].SwitchTab();
            }

            _animationIndex = _index;
            _animations[_index].SwitchTab();
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

            _inputController.OnEscapePressed += HidePlayerMenu;
            _inputController.OnBackPressed += HidePlayerMenu;
            _inputController.OnPrevPressed += SwitchTab;
            _inputController.OnNextPressed += SwitchTab;
            _inputController.OnAnyActionPressed += ShowCursor;
            _inputController.OnMousePointChange += ShowCursor;

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

            _inputController.OnEscapePressed -= HidePlayerMenu;
            _inputController.OnBackPressed -= HidePlayerMenu;
            _inputController.OnPrevPressed -= SwitchTab;
            _inputController.OnNextPressed -= SwitchTab;
            _inputController.OnAnyActionPressed -= ShowCursor;
            _inputController.OnMousePointChange -= ShowCursor;

            _prevKey.onClick.RemoveAllListeners();
            _nextKey.onClick.RemoveAllListeners();

            _backButton.onClick.RemoveAllListeners();
            _closeButton.onClick.RemoveAllListeners();
        }

        private void OnDestroy()
        {
            _inputController.OnInventoryPressed -= ShowPlayerMenu;
            _inputController.OnCombosPressed -= ShowPlayerMenu;
            _inputController.OnAbilitiesPressed -= ShowPlayerMenu;
            _inputController.OnSkillsPressed -= ShowPlayerMenu;
            _inputController.OnJournalPressed -= ShowPlayerMenu;
            _inputController.OnMapPressed -= ShowPlayerMenu;

            UnsubscribeToActions();
        }
    }
}