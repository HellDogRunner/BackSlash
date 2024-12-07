using DG.Tweening;
using UnityEngine;
using Zenject;

namespace Scripts.Menu
{
	public class MenuView : MenuElement
	{
		public MenuDescriptionView Description;
		public MenuExperienceView Experience;
		
		[Header("Weapon")]
		public MenuModView Hilt;
		public MenuModView Guard;
		public MenuModView Blade;
		
		[Header("Animation Settings")]
		public float Duration;
		public float Delay;
		
		[Header("Limits")]
		[SerializeField] private Vector2 min;
		[SerializeField] private Vector2 max;

		private Vector2 screen;
		Vector2 itemPivot;
		Vector2 descrPivot;

		private CurrencyService currencyService;

		[Inject]
		private void Construct(CurrencyService _currencyService)
		{
			currencyService = _currencyService;
		}

		private void Awake()
		{
			screen = new Vector2(Screen.width, Screen.height);
					
			Menu.Model.Currency.text = currencyService.Currency.ToString();
			
			SetViews();
		}
		
		private void SetViews()
		{
			Hilt.AwakeSet(this);
			Guard.AwakeSet(this);
			Blade.AwakeSet(this);
			Experience.AwakeSet(this);
			
			Description.OnAwake(Menu.View);
		}
		
		private void ChangeItemPos(RectTransform rt)
		{
			var size = rt.rect.size;
			var deltaPivot = rt.pivot - itemPivot;
			var deltaPosition = new Vector3(deltaPivot.x * size.x, deltaPivot.y * size.y);
			
			rt.pivot = itemPivot;
			rt.localPosition -= deltaPosition;
		}
		
		private void DefinePivot(Vector3 position)
		{	
			itemPivot = new Vector2(0.5f, 0);
			descrPivot = new Vector2(0.5f, 1);
			
			if (position.y < screen.y * min.y)
			{
				itemPivot.y = 1;
				descrPivot.y = 0;
			}
			
			if (position.y > screen.y * max.y)
			{
				itemPivot.y = 0;
				descrPivot.y = 1;
			}
			
			if (position.x < screen.x * min.x)
			{
				itemPivot.x = 0;
				descrPivot.x = 0;
			}
			
			if (position.x > screen.x * max.x)
			{
				itemPivot.x = 1;
				descrPivot.x = 1;
			}
		}
		
		public Vector2 SetPivot(RectTransform rectTransform)
		{
			DefinePivot(rectTransform.position);
			ChangeItemPos(rectTransform);
			
			return descrPivot;
		}
		
		public void KillTween(Tween tween)
		{
			if (tween.IsActive()) tween.Kill();
		}
	}
}
