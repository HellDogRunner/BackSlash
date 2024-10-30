using System.Collections.Generic;
using Scripts.Inventory;
using TMPro;
using UnityEngine;
using Zenject;

public class TradeWindow : MonoBehaviour
{
	[SerializeField] private PlayerItemsDatabase _playerItems;
	
	[Header("Instantiate Products")]
	[SerializeField] private RectTransform _productsRoot;
	[SerializeField] private GameObject _productTemplate;
	[SerializeField] private TMP_Text _description;
	[Space]
	[SerializeField] private GameObject _buyButton;
	
	[Header("Trade Button Texts")]
	[SerializeField] private GameObject _showTrade;
	[SerializeField] private GameObject _hideTrade;
	
	private List<GameObject> _products = new List<GameObject>();
	private PlayerItemsDatabase _items;
	private TradeProduct _firstProduct;
	
	private bool _windowVisible;
	
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
		
		SwitchTradeText(true);
		_buyButton.SetActive(false);
	}
	
	private void SetInventory(PlayerItemsDatabase items) 
	{
		_items = items;
		
		foreach (var item in _items.GetData())
		{
			var product = _diContainer.InstantiatePrefab(_productTemplate, _productsRoot);
			var tradeProduct = product.GetComponent<TradeProduct>();
			
			tradeProduct.SetProduct(item);
			if (_products.Count == 0) _firstProduct = tradeProduct;
			
			_products.Add(product);
		}
	}

	private void SwitchTradeText(bool enable)
	{
		_showTrade.SetActive(enable);
		_hideTrade.SetActive(!enable);
	}
	
	private void ResetInventory()
	{
		foreach (var product in _products)
		{
			Destroy(product);
		}
		
		_items = null;
		_firstProduct = null;
		_products.Clear();
	}
	
	private void SetCurrency() 
	{
		var value = _currencyService.GetCurrentCurrency();
		var target = _animator.GetCurrency();
		
		_windowVisible = true;
		_currencyAnimation.SetCurrency(target, value);
	}
	
	private void ShowTrade()
	{
		SwitchTradeText(false);
		_animator.Trade(1);
		_buyButton.SetActive(true);
		
		_firstProduct.SelectButton();
		SetCurrency();
	}
	
	private void HideTrade()
	{
		_windowVisible = false;
		
		SwitchTradeText(true);
		_animator.Trade();
		_buyButton.SetActive(false);
	}
	
	public bool TruBuyProduct(int price) 
	{
		if (_currencyService.CheckCurrency(price))
		{
			_currencyService.RemoveCurrency(price);
			return true;
		}
		return false;
	}

	private void ChangeCurrency(int endValue)
	{
		if (_windowVisible) _currencyAnimation.Animate(_animator.GetCurrency(), endValue);
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