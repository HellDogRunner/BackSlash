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
        [SerializeField] private Button _backKey;
        [SerializeField] private Button _nextKey;
        [SerializeField] private Button _selectButton;
        [SerializeField] private Button _closeButton;

        private List<GameObject> _tabs = new List<GameObject>();
        private int _currentTabIndex;

        private MenuActions _inputController;
        private GameWindowsController _windowsController;
        private PlayerMenuAnimationService _animationService;
        private AudioController _audioController;

        [Inject]
        private void Binding(MenuActions inputController, GameWindowsController windowsController, PlayerMenuAnimationService animationService, AudioController audioController)
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
            _canvasGroup.alpha = 0;

            _tabs.Add(_inventoryTab);
            _tabs.Add(_combosTab);
            _tabs.Add(_abilitiesTab);
            _tabs.Add(_skillsTab);
            _tabs.Add(_journalTab);
            _tabs.Add(_mapTab);
        }

        private void ShowPlayerMenu(int index)
        {
            if (_canvasGroup.alpha == 0)
            {
                SubscribeToActions();
                _animationService.AnimateMenuShow(_canvasGroup);
            }
            
            OpenTab(index);

            _windowsController.SwitchPause(true);
        }

        private void HidePlayerMenu()
        {
            if (_canvasGroup.alpha == 1)
            {
                UnsubscribeToActions();
                HideAllTabs();

                _animationService.AnimateMenuHide(_canvasGroup);

                _windowsController.SwitchPause(false);
            }
        }

        private void NextTab()
        {
            OpenTab(_currentTabIndex + 1);
        }

        private void PreviousTab()
        {
            OpenTab(_currentTabIndex - 1);
        }

        private void InventoryPressed()
        {
            OpenTab(0);
        }

        private void SwitchTab(int index)
        {
            OpenTab(_currentTabIndex + index);
        }

        private void OpenTab(int index)
        {
            index = index % _tabs.Count;
            if (index < 0) index = _tabs.Count - 1;

            if (_tabs[index].activeSelf)
            {
                return;
            }

            HideAllTabs();

            _currentTabIndex = index;
            _audioController.PlayGenericEvent(FMODEvents.instance.UIButtonClickEvent);
            _tabs[_currentTabIndex].SetActive(true);
        }

        private void HideAllTabs()
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
            _inputController.OnBackPressed += PreviousTab;
            _inputController.OnNextPressed += NextTab;
            _inputController.OnAnyActionPressed += ShowCursor;
            _inputController.OnMousePointChange += ShowCursor;

            _backKey.onClick.AddListener(() => SwitchTab(-1));
            _nextKey.onClick.AddListener(() => SwitchTab(+1));

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
            _inputController.OnBackPressed -= PreviousTab;
            _inputController.OnNextPressed -= NextTab;
            _inputController.OnAnyActionPressed -= ShowCursor;
            _inputController.OnMousePointChange -= ShowCursor;

            _backKey.onClick.RemoveAllListeners();
            _nextKey.onClick.RemoveAllListeners();

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