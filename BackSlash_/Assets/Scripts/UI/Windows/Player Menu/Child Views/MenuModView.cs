using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Scripts.Menu
{
	public class MenuModView : MenuElement, IPointerEnterHandler, IPointerExitHandler
	{
		private PlayerMenu menu;
		private MenuView view;

		private Item item;
		
		public EItemType Type;
		[Space]
		[SerializeField] private Image icon;
		[SerializeField] private CanvasGroup glow;

		private float fade;
		private float duration;
		
		private void Awake()
		{
			glow.alpha = 0;
			
			fade = view.GlowFade;
			duration = view.GlowDuration;
		}
		
		public void OnPointerEnter(PointerEventData eventData)
		{
			view.Description.Show(item.Stats, transform.position);
			glow.DOFade(fade, duration).SetUpdate(true);
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			view.Description.Hide();
			glow.DOFade(0, duration).SetUpdate(true);
		}
		
		public void SetOnAwake(PlayerMenu menu, List<InventoryModel> data)
		{
			this.menu = menu;
			view = this.menu.View;
			
			foreach (var model in data)
			{
				if (model.Type == Type)
				{
					item = model.Item;
					icon.sprite = item.Icon;
					item.SetValues();
					return;
				}
			}
		}
	}
}
