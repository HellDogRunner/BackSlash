using Scripts.UI.Dialogue;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(SphereCollider))]
public class NpcInteractionService : MonoBehaviour
{
	[SerializeField] private string _name;
	[SerializeField] private QuestDatabase _quest;
	[SerializeField] private bool _canTrade;
	[Space]
	[SerializeField] private Transform _dialogueLookAt;

	private Vector3 _defaultRotation;
	private float _distance; 
	
	private InteractionAnimator _animator;
	private InteractionSystem _interaction;
	
	public bool CanTrade => _canTrade;
	public string Name => _name;
	public float Distance => _distance;
	public Transform LookAt => _dialogueLookAt;
	
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
			_interaction.SetInformation(_quest, this);
			
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