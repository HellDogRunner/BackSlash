using RedMoonGames.Window;
using Scripts.Player;
using Scripts.UI.Dialogue;
using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class InteractionSystem : MonoBehaviour
{
	[SerializeField] private CinemachineCamera _dialogueCamera;
	
	private Button _backButton;
	private Button _nextButton;
	private Button _tradeButton;
	
	private NpcInteractionService _npc;
	private bool _canInteract;
	private bool _isInteracting;
	private bool _canTrade;
	private bool _inTrade;
	private float _maxDistance;

	private UIActionsController _uiActions;
	private InteractionAnimator _animator;
	private GameMenuController _menuController;
	private HUDController _hudController;

	public event Action OnExitTrigger;
	public event Action OnEnterTrigger;
	public event Action<QuestDatabase> SetQuestData;
	public event Action<TraderInventory> SetTradeInventory;
	public event Action ShowDialogue;
	public event Action ShowTrade;
	public event Action OnInteracting;
	public event Action EndInteracting;

	[Inject]
	private void Construct(HUDController hudController, GameMenuController menuController, InteractionAnimator animator, UIActionsController uIActions)
	{
		_animator = animator;
		_uiActions = uIActions;
		_hudController = hudController;
		_menuController = menuController;
	}

	private void Awake()
	{
		_uiActions.OnEnterKeyPressed += TryInteract;
		_uiActions.OnBackKeyPressed += TryStopInteract;
		_menuController.OnGamePause += OnGamePause;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "NPC")
		{
			_npc = other.GetComponent<NpcInteractionService>();
   
			if (_npc.GetQuestData() == null) return;
   
			_canInteract = true;
			
			_animator.SetTransform(_npc.transform, _npc.GetRotation());
			_maxDistance = other.GetComponent<SphereCollider>().radius + 0.2f;

			OnEnterTrigger?.Invoke();
			SetQuestData?.Invoke(_npc.GetQuestData());

			if (_npc.GetTraderInventory() != null) 
			{
				_canTrade = true;
				
				SetTradeInventory?.Invoke(_npc.GetTraderInventory());
			}
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.tag == "NPC")
		{
			_canInteract = false;
			_canTrade = false;
			
			_animator.RotateToDefault();

			OnExitTrigger?.Invoke();
		}
	}

	private void TryInteract()
	{
		if (CanInteract(transform))
		{
			if (!_isInteracting)
			{
				_isInteracting = true;

				_backButton.onClick.AddListener(TryStopInteract);
				_nextButton.onClick.AddListener(TryInteract);
				if (_canTrade)
				{
					_uiActions.OnTradeKeyPressed += SwitchInteractionWindow;
					_tradeButton.onClick.AddListener(SwitchInteractionWindow);
					_tradeButton.gameObject.SetActive(true);
				}
				
				_dialogueCamera.Target.LookAtTarget = _npc.GetLookAt();
				_dialogueCamera.gameObject.SetActive(true);

				_animator.TalkKey();
				_animator.LookAtEachOther(gameObject.transform);
				_animator.SetName(_npc.GetName());
				_hudController.SwitchOverlay();
				_menuController.SwitchDialogue(true);

				ShowDialogue?.Invoke();
			}
			
			if (!_inTrade) OnInteracting?.Invoke();
		}
	}

	private bool CanInteract(Transform playerTR)
	{
		if (!_npc) return false;

		float distance = (playerTR.position - _npc.transform.position).magnitude;

		return distance < _maxDistance && _canInteract;
	}
	
	public void TryStopInteract() 
	{
		if (_isInteracting) 
		{
			_isInteracting = false;
			_inTrade = false;
			
			_backButton.onClick.RemoveListener(TryStopInteract);
			_nextButton.onClick.RemoveListener(TryInteract);
			if (_canTrade)
			{
				_uiActions.OnTradeKeyPressed -= SwitchInteractionWindow;
				_tradeButton.onClick.RemoveListener(SwitchInteractionWindow);
				_tradeButton.gameObject.SetActive(false);
			}
			
			EndInteracting?.Invoke();
			
			_animator.TalkKey();
			_hudController.SwitchOverlay(1);
			_menuController.SwitchDialogue(false);
		
			_dialogueCamera.gameObject.SetActive(false);
			_dialogueCamera.Target.LookAtTarget = null;
		}
	}

	public void SetInteractionButtons(Button back, Button next, Button trade)
	{
		_backButton = back;
		_nextButton = next;
		_tradeButton = trade;
	}

	public void SwitchInteractionWindow()
	{
		if (_inTrade)
		{
			ShowDialogue?.Invoke();
		}
		else
		{
			ShowTrade?.Invoke();
		}
		_inTrade = !_inTrade;
	}

	private void OnGamePause(bool enable)
		{
			if (enable) _uiActions.OnBackKeyPressed -= TryStopInteract;
			else _uiActions.OnBackKeyPressed += TryStopInteract;
		}

	private void OnDestroy()
	{
		_uiActions.OnEnterKeyPressed -= TryInteract;
		_uiActions.OnBackKeyPressed -= TryStopInteract;
		_menuController.OnGamePause -= OnGamePause;
		
	}
}