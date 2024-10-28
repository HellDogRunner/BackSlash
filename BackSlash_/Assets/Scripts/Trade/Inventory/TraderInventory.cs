using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TraderInventory", menuName = "Scriptable Objects/TraderInventory")]
public class TraderInventory : ScriptableObject
{
	public List<TraderProduct> Inventory;
	public Sprite SoldSprite;
	
	[ContextMenu("Reset inventory")]
	private void ResetInventory() 
	{
		foreach (var product in Inventory) product.Sold = false;
	}
}