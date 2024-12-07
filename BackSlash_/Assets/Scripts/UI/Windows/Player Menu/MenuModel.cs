using Scripts.Inventory;
using TMPro;
using UnityEngine;

namespace Scripts.Menu
{
	public class MenuModel : MenuElement
	{
		[Header("Spawn")]
		public GameObject PlayerProjection; 
		public GameObject WeaponProjection;
		[Space]
		public InventoryDatabase EquippedItems;
		[Space]
		public TMP_Text Currency;
	}
}