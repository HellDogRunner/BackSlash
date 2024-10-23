using UnityEngine;
using Zenject;

public class TradeSystem : MonoBehaviour
{
	private CurrencyService _currencyService;
	
	[Inject]
	private void Construct(CurrencyService currencyService) 
	{
		_currencyService = currencyService;
	}
	
	private void Awake()
	{
		_currencyService.OnCheckCurrency += BuyingConfirm;
	}
	
	public void TryBuy(int price) 
	{
		_currencyService.TryRemoveCurrency(price);
	}
	
	private void BuyingConfirm(bool confirm) 
	{
		if (confirm) 
		{
			// Покурка состоялась
			Debug.Log("Buy Success");
		}
		else 
		{
			// Недостаточно средств
			Debug.Log("Not enough currency");
		}
	}
	
	private void OnDestroy()
	{
		_currencyService.OnCheckCurrency -= BuyingConfirm;
	}
}
