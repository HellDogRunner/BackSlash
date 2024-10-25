using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class TradeWindow : MonoBehaviour
{
	[SerializeField] private RectTransform _productsRoot;
	[SerializeField] private Image _image;
	
	private List<GameObject> _products;
	
	private InteractionSystem _interactionSystem;
	private TradeSystem _tradeSystem;
	private InteractionAnimator _animator;
	private CurrencyAnimation _currencyAnimation;
	private CurrencyService _currencyService;
	
	[Inject]
	private void Construct(InteractionAnimator animator, CurrencyService currencyService, CurrencyAnimation currencyAnimation, InteractionSystem interactionSystem, TradeSystem tradeSystem)
	{
		_currencyService = currencyService;
		_interactionSystem = interactionSystem;
		_currencyAnimation = currencyAnimation;
		_tradeSystem = tradeSystem;
		_animator = animator;
	}
	
	private void Awake()
	{
		_interactionSystem.ShowDialogue += HideTrade;
		_interactionSystem.ShowTrade += ShowTrade;
		_interactionSystem.EndInteracting += HideTrade;
		
		_currencyService.OnCurrencyChanged += ChangeCurrency;
		_currencyService.OnCheckCurrency += BuyingConfirm;
		_tradeSystem.OnSetInventory += InstantiateProducts;
	}
	
	private void InstantiateProducts(List<GameObject> products) 
	{
		List<Image> icons = new List<Image>();
		
		foreach (var product in products)
		{
			Instantiate(product, _productsRoot);
		}
	}
	
	private void ShowTrade()
	{
		//_button.onClick.AddListener(TruBuyProduct);
		
		_animator.Trade(1);
	}
	
	private void HideTrade()
	{
		//_button.onClick.RemoveListener(TruBuyProduct);
		
		_animator.Trade();
	}
	
	private void TruBuyProduct() 
	{
		_currencyService.TryRemoveCurrency(10);
	}
	
	private void BuyingConfirm(bool confirm) 
	{
		if (confirm) 
		{
			// Покупка состоялась
		}
		else 
		{
			// Недостаточно средств
		}
	}
	
	private void ChangeCurrency(int startValue, int endValue)
	{
		_currencyAnimation.Animate(_animator.GetCurrencyText(), startValue, endValue);
	}

	
	private void OnDestroy()
	{
		_interactionSystem.ShowDialogue -= HideTrade;
		_interactionSystem.ShowTrade -= ShowTrade;
		_interactionSystem.EndInteracting -= HideTrade;
		
		_currencyService.OnCurrencyChanged -= ChangeCurrency;
		_currencyService.OnCheckCurrency -= BuyingConfirm;
		_tradeSystem.OnSetInventory -= InstantiateProducts;
	}
}