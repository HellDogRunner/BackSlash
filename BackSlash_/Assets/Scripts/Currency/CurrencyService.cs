using RedMoonGames.Basics;
using System;
using UnityEngine;

public class CurrencyService : MonoBehaviour
{
    [SerializeField] private int _currency = 10000;

    public int Currency => _currency;

    public event Action<int> OnCurrencyChanged;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            AddCurrency(1000);
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (_currency < 1000) TryRemoveCurrency(_currency);
            else TryRemoveCurrency(1000);
        }
    }

    public void AddCurrency(int value)
    {
        _currency += value;
        OnCurrencyChanged?.Invoke(_currency);
    }

    public TryResult TryRemoveCurrency(int value)
    {
        if (_currency - value <= 0)
        {
            return TryResult.Fail;
        }
        _currency -= value;
        OnCurrencyChanged?.Invoke(_currency);
        return TryResult.Successfully;
    }
}