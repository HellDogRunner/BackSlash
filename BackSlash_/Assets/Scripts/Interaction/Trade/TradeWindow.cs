using RedMoonGames.Basics;
using Scripts.Inventory;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace RedMoonGames.Window
{
	public class TradeWindow : BasicInteractionWindow
	{
		[SerializeField] private WindowHandler _dialogueWindow;
		[Space]
		[SerializeField] private GameObject _productPrefab;
		[SerializeField] private GameObject _noProductsItem;
		
		[Header("Roots")]
		[SerializeField] private RectTransform _buffsRoot;
		[SerializeField] private RectTransform _modsRoot;

		[Header("Description Texts")]
		[SerializeField] private TMP_Text _price;
		[SerializeField] private TMP_Text _name;
		[SerializeField] private TMP_Text _stats;
		[SerializeField] private TMP_Text _description;
		[Space]
		[SerializeField] private TMP_Text _currency;

		[Header("Buttons")]

		[SerializeField] private Button _buyButton;
		[SerializeField] private Button _tradeButton;
		[SerializeField] private Button _leaveButton;
		[Space]
		[SerializeField] private float _showDelay = 0.1f;

		private List<Product> _products = new List<Product>();
		private Product _currentProduct;
		
		private InventoryDatabase _playerInventory;
		private TradeSystem _tradeSystem;
		private CurrencyAnimator _currencyAnimation;
		private CurrencyService _currencyService;
		private DiContainer _diContainer;

		[Inject]
		private void Construct(TradeSystem tradeSystem, InventoryDatabase playerInventory, DiContainer diContainer, CurrencyService currencyService, CurrencyAnimator currencyAnimation)
		{
			_tradeSystem = tradeSystem;
			_playerInventory = playerInventory;
			_diContainer = diContainer;
			_currencyService = currencyService;
			_currencyAnimation = currencyAnimation;
		}

		private void Awake()
		{;
			SetItems();
			SetCurrency();
			Show(false, _showDelay);
		}

		protected override void OnEnable()
		{	
			base.OnEnable();
			
			_uiInputs.OnBackKeyPressed += HideWindow;
			_uiInputs.OnTradeKeyPressed += TradeButton;

			_currencyService.OnCurrencyChanged += ChangeCurrency;
			
			_buyButton.onClick.AddListener(BuyButton);
			_tradeButton.onClick.AddListener(TradeButton);
			_leaveButton.onClick.AddListener(HideWindow);
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			
			_uiInputs.OnBackKeyPressed -= HideWindow;
			_uiInputs.OnTradeKeyPressed -= TradeButton;

			_currencyService.OnCurrencyChanged -= ChangeCurrency;

			_buyButton.onClick.RemoveListener(BuyButton);
			_tradeButton.onClick.RemoveListener(TradeButton);
			_leaveButton.onClick.RemoveListener(HideWindow);
		}

		private void TradeButton() { OpenWindow(_dialogueWindow); }
		
		private void BuyButton()
		{
			_currentProduct.TryBuyProduct();
			_currentProduct.SelectButton();
		}
	
		private void SetCurrency()
		{
			_currency.text = _currencyService.Currency.ToString();
		}

		private void ChangeCurrency(int endValue)
		{
			_currencyAnimation.Animate(_currency, endValue);
		}
		
		private void SetItems()
		{
			foreach (var item in _tradeSystem.GetItems())
			{
				bool bought = _playerInventory.GetModelByItem(item.Item) is not null ? true : false;
				
				var product = _diContainer.InstantiatePrefabForComponent<Product>(_productPrefab, GetRoot(item.Type));

				product.ProductSelected += ProductSelected;
				product.SetValues(item, bought);
				if (_products.Count == 0) product.SelectButton();
				_products.Add(product);
			}
			
			FillEmptyRoots();
		}

		private void ResetItems()
		{
			foreach (var product in _products)
			{
				product.ProductSelected -= ProductSelected;
				Destroy(product);
			}
			
			_products.Clear();
		}

		private RectTransform GetRoot(EItemType type)
		{	
			switch (type)
			{
				case EItemType.Buff:
				return _buffsRoot;
				
				case EItemType.Blade:
				return _modsRoot;
				
				case EItemType.Guard:
				return _modsRoot;
				
				case EItemType.Hilt:
				return _modsRoot;
			}
			return null;
		}

		private void FillEmptyRoots()
		{
			if (_buffsRoot.transform.childCount == 0) Instantiate(_noProductsItem, _buffsRoot);
			if (_modsRoot.transform.childCount == 0) Instantiate(_noProductsItem, _modsRoot);
		}

		public bool TruBuyProduct(int price)
		{
			var result = _currencyService.TryRemoveCurrency(price);

			if (result == TryResult.Successfully)
			{
				return true;
			}
			return false;
		}

		public void ProductSelected(Item item, Product product)
		{
			_currentProduct = product;
			_name.text = item.Name;
			_price.text = string.Format("Price: {0}", item.Price);
			_description.text = string.Format("DESCRIPTION\n{0}", item.Description);
			_stats.text = string.Format("STATS\n{0}", item.Stats);
		}
		
		private void OnDestroy()
		{
			ResetItems();
		}
	}
}
