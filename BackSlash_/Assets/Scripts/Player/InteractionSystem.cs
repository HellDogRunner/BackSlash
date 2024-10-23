using Scripts.Player;
using Scripts.UI.Dialogue;
using System;
using Unity.Cinemachine;
using UnityEngine;
using Zenject;

public class InteractionSystem : MonoBehaviour
{
	[SerializeField] private CinemachineCamera _dialogueCamera;

	private NpcInteractionService _npc;
	private bool _canInteract;
	private bool _isInteracting;
	private float _maxDistance;

	private UIActionsController _uiActions;
	private InteractionAnimation _interactionAnimation;

	public event Action<QuestDatabase> SetData;
	public event Action OnStartInteract;
	public event Action OnInteracting;
	public event Action OnExitTrigger;
	public event Action<string> OnEnterTrigger;

	[Inject]
	private void Construct(InteractionAnimation interaction, UIActionsController uIActions)
	{
		_interactionAnimation = interaction;
		_uiActions = uIActions;
	}

	private void Awake()
	{
		_uiActions.OnEnterKeyPressed += TryInteract;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "NPC")
		{
			_npc = other.GetComponent<NpcInteractionService>();
   
			if (_npc.GetQuestData() != null)
			{
				_interactionAnimation.SetTransform(_npc.transform, _npc.GetRotation());
				_maxDistance = other.GetComponent<SphereCollider>().radius + 0.2f;

				OnEnterTrigger?.Invoke(_npc.GetName());
				SetData?.Invoke(_npc.GetQuestData());

				_canInteract = true;
			}
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.tag == "NPC")
		{
			_canInteract = false;

			_interactionAnimation.RotateToDefault();

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

				_dialogueCamera.Target.LookAtTarget = _npc.GetLookAt();
				_dialogueCamera.gameObject.SetActive(true);

				_interactionAnimation.LookAtEachOther(gameObject.transform);

				OnStartInteract?.Invoke();
			}

			if (_isInteracting)
			{
				OnInteracting?.Invoke();
			}
		}
	}

	private bool CanInteract(Transform playerTR)
	{
		if (!_npc) return false;

		float distance = (playerTR.position - _npc.transform.position).magnitude;

		return distance < _maxDistance && _canInteract;
	}

	public void EndInteraction()
	{
		_isInteracting = false;
		
		_dialogueCamera.gameObject.SetActive(false);
		_dialogueCamera.Target.LookAtTarget = null;
	}

	private void OnDestroy()
	{
		_uiActions.OnEnterKeyPressed -= TryInteract;
	}
}