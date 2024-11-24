using System.Collections.Generic;
using Scripts.Inventory;
using UnityEngine;
using Zenject;

public class TradeSystem : MonoBehaviour
{
	[SerializeField] private InventoryDatabase _traderInventory;

	[SerializeField] private List<InventoryModel> _models = new List<InventoryModel>();

	private InventoryDatabase _playerInventory;

	[Inject]
	private void Construct(InventoryDatabase playerInventory)
	{
		_playerInventory = playerInventory;
	}

	private void Awake()
	{
		SetTraderItems();
	}

	private void SetTraderItems()
	{
		foreach (var item in _traderInventory.GetData())
		{
			if (_playerInventory.GetItemTypeModel(item.Item) != null) continue;

			_models.Add(item);
		}
	}
	
	public List<InventoryModel> GetItems()
	{
		return _models;
	}
}