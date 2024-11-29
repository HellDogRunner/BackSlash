using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Scripts.Menu
{
	public class MenuModView : MenuElement, IPointerEnterHandler, IPointerMoveHandler, IPointerExitHandler
	{
		private PlayerMenu menu;

		private Item item;
		
		public EItemType Type;
		[Space]
		[SerializeField] private Image Icon;
		
		public void OnPointerEnter(PointerEventData eventData)
		{
			menu.View.ShowDescription(item.Stats);
		}

		public void OnPointerMove(PointerEventData eventData)
		{
			menu.View.FollowingDescription();
		}
		
		public void OnPointerExit(PointerEventData eventData)
		{
			menu.View.HideDescription();
		}
		
		public void SetOnAwake(PlayerMenu menu, List<InventoryModel> data)
		{
			this.menu = menu;
			
			foreach (var model in data)
			{
				if (model.Type == Type)
				{
					item = model.Item;
					Icon.sprite = item.Icon;
					item.SetValues();
					return;
				}
			}
		}

	}
}
