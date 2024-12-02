using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Menu
{
	public class MenuModView : BasicMenuChildView
	{
		[SerializeField] private Image icon;
		[Space]
		[SerializeField] private EItemType Type;
		
		private Item item;
		
		public void SetItem(List<InventoryModel> data)
		{
			foreach (var model in data)
			{
				if (model.Type == Type)
				{
					item = model.Item;
					icon.sprite = item.Icon;
					item.SetValues();
					descriptionText = item.Stats;
					return;
				}
			}
		}
	}
}
