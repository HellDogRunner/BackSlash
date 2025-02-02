using Scripts.Player;
using Scripts.UI.Dialogue;
using Scripts.Weapon;
using System;
using Unity.Cinemachine;
using UnityEngine;
using Zenject;

public class InteractionSystem : MonoBehaviour
{
	[SerializeField] private CinemachineCamera _interactCamera;
	[Header("Settings")]
	[SerializeField] private float _angleToInteract;
	
	private bool _interacting = false;
	private NpcInteractable _npc;

	private GameObject _activeWindow;
	
	private QuestSystem _questSystem;
	private WeaponController _weaponController;
	private UiInputsController _uiActions;
	private InteractionAnimator _animator;
	private GameMenuController _menuController;
	private PlayerStateController _stateController;
	
	public event Action OnResetDialogue;
	public event Action<QuestDatabase, string, bool> OnStartInteract;
	
	[Inject]
	private void Construct(GameMenuController menuController, QuestSystem questSystem, PlayerStateController stateController, WeaponController weaponController, InteractionAnimator animator, UiInputsController uIActions)
	{
		_animator = animator;
		_uiActions = uIActions;
		_questSystem = questSystem;
		_menuController = menuController;
		_stateController = stateController;
		_weaponController = weaponController;
	}

	private void OnEnable()
	{
		_uiActions.OnEnterKeyPressed += TryStartInteract;
		_menuController.BeforePaused += SwitchWindow;
	}

	private void OnDisable()
	{
		_uiActions.OnEnterKeyPressed -= TryStartInteract;
		_menuController.BeforePaused -= SwitchWindow;
	}
	
	private void OnDestroy()
	{
		if (_npc)
		{
			_npc.Quest.Index = 0;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "NPC")
		{
			other.gameObject.TryGetComponent(out _npc);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.tag == "NPC")
		{
			_npc = null;
		}
	}

	private void Update()
	{
		if (CanInteract() && !_interacting)
		{
			_animator.ShowTalk();
		}
		else
		{
			_animator.HideTalk();
		}
	}

	public void TryStartInteract()
	{
		if (CanInteract())
		{
			_interacting = true;
			_stateController.SetInteract();
			_animator.HideTalk();
			_animator.SetRotation(_npc.DefaultRotation);
			_animator.LookAtEachOther(transform, _npc.transform);
			SwitchCamera(_npc.LookAt);
			
			//TODO убирать оружие по смене стейта на интеракшн в вепон конторолере
			if (_weaponController.CurrentWeaponType == EWeaponType.Melee) _weaponController.UnequipWeapon();
			
			_questSystem.TryUpdateData(_npc.Quest);
			OnStartInteract?.Invoke(_npc.Quest, _npc.Name, _npc.CanTrade);			
		}
	}

	public void StopInteract()
	{	
		_interacting = false;
		_stateController.SetBeInterrupt();
		_stateController.SetNone();
		_animator.RotateToDefault(_npc.transform);
		_animator.ShowTalk();
		SwitchCamera(null);
		
		OnResetDialogue?.Invoke();
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
		if (!_npc || !_npc.Quest || !_stateController.CanInteract() || _interacting) return false;
		var angleToNPC = (_npc.transform.position - transform.position).normalized;
		var correctAngle = Vector3.Angle(angleToNPC, transform.forward) < _angleToInteract;
		return correctAngle;
	}
	
	public void SetActiveWindow(GameObject window)
	{
		_activeWindow = window;
	}
	
	public void SwitchWindow(bool value)
	{
		if (_interacting)
		{
			_activeWindow.SetActive(!value);
		}
	}
}
