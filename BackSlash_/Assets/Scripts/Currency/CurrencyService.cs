using System;
using Scripts.Inventory;
using UnityEngine;

public class CurrencyService : MonoBehaviour
{
	[SerializeField] private AvailableItemsDatabase _itemsData;

	private int _currency;

	public event Action<bool> OnCheckCurrency;
	public event Action<int, int> OnCurrencyChanged;

	private void Awake()
	{
		// Получение количества валюты
		ChangeCurrency();
	}

	private void Update()
	{
		if (Input.GetKey(KeyCode.R)) 
		{
			AddCurrency(10);
		}
		
		if (Input.GetKey(KeyCode.F)) 
		{
			TryRemoveCurrency(10);
		}
	}

	public void AddCurrency(int value) 
	{
		// Добавление валюты
		_itemsData.SetCurrency(_currency + value);
		ChangeCurrency();
	}

	public void TryRemoveCurrency(int value)
	{
		// Трата валюты
		if (CheckCurrency(value))
		{
			_itemsData.SetCurrency(_currency - value);
			ChangeCurrency();
		}
	}

	private void ChangeCurrency()
	{
		// запуск анимаций и пр.
		int startValue = _currency;
		_currency = _itemsData.GetCurrency();
		OnCurrencyChanged?.Invoke(startValue, _currency);
	}
	
	private bool CheckCurrency(int value)
	{
		bool check = _currency >= value;
		
		OnCheckCurrency?.Invoke(check);
		return check;
	}
}