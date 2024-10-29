using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

public class TradeWindow : MonoBehaviour
{
	[SerializeField] private RectTransform _productsRoot;
	[SerializeField] private GameObject _productTemplate;
	[SerializeField] private TMP_Text _description;
	
	private List<GameObject> _products = new List<GameObject>();
	private TraderInventory _trader;
	
	private bool _currencyVisible;
	
	private InteractionSystem _interactionSystem;
	private InteractionAnimator _animator;
	private CurrencyAnimation _currencyAnimation;
	private CurrencyService _currencyService;
	
	[Inject] private DiContainer _diContainer;
	
	[Inject]
	private void Construct(InteractionAnimator animator, CurrencyService currencyService, CurrencyAnimation currencyAnimation, InteractionSystem interactionSystem)
	{
		_currencyService = currencyService;
		_interactionSystem = interactionSystem;
		_currencyAnimation = currencyAnimation;
		_animator = animator;
	}
	
	private void Awake()
	{
		_interactionSystem.ShowDialogue += HideTrade;
		_interactionSystem.ShowTrade += ShowTrade;
		_interactionSystem.EndInteracting += HideTrade;
		_interactionSystem.SetTradeInventory += SetInventory;
		_interactionSystem.OnExitTrigger += ResetInventory;
		
		_currencyService.OnCurrencyChanged += ChangeCurrency;
	}
	
	private void SetInventory(TraderInventory trader) 
	{
		_trader = trader;
		
		foreach (var item in _trader.Inventory)
		{
			var product = _diContainer.InstantiatePrefab(_productTemplate, _productsRoot);
			
			product.GetComponent<TradeProduct>().SetProduct(item);
			
			_products.Add(product);
		}
	}
	
	private void ResetInventory()
	{
		foreach (var product in _products)
		{
			Destroy(product);
		}
		
		_trader = null;
		_products.Clear();
	}
	
	private void SetCurrency() 
	{
		var value = _currencyService.GetCurrentCurrency();
		var target = _animator.GetCurrency();
		
		_currencyVisible = true;
		_currencyAnimation.SetCurrency(target, value);
	}
	
	private void ShowTrade()
	{
		_animator.Trade(1);
		SetCurrency();
	}
	
	private void HideTrade()
	{
		_animator.Trade();
		_currencyVisible = false;
	}
	
	public bool TruBuyProduct(TraderProduct product) 
	{
		if (_currencyService.CheckCurrency(product.Price))
		{
			_currencyService.RemoveCurrency(product.Price);
			return true;
		}
		return false;
	}

	private void ChangeCurrency(int endValue)
	{
		if (_currencyVisible) _currencyAnimation.Animate(_animator.GetCurrency(), endValue);
	}

	public void ProductSelected(string description) 
	{
		_description.text = description;
	}
	
	private void OnDestroy()
	{
		_interactionSystem.ShowDialogue -= HideTrade;
		_interactionSystem.ShowTrade -= ShowTrade;
		_interactionSystem.EndInteracting -= HideTrade;
		_interactionSystem.SetTradeInventory -= SetInventory;
		_interactionSystem.OnExitTrigger -= ResetInventory;
		
		_currencyService.OnCurrencyChanged -= ChangeCurrency;
	}
}