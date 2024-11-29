using RedMoonGames.Basics;
using RedMoonGames.Database;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Scripts.Inventory
{
	[CreateAssetMenu(fileName = "PlayerItems", menuName = "Scriptable/PlayerItems")]
	public class InventoryDatabase : ScriprtableDatabase<InventoryModel>
	{
		public void AddItem(InventoryModel item)
		{
			_data.Add(item);
		}

		public InventoryModel GetModelByType(EItemType itemType)
		{
			return _data.GetBy(itemModel => itemModel.Type == itemType);
		}

		public InventoryModel GetModelByItem(Item item)
		{
			return _data.GetBy(itemModel => itemModel.Item == item);
		}
		
		private List<InventoryModel> GetTypelist(EItemType type)
		{
			var list = new List<InventoryModel>();
			foreach (var item in _data)
			{
				if (item.Type == type)
				{
					list.Add(item);
				}
			}
			return list;
		}
		
		[ContextMenu("Sort Inventory")]
		public void SortData()
		{
			var buffs = GetTypelist(EItemType.Buff);
			var hilts = GetTypelist(EItemType.Hilt);
			var guards = GetTypelist(EItemType.Guard);
			var blades = GetTypelist(EItemType.Blade);
			
			_data.Clear();
			_data = buffs.Concat(hilts).Concat(guards).Concat(blades).ToList();
		}
		
		[ContextMenu("Clear Inventory")]
		private void ClearInventory()
		{
			_data.Clear();
		}
	}
}