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
	[Space]
	[SerializeField] private CinemachineCamera _dialogueCamera;
	[SerializeField] private InventoryDatabase _playerInventory;
	[Space]
	[SerializeField] private GameObject _player;

	private InventoryDatabase _traderInventory;
	private QuestDatabase _quest;
	private NpcInteractionService _npc;
	private WindowHandler _lastOpenned; 
	
	private bool _isInteracting;
	private bool _inTrade;

	private GameMenuController _menuController;
	private UIActionsController _uiActions;
	private InteractionAnimator _animator;
	private HUDController _hudController;
	private WindowService _windowService;

	public event Action<QuestDatabase> SetQuest;
	public event Action OnResetDialogue;	
	public event Action EndInteracting;
	public event Action<bool> OnInteracting;
	public event Action OnButtonClick;

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

	public void SetInformation(QuestDatabase quest, InventoryDatabase inventory, GameObject npc)
	{
		_quest = quest;
		_traderInventory = inventory;
		_npc = npc.GetComponent<NpcInteractionService>();
		
		SetQuest?.Invoke(_quest);
		if (_quest != null) _animator.ShowTalk();
	}

	public void ResetInformation()
	{
		_quest = null;
		_traderInventory = null;
		_npc = null;
		
		_animator.HideTalk();
		_animator.RotateToDefault();
		OnResetDialogue?.Invoke();
	}

	public void TryInteract()
	{
		if (CanInteract())
		{
			if (!_isInteracting)
			{
				_isInteracting = true;

				if (_traderInventory != null) _uiActions.OnTradeKeyPressed += TradeButton;

				_dialogueCamera.Target.LookAtTarget = _npc.LookAt;
				_dialogueCamera.gameObject.SetActive(true);

				_animator.HideTalk();
				_animator.LookAtEachOther(_player.transform);
				
				_hudController.SwitchOverlay();
				_menuController.SwitchDialogue(true);

				SwitchWindows(_dialogueWindow);
				return;
			}

			if (!_inTrade) OnInteracting?.Invoke(true);
		}
	}

	private bool CanInteract()
	{
		if (!_quest) return false;

		float distance = (_player.transform.position - _npc.transform.position).magnitude;

		return distance < _npc.Distance;
	}

	public void TryStopInteract()
	{
		if (_isInteracting)
		{
			_isInteracting = false;
			_inTrade = false;

			if (_traderInventory != null) _uiActions.OnTradeKeyPressed -= TradeButton;

			EndInteracting?.Invoke();
			_windowService.CloseActiveWindow();

			_animator.ShowTalk();
			_hudController.SwitchOverlay(1);
			_menuController.SwitchDialogue(false);

			_dialogueCamera.gameObject.SetActive(false);
			_dialogueCamera.Target.LookAtTarget = null;
		}
	}

	public void SwitchWindows(WindowHandler window)
	{
		_windowService.CloseActiveWindow();
		_windowService.TryOpenWindow(window);
		_lastOpenned = window;
		
		if (window == _dialogueWindow) OnInteracting?.Invoke(false);
		else _inTrade = true;
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
		return _npc.Name;
	}

	private void TradeButton()
	{
		OnButtonClick?.Invoke();
	}

	private void OnGamePause(bool paused)
	{
		if (paused)
		{
			_uiActions.OnBackKeyPressed -= TryStopInteract;
		}
		else
		{
			_uiActions.OnBackKeyPressed += TryStopInteract;
			_windowService.TryOpenWindow(_lastOpenned);
			OnInteracting?.Invoke(false);
		}
	}
}