using RedMoonGames.Window;
using Scripts.Player;
using Scripts.UI.Dialogue;
using Scripts.Weapon;
using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;
using static PlayerStates;

public class InteractionSystem : MonoBehaviour
{
	[SerializeField] private CinemachineCamera _interactCamera;
	[Space]
	[SerializeField] private GameObject _playerObject;

	public List<GameObject> _windows = new List<GameObject>();
	private QuestDatabase _quest;
	private NpcInteractionService _npc;

	private bool _canTrade;

	private WeaponController _weaponController;
	private UIActionsController _uiActions;
	private InteractionAnimator _animator;
	private WindowService _windowService;
	private PlayerStateMachine _playerSM;

	public event Action<QuestDatabase> SetQuest;
	public event Action OnResetDialogue;
	public event Action<string, bool> OnStartInteract;

	[Inject]
	private void Construct(PlayerStateMachine playerState, WeaponController weaponController, WindowService windowService, InteractionAnimator animator, UIActionsController uIActions)
	{
		_animator = animator;
		_uiActions = uIActions;
		_playerSM = playerState;
		_windowService = windowService;
		_weaponController = weaponController;
	}

	private void OnEnable()
	{
		_uiActions.OnEnterKeyPressed += TryStartInteract;
		
		_playerSM.OnChangeState += ActivateWindows;
	}

	private void OnDisable()
	{
		_uiActions.OnEnterKeyPressed -= TryStartInteract;
		
		_playerSM.OnChangeState -= ActivateWindows;
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

	public void TryStartInteract()
	{
		if (CanInteract())
		{
			_playerSM.Interact();
			_animator.HideTalk();
			_animator.LookAtEachOther(_playerObject.transform);
			SwitchCamera(_npc.LookAt);
			
			if (_weaponController.CurrentWeaponType == EWeaponType.Melee) _weaponController.UnequipWeapon();
				
			OnStartInteract?.Invoke(_npc.name, _canTrade);			
		}
	}

	public void SetExplore()
	{	
		if (_playerSM.State != EState.Explore)
		{
			_playerSM.Explore();
			_animator.ShowTalk();
			SwitchCamera(null);
		}
	}
	
	private void SwitchCamera(Transform lookAt)
	{
		if (_interactCamera != null)
		{	
			_interactCamera.Target.LookAtTarget = lookAt;
			_interactCamera.gameObject.SetActive(!(lookAt is null));
		}
	}

	private bool CanInteract()
	{
		if (!_quest || _playerSM.State == EState.Interact) return false;
		
		float distance = (_playerObject.transform.position - _npc.transform.position).magnitude;

		return distance < _npc.Distance;
	}
	
	private void ActivateWindows(EState state) 
	{
		if (state == EState.Explore)
		{
			_windows.Clear();
			return;
		}
		
		if (state == EState.Interact || state == EState.Pause)
		{
			bool activate = state == EState.Interact ? true : false;
			
			foreach (var window in _windows)
			{
				window.SetActive(activate);
			}
		}
	}
}
