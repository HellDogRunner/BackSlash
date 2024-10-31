using System.Collections.Generic;
using Scripts.Inventory;
using Scripts.UI.Dialogue;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class NpcInteractionService : MonoBehaviour
{
	[SerializeField] private string _name;
	[SerializeField] private QuestDatabase _quest;
	[SerializeField] private InventoryDatabase _items;
	[Space]
	[SerializeField] private Transform _dialogueLookAt;

	private Vector3 _defaultRotation;

	private void Awake()
	{
		_defaultRotation = transform.rotation.eulerAngles;
		gameObject.tag = "NPC";
	}

	public QuestDatabase GetQuestData()
	{
		return _quest;
	}

	public string GetName()
	{
		return _name;
	}

	public Vector3 GetRotation()
	{
		return _defaultRotation;
	}

	public Transform GetLookAt()
	{
		return _dialogueLookAt;
	}
	
	public InventoryDatabase GetTraderInventory()
	{
		return _items;
	}
}