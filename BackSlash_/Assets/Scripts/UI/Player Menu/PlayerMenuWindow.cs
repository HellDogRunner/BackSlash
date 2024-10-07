using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace RedMoonGames.Window
{
    [RequireComponent(typeof(CanvasGroup))]
    public class PlayerMenuWindow : GameBasicWindow
    {
        [Header("Tabs")]
        [SerializeField] private GameObject _weaponTab;
        [SerializeField] private GameObject _combosTab;
        [SerializeField] private GameObject _abilitiesTab;
        [SerializeField] private GameObject _skillsTab;
        [SerializeField] private GameObject _journalTab;
        [SerializeField] private GameObject _mapTab;

        [Header("Buttons")]
        [SerializeField] private Button _weaponButton;
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
        [SerializeField] private MenuTabAnimationService _weaponAnimation;
        [SerializeField] private MenuTabAnimationService _combosAnimation;
        [SerializeField] private MenuTabAnimationService _abilitiesAnimation;
        [SerializeField] private MenuTabAnimationService _skillsAnimation;
        [SerializeField] private MenuTabAnimationService _journalAnimation;
        [SerializeField] private MenuTabAnimationService _mapAnimation;

        private List<GameObject> _tabs = new List<GameObject>();
        private List<MenuTabAnimationService> _animations = new List<MenuTabAnimationService>();

        private GameMenuController _menuController;

        [Inject]
        private void Construct(GameMenuController menuController)
        {
            _menuController = menuController;
        }

        private int _index;
        private int _animationIndex;

        private bool _menuActive;

        private void Awake()
        {
            _windowService.OnShowWindow += EnablePause;

            _pauseInputs.OnMenuSwitchTabAction += SwitchTab;
            _pauseInputs.OnMenuTabPressed += OpenTab;
            _pauseInputs.OnBackKeyPressed += _windowService.Unpause;

            _weaponButton.onClick.AddListener(WeaponButton);
            _combosButton.onClick.AddListener(CombosButton);
            _abilitiesButton.onClick.AddListener(AbilitiesButton);
            _skillsButton.onClick.AddListener(SkillsButton);
            _journalButton.onClick.AddListener(JournalButton);
            _mapButton.onClick.AddListener(MapButton);

            _prevKey.onClick.AddListener(PrevButton);
            _nextKey.onClick.AddListener(NextButton);

            _backButton.onClick.AddListener(_windowService.Unpause);
            _closeButton.onClick.AddListener(_windowService.Unpause);

            _menuController.OnPlayerMenuOpened += OpenTab;

            _animationIndex = -1;

            _canvasGroup.alpha = 0;

            _tabs.Add(_weaponTab);
            _tabs.Add(_combosTab);
            _tabs.Add(_abilitiesTab);
            _tabs.Add(_skillsTab);
            _tabs.Add(_journalTab);
            _tabs.Add(_mapTab);

            _animations.Add(_weaponAnimation);
            _animations.Add(_combosAnimation);
            _animations.Add(_abilitiesAnimation);
            _animations.Add(_skillsAnimation);
            _animations.Add(_journalAnimation);
            _animations.Add(_mapAnimation);
        }

        private void WeaponButton() { OpenTab(0); }
        private void CombosButton() { OpenTab(1); }
        private void AbilitiesButton() { OpenTab(2); }
        private void SkillsButton() { OpenTab(3); }
        private void JournalButton() { OpenTab(4); }
        private void MapButton() { OpenTab(5); }
        private void PrevButton() { SwitchTab(-1); }
        private void NextButton() { SwitchTab(+1); }

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

            PlayHoverSound();

            CloseAllTabs();
            ShowActiveTabButton();

            _tabs[_index].SetActive(true);
        }

        private void ShowActiveTabButton()
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

        private void OnDestroy()
        {
            _pauseInputs.OnMenuSwitchTabAction -= SwitchTab;
            _pauseInputs.OnMenuTabPressed -= OpenTab;
            _pauseInputs.OnBackKeyPressed -= _windowService.Unpause;

            _weaponButton.onClick.RemoveListener(WeaponButton);
            _combosButton.onClick.RemoveListener(CombosButton);
            _abilitiesButton.onClick.RemoveListener(AbilitiesButton);
            _skillsButton.onClick.RemoveListener(SkillsButton);
            _journalButton.onClick.RemoveListener(JournalButton);
            _mapButton.onClick.RemoveListener(MapButton);

            _prevKey.onClick.RemoveListener(PrevButton);
            _nextKey.onClick.RemoveListener(NextButton);

            _backButton.onClick.RemoveListener(_windowService.Unpause);
            _closeButton.onClick.RemoveListener(_windowService.Unpause);

            _windowService.OnHideWindow -= DisablePause;
            _windowService.OnShowWindow -= EnablePause;

            _menuController.OnPlayerMenuOpened -= OpenTab;
        }
    }
}