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

		private CurrencyService currencyService;

		[Inject]
		private void Construct(CurrencyService _currencyService)
		{
			currencyService = _currencyService;
		}

		private void Awake()
		{		
			Menu.Model.Currency.text = currencyService.Currency.ToString();
			
			SetViews();
		}
		
		private void SetViews()
		{
			Hilt.SetView(this);
			Guard.SetView(this);
			Blade.SetView(this);
			Experience.SetView(this);
			
			Description.OnAwake(Menu.View);
		}
		
		public void KillTween(Tween tween)
		{
			if (tween.IsActive()) tween.Kill();
		}
	}
}
