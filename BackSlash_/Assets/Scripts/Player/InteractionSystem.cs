using RedMoonGames.Window;
using Scripts.Player;
using Scripts.UI.Dialogue;
using Scripts.Weapon;
using System;
using Unity.Cinemachine;
using UnityEngine;
using Zenject;
using static PlayerStates;

public class InteractionSystem : MonoBehaviour
{
	[SerializeField] private WindowHandler _dialogueWindow;
	[Space]
	[SerializeField] private CinemachineCamera _dialogueCamera;
	[Space]
	[SerializeField] private GameObject _player;

	private QuestDatabase _quest;
	private NpcInteractionService _npc;
	private WindowHandler _lastOpenned;

	private bool _isInteracting;
	private bool _inDialogue;
	private bool _canTrade;

	private WeaponController _weaponController;
	private UIActionsController _uiActions;
	private InteractionAnimator _animator;
	private WindowService _windowService;
	private PlayerStateMachine _playerState;

	public event Action<QuestDatabase> SetQuest;
	public event Action OnResetDialogue;
	public event Action EndInteracting;
	public event Action<bool> OnInteracting;
	public event Action OnButtonClick;
	public event Action OnCanTrade;

	[Inject]
	private void Construct(PlayerStateMachine playerState, WeaponController weaponController, WindowService windowService, InteractionAnimator animator, UIActionsController uIActions)
	{
		_animator = animator;
		_uiActions = uIActions;
		_playerState = playerState;
		_windowService = windowService;
		_weaponController = weaponController;
	}

	private void OnEnable()
	{
		_uiActions.OnEnterKeyPressed += TryInteract;
		_uiActions.OnBackKeyPressed += TryStopInteract;
		_playerState.OnPause += Pause;
		_playerState.OnInteract += Interact;
	}

	private void OnDisable()
	{
		_uiActions.OnEnterKeyPressed -= TryInteract;
		_uiActions.OnBackKeyPressed -= TryStopInteract;
		_playerState.OnPause -= Pause;
		_playerState.OnInteract -= Interact;
	}

	public void SetInformation(QuestDatabase quest, bool canTrade, GameObject npc)
	{
		_quest = quest;
		_npc = npc.GetComponent<NpcInteractionService>();
		_canTrade = canTrade;

		SetQuest?.Invoke(_quest);
		if (_quest != null) _animator.ShowTalk();
	}

	public void ResetInformation()
	{
		_quest = null;
		_npc = null;
		_canTrade = false;

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
				_playerState.Interact();

				_isInteracting = true;
				_inDialogue = true;

				_dialogueCamera.Target.LookAtTarget = _npc.LookAt;
				_dialogueCamera.gameObject.SetActive(true);

				_animator.HideTalk();
				_animator.LookAtEachOther(_player.transform);

				if (_weaponController.CurrentWeaponType == EWeaponType.Melee) _weaponController.UnequipWeapon();

				SwitchWindows(_dialogueWindow);
				if (_canTrade)
				{
					_uiActions.OnTradeKeyPressed += TradeButton;
					OnCanTrade?.Invoke();
				}

				return;
			}

			if (_inDialogue) OnInteracting?.Invoke(true);
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

			_playerState.Explore();

			if (_canTrade) _uiActions.OnTradeKeyPressed -= TradeButton;

			EndInteracting?.Invoke();
			_windowService.CloseActiveWindow();

			_animator.ShowTalk();

			_dialogueCamera.gameObject.SetActive(false);
			_dialogueCamera.Target.LookAtTarget = null;
		}
	}

	public void SwitchWindows(WindowHandler window)
	{
		_windowService.CloseActiveWindow();
		_windowService.TryOpenWindow(window);
		_lastOpenned = window;

		_inDialogue = window == _dialogueWindow;
		if (_inDialogue) OnInteracting?.Invoke(false);
	}

	public string GetName()
	{
		return _npc.Name;
	}

	private void TradeButton()
	{
		OnButtonClick?.Invoke();
	}

	private void Pause()
	{
		_uiActions.OnBackKeyPressed -= TryStopInteract;
	}

	private void Interact()
	{
		_uiActions.OnBackKeyPressed += TryStopInteract;
		_windowService.TryOpenWindow(_lastOpenned);
		OnInteracting?.Invoke(false);
	}

	private void SwitchInteract(EState state)
	{
		if (_isInteracting)
		{
			if (state == EState.Pause)
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
}
