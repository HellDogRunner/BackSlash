using System;
using Scripts.Inventory;
using UnityEngine;

public class CurrencyService : MonoBehaviour
{
	[SerializeField] private AvailableItemsDatabase _playerInventory;

	private int _currency;
	public event Action<int, int> OnCurrencyChanged;

	private void Awake()
	{
		_currency = _playerInventory.GetCurrency();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.R)) 
		{
			AddCurrency(10);
		}
		
		if (Input.GetKeyDown(KeyCode.F)) 
		{
			RemoveCurrency(10);
		}
	}

	public void AddCurrency(int value) 
	{
		_playerInventory.SetCurrency(_currency + value);
		ChangeCurrency();
	}

	public void RemoveCurrency(int value)
	{
		if (CheckCurrency(value))
		{
			_playerInventory.SetCurrency(_currency - value);
			ChangeCurrency();
		}
	}

	public int GetCurrentCurrency() 
	{
		return _playerInventory.GetCurrency();
	}

	private void ChangeCurrency()
	{
		int startValue = _currency;
		_currency = _playerInventory.GetCurrency();
		OnCurrencyChanged?.Invoke(startValue, _currency);
	}
	
	public bool CheckCurrency(int value)
	{
		return _currency >= value;
	}
}