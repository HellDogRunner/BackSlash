using System.Collections.Generic;
using Scripts.Inventory;
using UnityEngine;
using Zenject;

public class TradeProductSetter : MonoBehaviour
{
	[SerializeField] private RectTransform _productsRoot;
	[SerializeField] private GameObject _productTemplate;
	
	private List<GameObject> _products = new List<GameObject>();
	private InventoryDatabase _traderInventory;
    private InventoryDatabase _playerInventory;
    [Inject] private DiContainer _diContainer;
	
	public void SetTradeInventory(InventoryDatabase traderInventory, InventoryDatabase playerInventory) 
	{
		_traderInventory = traderInventory;
        _playerInventory = playerInventory;

        SetItems();
	}
	
	private void SetItems()
	{
		foreach (var buff in _traderInventory.GetData())
		{
			var isPlayerHaveItem = _playerInventory.GetItemTypeModel(buff.Item);

            if (isPlayerHaveItem == null)
			{
                var prefab = _diContainer.InstantiatePrefab(_productTemplate, _productsRoot);
                var product = prefab.GetComponent<Product>();// говно дерьма

                product.SetValues(buff);

                if (_products.Count == 0) product.SelectFirst();

                _products.Add(product.gameObject);
            }
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