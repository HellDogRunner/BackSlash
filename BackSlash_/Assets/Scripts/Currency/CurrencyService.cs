using Scripts.Inventory;
using UnityEngine;
using Zenject;

public class CurrencyService : MonoBehaviour
{
    [SerializeField] private AvailableItemsDatabase _itemsData;

    private int _currency;

    private HUDAnimationService _hudAnimation;

    public int Currency => _currency;

    [Inject]
    private void Construct(HUDAnimationService hudAnimation)
    {
        _hudAnimation = hudAnimation;
    }

    private void Awake()
    {
        // Получение количества валюты
        ChangeCurrency();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) 
        {
            AddCurrency(10);
        }
    }

    public void AddCurrency(int value) 
    {
        // Добавление валюты
        _itemsData.SetCurrency(_currency + value);
        ChangeCurrency();
    }

    public void RemoveCurrency(int value)
    {
        // Убавление валюты
        if (CheckCurrency(value))
        {
            _itemsData.SetCurrency(_currency - value);
            ChangeCurrency();
        }
    }

    private void ChangeCurrency()
    {
        // запуск анимаций и пр.
        _currency = _itemsData.GetCurrency();
        _hudAnimation.CurrencyAnimation(_currency);
    }
    
    private bool CheckCurrency(int value)
    {
        if (_currency >= value) return true;

        return false;
    }
}