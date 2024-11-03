using System.Collections.Generic;
using Scripts.Inventory;
using UnityEngine;
using Zenject;

public class TradeProtuctSetter : MonoBehaviour
{
	[Header("Roots")]
	[SerializeField] private RectTransform _buffRoot;
	[SerializeField] private RectTransform _modsRoot;
	
	[Header("Product Templates")]
	[SerializeField] private GameObject _buff;
	[SerializeField] private GameObject _bladeMod;
	[SerializeField] private GameObject _guardMod;
	
	private List<GameObject> _products = new List<GameObject>();
	private InventoryDatabase _traderInventory;
	
	[Inject] private DiContainer _diContainer;
	
	public void SetTradeInventory(InventoryDatabase inventory) 
	{
		_traderInventory = inventory;
		
		SetBuffs();
		SetBlades();
		SetGuards();
	}
	
	private void SetBuffs()
	{
		foreach (var item in _traderInventory.GetBuffs())
		{
			if (item.Have) continue; 
			
			var product = _diContainer.InstantiatePrefab(_buff, _buffRoot);
			var buff = product.GetComponent<BuffProduct>();
			
			buff.SetProduct(item);
			if (_products.Count == 0) buff.SelectFirst();
			
			_products.Add(product);
		}
	}
	
	private void SetBlades()
	{
		foreach (var item in _traderInventory.GetBlades())
		{
			if (item.Have) continue;
			
			var product = _diContainer.InstantiatePrefab(_bladeMod, _modsRoot);
			var bladeMod = product.GetComponent<BladeModProduct>();
			
			bladeMod.SetProduct(item);
			if (_products.Count == 0) bladeMod.SelectFirst();
			
			_products.Add(product);
		}
	}
	
	private void SetGuards()
	{
		foreach (var item in _traderInventory.GetGuards())
		{
			if (item.Have) continue;
			
			var product = _diContainer.InstantiatePrefab(_guardMod, _modsRoot);
			var bladeMod = product.GetComponent<GuardModProduct>();
			
			bladeMod.SetProduct(item);
			if (_products.Count == 0) bladeMod.SelectFirst();
			
			_products.Add(product);
		}
	}
	
	public void ResetInventory()
	{
		foreach (var product in _products)
		{
			Destroy(product);
		}
		
		_traderInventory = null;
		_products.Clear();
	}
}