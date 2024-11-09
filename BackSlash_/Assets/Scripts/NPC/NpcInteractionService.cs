using Scripts.Inventory;
using Scripts.UI.Dialogue;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(SphereCollider))]
public class NpcInteractionService : MonoBehaviour
{
	[SerializeField] private string _name;
	[SerializeField] private QuestDatabase _quest;
	[SerializeField] private InventoryDatabase _inentory;
	[Space]
	[SerializeField] private Transform _dialogueLookAt;

	private Vector3 _defaultRotation;
	private float _distance; 
	
	private InteractionAnimator _animator;
	private InteractionSystem _interaction;
	
	[Inject]
	private void Construct(InteractionAnimator animator, InteractionSystem interaction)
	{
		_interaction = interaction;
		_animator = animator;
	}

	private void Awake()
	{
		_defaultRotation = transform.rotation.eulerAngles;
		
		_distance = GetComponent<SphereCollider>().radius + 0.2f;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "CharacterController")
		{
			_interaction.SetInformation(_quest, _inentory, _dialogueLookAt, _distance, _name);
			
			_animator.SetTransform(transform, _defaultRotation);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.tag == "CharacterController")
		{
			_interaction.ResetInformation();
		}
	}
}