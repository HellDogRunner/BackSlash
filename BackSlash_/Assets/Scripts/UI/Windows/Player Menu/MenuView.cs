using UnityEngine;
using Zenject;

namespace Scripts.Menu
{
	public class MenuView : MenuElement
	{
		public MenuDescriptionView Description;
		
		[Header("Weapon")]
		public MenuModView Hilt;
		public MenuModView Guard;
		public MenuModView Blade;

		[Header("Animation Settings")]
		public float GlowFade;
		public float GlowDuration;

		private MenuModel model;

		private CurrencyService currencyService;

		[Inject]
		private void Construct(CurrencyService _currencyService)
		{
			currencyService = _currencyService;
		}

		private void Awake()
		{		
			model = Menu.Model;
			model.Currency.text = currencyService.Currency.ToString();
		}
	}
}
