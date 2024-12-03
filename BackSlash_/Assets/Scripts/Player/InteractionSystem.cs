using RedMoonGames.Window;
using Scripts.Player;
using Scripts.UI.Dialogue;
using Scripts.Weapon;
using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using Zenject;
using static PlayerStates;

public class InteractionSystem : MonoBehaviour
{
	[SerializeField] private CinemachineCamera _interactCamera;
	[Space]
	[SerializeField] private GameObject _playerObject;

	private QuestDatabase _quest;
	private NpcInteractionService _npc;

	private List<IWindow> _openedWindows = new List<IWindow>();
	private bool _canTrade;
	private GameObject _disabledWindow;

	private WeaponController _weaponController;
	private UiInputsController _uiActions;
	private InteractionAnimator _animator;
	private PlayerStateMachine _playerSM;

	public event Action<QuestDatabase> SetQuest;
	public event Action OnResetDialogue;
	public event Action<QuestDatabase, string, bool> OnStartInteract;
	
	[Inject]
	private void Construct(PlayerStateMachine playerState, WeaponController weaponController, InteractionAnimator animator, UiInputsController uIActions)
	{
		_animator = animator;
		_uiActions = uIActions;
		_playerSM = playerState;
		_weaponController = weaponController;
	}

	private void OnEnable()
	{
		_uiActions.OnEnterKeyPressed += TryStartInteract;
		
		_playerSM.OnInteract += ActivateWindow;
		_playerSM.OnExplore += CloseAllWindows;
	}

	private void OnDisable()
	{
		_uiActions.OnEnterKeyPressed -= TryStartInteract;
		
		_playerSM.OnInteract -= ActivateWindow;
		_playerSM.OnExplore -= CloseAllWindows;
	}

	public void SetInformation(QuestDatabase quest, NpcInteractionService npc)
	{
		_quest = quest;
		_npc = npc;
		_canTrade = npc.CanTrade;

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
				
			OnStartInteract?.Invoke(_quest, _npc.name, _canTrade);			
		}
	}

	public void StopInteract()
	{	
		_animator.ShowTalk();
		SwitchCamera(null);
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
	
	private void ActivateWindow() 
	{
		if (_disabledWindow != null)
		{
			_disabledWindow.SetActive(true);
			_disabledWindow = null;
		}
	}
	
	public void TryAddWindow(IWindow window)
	{
		if (!_openedWindows.Contains(window)) _openedWindows.Add(window);
	}
	
	public void TryRemoveWindow(IWindow window)
	{
		if (_openedWindows.Contains(window)) _openedWindows.Remove(window);
	}
	
	public void CloseAllWindows()
	{
		foreach (var iwindow in _openedWindows)
		{
			iwindow.Close();
		}
		_openedWindows.Clear();
	}
	
	public void DisableWindow(GameObject window) 
	{
		_disabledWindow = window;
		window.SetActive(false);
	}
}
