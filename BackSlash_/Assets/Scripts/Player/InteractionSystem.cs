using RedMoonGames.Window;
using Scripts.Inventory;
using Scripts.Player;
using Scripts.UI.Dialogue;
using System;
using Unity.Cinemachine;
using UnityEngine;
using Zenject;

public class InteractionSystem : MonoBehaviour
{
	[SerializeField] private WindowHandler _dialogueWindow;
	[SerializeField] private WindowHandler _tradeWindow;
	[Space]
	[SerializeField] private CinemachineCamera _dialogueCamera;
	[SerializeField] private InventoryDatabase _playerInventory;
	[Space]
	[SerializeField] private GameObject _player;
	
	private Transform _lookAt;

	private InventoryDatabase _traderInventory;
	private QuestDatabase _quest;
	
	private bool _isInteracting;
	private bool _inTrade;
	private float _distance;
	private string _name;

	private UIActionsController _uiActions;
	private InteractionAnimator _animator;
	private GameMenuController _menuController;
	private HUDController _hudController;
	private WindowService _windowService;

	public event Action OnReset;	
	public event Action<QuestDatabase> SetQuest;
	public event Action OnInteracting;
	public event Action EndInteracting;

	[Inject]
	private void Construct(WindowService windowService, HUDController hudController, GameMenuController menuController, InteractionAnimator animator, UIActionsController uIActions)
	{
		_animator = animator;
		_uiActions = uIActions;
		_windowService = windowService;
		_hudController = hudController;
		_menuController = menuController;
	}

	private  void OnEnable()
	{
		_uiActions.OnEnterKeyPressed += TryInteract;
		_uiActions.OnBackKeyPressed += TryStopInteract;
		_menuController.OnGamePause += OnGamePause;
	}

	private void OnDisable()
	{
		_uiActions.OnEnterKeyPressed -= TryInteract;
		_uiActions.OnBackKeyPressed -= TryStopInteract;
		_menuController.OnGamePause -= OnGamePause;
	}

	public void SetInformation(QuestDatabase quest, InventoryDatabase inventory, Transform lookAt, float distance, string name)
	{
		_quest = quest;
		_traderInventory = inventory;
		_lookAt = lookAt;
		_distance = distance;
		_name = name;
		
		SetQuest?.Invoke(_quest);
		if (_quest != null) _animator.TalkKey(1);
	}

	public void ResetInformation()
	{
		_quest = null;
		_traderInventory = null;
		_lookAt = null;
		_distance = 0;
		_name = "";
		
		_animator.TalkKey();
		_animator.RotateToDefault();
		OnReset?.Invoke();
	}

	public void TryInteract()
	{
		if (CanInteract())
		{
			if (!_isInteracting)
			{
				_isInteracting = true;

				if (_traderInventory != null) _uiActions.OnTradeKeyPressed += OpenWindow;

				_dialogueCamera.Target.LookAtTarget = _lookAt;
				_dialogueCamera.gameObject.SetActive(true);

				_animator.TalkKey();
				_animator.LookAtEachOther(_player.transform);
				
				_hudController.SwitchOverlay();
				_menuController.SwitchDialogue(true);

				_windowService.TryOpenWindow(_dialogueWindow);
			}

			if (!_inTrade) OnInteracting?.Invoke();
		}
	}

	private bool CanInteract()
	{
		if (!_quest) return false;

		float distance = (_player.transform.position - _lookAt.position).magnitude;

		return distance < _distance;
	}

	public void TryStopInteract()
	{
		if (_isInteracting)
		{
			_isInteracting = false;
			_inTrade = false;

			if (_traderInventory != null) _uiActions.OnTradeKeyPressed -= OpenWindow;

			EndInteracting?.Invoke();

			_animator.TalkKey(1);
			_hudController.SwitchOverlay(1);
			_menuController.SwitchDialogue(false);

			_dialogueCamera.gameObject.SetActive(false);
			_dialogueCamera.Target.LookAtTarget = null;
		}
	}

	public void OpenWindow()
	{
		var window = _inTrade ? _dialogueWindow : _tradeWindow;
		
		_windowService.ReturnActiveWindow()?.Close();
		_windowService.TryOpenWindow(window);
		
		_inTrade = !_inTrade;
	}

	public InventoryDatabase GetPlayerInventory() 
	{
		return _playerInventory;
	}

	public InventoryDatabase GetTraderInventory() 
	{
		return _traderInventory;
	}

	public string GetName()
	{
		return _name;
	}

	private void OnGamePause(bool enable)
	{
		if (enable) _uiActions.OnBackKeyPressed -= TryStopInteract;
		else _uiActions.OnBackKeyPressed += TryStopInteract;
	}
}