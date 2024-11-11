using RedMoonGames.Basics;
using Scripts.Inventory;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace RedMoonGames.Window
{
	public class TradeWindow : GameBasicWindow
	{
		[SerializeField] private WindowHandler _dialogueWindow;
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
		[SerializeField] private Button _leaveButton;
		[SerializeField] private Button _buyButton;
		[SerializeField] private Button _tradeButton;
		[Space]
		[SerializeField] private float _showWindowDelay = 0.02f;

		private List<Product> _products = new List<Product>();
		private InventoryDatabase _traderInventory;
		private InventoryDatabase _playerInventory;

		private InteractionSystem _interactionSystem;
		private CurrencyAnimation _currencyAnimation;
		private CurrencyService _currencyService;
		private DiContainer _diContainer;

		[Inject]
		private void Construct(
			DiContainer diContainer,
			InteractionSystem interactionSystem,
			CurrencyService currencyService,
			CurrencyAnimation currencyAnimation
			)
		{
			_diContainer = diContainer;
			_interactionSystem = interactionSystem;
			_currencyService = currencyService;
			_currencyAnimation = currencyAnimation;
		}

		private void Awake()
		{
			_currencyService.OnCurrencyChanged += ChangeCurrency;
			_interactionSystem.OnButtonClick += TradeButton;

			_leaveButton.onClick.AddListener(LeaveButton);
			_buyButton.onClick.AddListener(BuyButton);
			_tradeButton.onClick.AddListener(TradeButton);

			SetInventories();
			SetCurrency();
		}

		private void OnDestroy()
		{
			ResetItems();

			_windowService.OnHideWindow -= DisablePause;
			_currencyService.OnCurrencyChanged -= ChangeCurrency;
			_interactionSystem.OnButtonClick -= TradeButton;

			_leaveButton.onClick.RemoveListener(LeaveButton);
			_buyButton.onClick.RemoveListener(BuyButton);
			_tradeButton.onClick.RemoveListener(TradeButton);
		}

		private void LeaveButton()
		{
			_interactionSystem.TryStopInteract();
		}

		private void BuyButton()
		{
			Debug.Log("BuyButton");

			// Покупка выбранного
		}

		private void TradeButton()
		{
			_interactionSystem.SwitchWindows(_dialogueWindow);
		}
		
		private void SetCurrency()
		{
			var value = _currencyService.Currency;
			
			_currencyAnimation.SetCurrency(_currency, value);
		}

		private void SetInventories()
		{
			_traderInventory = _interactionSystem.GetTraderInventory();
			_playerInventory = _interactionSystem.GetPlayerInventory();

			SetItems();
		}

		private void ChangeCurrency(int endValue)
		{
			_currencyAnimation.Animate(_currency, endValue);
		}
		
		private void SetItems()
		{
			_animationService.ShowInteractionWindow(_canvasGroup, _showWindowDelay);
			
			foreach (var item in _traderInventory.GetData())
			{
				if (_playerInventory.GetItemTypeModel(item.Item) != null) continue;

				var product = _diContainer.InstantiatePrefabForComponent<Product>(_productPrefab, GetRoot(item.ItemType));

				product.ProductSelected += ProductSelected;
				product.OnBoughtProduct += RemoveItem;
				product.SetValues(item);

				if (_products.Count == 0) product.SelectFirst();

				_products.Add(product);

			}
			
			FillEmptyRoots();
		}

		private void ResetItems()
		{
			foreach (var product in _products)
			{
				product.ProductSelected -= ProductSelected;
				product.OnBoughtProduct -= RemoveItem;
				Destroy(product);
			}

			_traderInventory = null;
			_products.Clear();
		}

		private void RemoveItem(Product prod)
		{
			foreach (var product in _products)
			{
				if (product == prod)
				{
					product.OnBoughtProduct -= RemoveItem;
					product.gameObject.SetActive(false);
					//Destroy(product);
					return;
				}
			}
		}

		private RectTransform GetRoot(EItemType type)
		{	
			switch (type)
			{
				case EItemType.BuffItem:
				return _buffsRoot;
				
				case EItemType.Blade:
				return _modsRoot;
				
				case EItemType.Guard:
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

		public void ProductSelected(Item item, string stats)
		{
			_name.text = item.Name;
			_price.text = string.Format("Price: {0}", item.Price);
			_description.text = string.Format("DESCRIPTION\n{0}", item.Description);
			_stats.text = string.Format("STATS\n{0}", stats);
		}
	}
}