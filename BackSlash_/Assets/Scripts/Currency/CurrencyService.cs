using System;
using Scripts.Inventory;
using UnityEngine;

public class CurrencyService : MonoBehaviour
{
	[SerializeField] private PlayerItemsDatabase _playerInventory;

	public event Action<int> OnCurrencyChanged;

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.R)) 
		{
			AddCurrency(1000);
		}
		
		if (Input.GetKeyDown(KeyCode.F)) 
		{
			RemoveCurrency(1000);
		}
	}

	private int GetCurrency()
	{
		return _playerInventory.GetCurrency();
	}

	public void AddCurrency(int value) 
	{
		_playerInventory.SetCurrency(GetCurrency() + value);
		ChangeCurrency();
	}

	public void RemoveCurrency(int value)
	{
		if (CheckCurrency(value))
		{
			_playerInventory.SetCurrency(GetCurrency() - value);
			ChangeCurrency();
		}
	}

	public int GetCurrentCurrency() 
	{
		return _playerInventory.GetCurrency();
	}

	private void ChangeCurrency()
	{
		OnCurrencyChanged?.Invoke(GetCurrency());
	}
	
	public bool CheckCurrency(int value)
	{
		return GetCurrency() >= value;
	}
}