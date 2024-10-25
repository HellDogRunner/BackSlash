using System;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using Zenject;

public class TradeSystem : MonoBehaviour
{
	private InteractionSystem _interactionSystem;
	
	public event Action<List<GameObject>> OnSetInventory;
	
	[Inject]
	private void Construct(InteractionSystem interactionSystem) 
	{
		_interactionSystem = interactionSystem;
	}
	
	private void Awake()
	{
		_interactionSystem.SetTradeInventory += SetInventory;
		_interactionSystem.OnExitTrigger += SetDefault;
	}
	
	private void SetInventory(TraderInventory inventory) 
	{
		// Установка инвентаря трейдера
		List<GameObject> products = new List<GameObject>();
		
		foreach (var item in inventory.Inventory)
		{
			products.Add(item.Icon);
		}
		
		OnSetInventory?.Invoke(products);
	}
	
	private void SetDefault()
	{
		
	}
	
	private void OnDestroy()
	{
		_interactionSystem.SetTradeInventory -= SetInventory;
		_interactionSystem.OnExitTrigger -= SetDefault;
	}
}