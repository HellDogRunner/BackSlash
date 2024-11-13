using RedMoonGames.Basics;
using RedMoonGames.Database;
using System.Collections.Generic;
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

		public InventoryModel GetItemTypeModel(EItemType itemType)
		{
			return _data.GetBy(itemModel => itemModel.Type == itemType);
		}

		public InventoryModel GetItemTypeModel(Item item)
		{
			return _data.GetBy(itemModel => itemModel.Item == item);
		}

		public List<InventoryModel> GetAllBuffs()
		{
			List<InventoryModel> temporaryBuffs = new List<InventoryModel>();
			foreach (var item in _data)
			{
				if (item.Type == EItemType.Buff)
				{
					temporaryBuffs.Add(item);
				}
			}
			return temporaryBuffs;
		}

		[ContextMenu("Clear Inventory")]
		private void ClearInventory()
		{
			_data.Clear();
		}
	}
}