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

		private MenuModel model;

		private CurrencyService currencyService;

		[Inject]
		private void Construct(CurrencyService _currencyService)
		{
			currencyService = _currencyService;
		}

		private void Awake()
		{
			Description.SetOnAwake(Menu);
			
			model = Menu.Model;
	
			model.Currency.text = currencyService.Currency.ToString();
		}

		public void ShowDescription(string text)
		{
			Description.Show(text);
		}

		public void FollowingDescription()
		{
			Description.Follow();
		}
		
		public void HideDescription() 
		{
			Description.Hide();
		}
	}
}
