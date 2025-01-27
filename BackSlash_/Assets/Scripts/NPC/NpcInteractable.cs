using Scripts.UI.Dialogue;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class NpcInteractable : MonoBehaviour
{
	[SerializeField] private Transform _dialogueLookAt;
	[Space]
	[SerializeField] private string _name;
	[SerializeField] private QuestDatabase _quest;
	[SerializeField] private bool _canTrade;

	private Vector3 _rotation;

	public Transform LookAt => _dialogueLookAt;
	public string Name => _name;
	public QuestDatabase Quest => _quest;
	public bool CanTrade => _canTrade;
	public Vector3 DefaultRotation => _rotation;
	
	private void Awake()
	{
		_rotation = transform.rotation.eulerAngles;
	}
}
