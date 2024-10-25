using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TraderInventory", menuName = "Scriptable Objects/TraderInventory")]
public class TraderInventory : ScriptableObject
{
	public List<TraderProduct> Inventory;
}