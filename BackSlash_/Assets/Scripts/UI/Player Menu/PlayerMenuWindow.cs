using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RedMoonGames.Window
{
    public class PlayerMenuWindow : GameBasicWindow
    {
        [Header("Tabs")]
        [SerializeField] private GameObject _inventoryTab;
        [SerializeField] private GameObject _combosTab;
        [SerializeField] private GameObject _abilitiesTab;
        [SerializeField] private GameObject _skillsTab;
        [SerializeField] private GameObject _tasksTab;
        [SerializeField] private GameObject _mapTab;

        [Header("Navigation Keys")]
        [SerializeField] private Button _backKey;
        [SerializeField] private Button _nextKey;
        [SerializeField] private Button _selectButton;
        [SerializeField] private Button _closeButton;

        private List<GameObject> _tabs = new List<GameObject>();
        private int _currentTabIndex;

        private void Awake()
        {
            _tabs.Add(_inventoryTab);
            _tabs.Add(_combosTab);
            _tabs.Add(_abilitiesTab);
            _tabs.Add(_skillsTab);
            _tabs.Add(_tasksTab);
            _tabs.Add(_mapTab);

            _animationService.ShowWindowAnimation(_canvasGroup);

            _uIController.OnInventoryKeyPressed += InventoryPressed;
            _uIController.OnBackKeyPressed += PreviousTab;
            _uIController.OnEnterKeyPressed += NextTab;

            _backKey.onClick.AddListener(() => SwitchTab(-1));
            _nextKey.onClick.AddListener(() => SwitchTab(+1));

            _closeButton.onClick.AddListener(_windowsController.SwitchPause);
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

            foreach (var _tab in _tabs)
            {
                _tab.SetActive(false);
            }

            _currentTabIndex = index;
            _tabs[_currentTabIndex].SetActive(true);
        }

        private void OnDestroy()
        {
            _windowsController.OnUnpausing -= DisablePause;

            _backKey.onClick.RemoveAllListeners();
            _nextKey.onClick.RemoveAllListeners();

            _uIController.OnInventoryKeyPressed -= InventoryPressed;
        }
    }
}