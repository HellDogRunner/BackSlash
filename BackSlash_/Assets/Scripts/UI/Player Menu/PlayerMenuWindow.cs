using Scripts.UI.PlayerMenu;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace RedMoonGames.Window
{
	public class PlayerMenuWindow : BasicWindow
	{
		[SerializeField] private RectTransform _tabsRoot;

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
		[SerializeField] private Button _backButton;

		[Inject] private DiContainer _diContainer;

		private List<MenuTabAnimationService> _animations = new List<MenuTabAnimationService>();

		private int _index = -1;
		private int _animationIndex = -1;
		private GameObject _activeTab;

		private GameMenuController _menuController;
		private TabDatabase _tabData;

		[Inject]
		private void Construct(TabDatabase tabData, GameMenuController menuController)
		{
			_tabData = tabData;
			_menuController = menuController;
		}

		private void Awake()
		{
			Show(true, 0);
		}

		// TODO remove this script OR if needed clear dependencies

		protected override void OnEnable()
		{
			base.OnEnable();
			
			//_menuController.OpenTab += OpenTab;
			_uiInputs.OnMenuSwitchTabAction += SwitchTab;
			_uiInputs.OnMenuKeyPressed += OpenTab;
			_uiInputs.OnBackKeyPressed += Hide;

			_weaponButton.onClick.AddListener(WeaponButton);
			_combosButton.onClick.AddListener(CombosButton);
			_abilitiesButton.onClick.AddListener(AbilitiesButton);
			_skillsButton.onClick.AddListener(SkillsButton);
			_journalButton.onClick.AddListener(JournalButton);
			_mapButton.onClick.AddListener(MapButton);
			_prevKey.onClick.AddListener(PrevButton);
			_nextKey.onClick.AddListener(NextButton);
			_backButton.onClick.AddListener(Hide);
			_close.onClick.AddListener(Hide);

			_canvasGroup.alpha = 0;

			_animations.Add(_weaponButton.GetComponent<MenuTabAnimationService>());
			_animations.Add(_combosButton.GetComponent<MenuTabAnimationService>());
			_animations.Add(_abilitiesButton.GetComponent<MenuTabAnimationService>());
			_animations.Add(_skillsButton.GetComponent<MenuTabAnimationService>());
			_animations.Add(_journalButton.GetComponent<MenuTabAnimationService>());
			_animations.Add(_mapButton.GetComponent<MenuTabAnimationService>());
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			
			//_menuController.OpenTab -= OpenTab;
			_uiInputs.OnMenuSwitchTabAction -= SwitchTab;
			_uiInputs.OnMenuKeyPressed -= OpenTab;
			_uiInputs.OnBackKeyPressed -= Hide;

			_weaponButton.onClick.RemoveListener(WeaponButton);
			_combosButton.onClick.RemoveListener(CombosButton);
			_abilitiesButton.onClick.RemoveListener(AbilitiesButton);
			_skillsButton.onClick.RemoveListener(SkillsButton);
			_journalButton.onClick.RemoveListener(JournalButton);
			_mapButton.onClick.RemoveListener(MapButton);
			_prevKey.onClick.RemoveListener(PrevButton);
			_nextKey.onClick.RemoveListener(NextButton);
			_backButton.onClick.RemoveListener(Hide);
			_close.onClick.RemoveListener(Hide);
		}

		private void WeaponButton() { OpenTab(0); }
		private void CombosButton() { OpenTab(1); }
		private void AbilitiesButton() { OpenTab(2); }
		private void SkillsButton() { OpenTab(3); }
		private void JournalButton() { OpenTab(4); }
		private void MapButton() { OpenTab(5); }
		private void PrevButton() { SwitchTab(-1); }
		private void NextButton() { SwitchTab(1); }

		private void SwitchTab(int index)
		{
			OpenTab(_index + index);
		}

		private void OpenTab(int index)
		{
			index = _tabData.GetTabIndex(index);
			var tabModel = _tabData.GetData()[index];

			if (_index == index) return;
			if (_activeTab != null) Destroy(_activeTab);
			_activeTab = _diContainer.InstantiatePrefab(tabModel.Prefab, _tabsRoot);

			_index = index;

			PlayHoverSound();
			ShowActiveTabButton();
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
	}
}
