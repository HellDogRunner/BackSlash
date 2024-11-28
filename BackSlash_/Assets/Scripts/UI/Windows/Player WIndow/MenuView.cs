using UnityEngine;
using Zenject;

namespace Scripts.Menu
{
	public class MenuView : MenuElement
	{
		[SerializeField] protected MenuWeaponView weapon;

		private CurrencyService _currencyService;

		[Inject]
		private void Construct(CurrencyService currencyService)
		{
			_currencyService = currencyService;
		}

		public void SetCurrency()
		{
			menu.model.currency.text = _currencyService.Currency.ToString();
		}
	}
}
