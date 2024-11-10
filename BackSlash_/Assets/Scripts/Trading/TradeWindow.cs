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
		[SerializeField] private TradeAnimator _tradeAnimator;
		[Space]
		[SerializeField] private RectTransform _productsRoot;
		[SerializeField] private GameObject _productPrefab;
		
		[Header("Description Texts")]
		[SerializeField] private TMP_Text _price;
		[SerializeField] private TMP_Text _name;
		[SerializeField] private TMP_Text _stats;
		[SerializeField] private TMP_Text _description;
		[SerializeField] private TMP_Text _currency;

		[Header("Buttons")]
		[SerializeField] private Button _leaveButton;
		[SerializeField] private Button _buyButton;
		[SerializeField] private Button _tradeButton;

		private List<GameObject> _products = new List<GameObject>();
		private InventoryDatabase _traderInventory;
		private InventoryDatabase _playerInventory;
		private TradeWindow _tradeWindowSC;
		
		private InteractionSystem _interactionSystem;
		private CurrencyAnimation _currencyAnimation;
		private CurrencyService _currencyService;
		private DiContainer _diContainer;

		[Inject]
		private void Construct(DiContainer diContainer, CurrencyService currencyService, CurrencyAnimation currencyAnimation, InteractionSystem interactionSystem)
		{
			_interactionSystem = interactionSystem;
			_currencyAnimation = currencyAnimation;
			_currencyService = currencyService;
			_diContainer = diContainer;
		}

		private void Awake()
		{
			_tradeWindowSC = GetComponent<TradeWindow>();
			
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
			Debug.Log("LeaveButton");
			
			_interactionSystem.TryStopInteract();
		}

		private void BuyButton()
		{
			Debug.Log("BuyButton");
			
			// Покупка выбранного
		}
		
		private void TradeButton()
		{
			Debug.Log("TradeButton");
			
			_interactionSystem.SwitchWindows(_dialogueWindow);
		}

		private void SetInventories()
		{
			_traderInventory = _interactionSystem.GetTraderInventory();
			_playerInventory = _interactionSystem.GetPlayerInventory();
			
			SetItems();
		}

		private void SetItems()
		{
			foreach (var item in _traderInventory.GetData())
			{
				var isPlayerHaveItem = _playerInventory.GetItemTypeModel(item.Item);

				if (isPlayerHaveItem == null)
				{
					var prefab = _diContainer.InstantiatePrefab(_productPrefab, _productsRoot);
					var product = prefab.GetComponent<Product>();

					product.SetComponents(_tradeWindowSC, _playerInventory, _tradeAnimator);
					product.SetValues(item);

					if (_products.Count == 0) product.SelectFirst();

					_products.Add(prefab);
				}
			}
		}

		private void ResetItems()
		{
			foreach (var product in _products)
			{
				Destroy(product);
			}

			_traderInventory = null;
			_products.Clear();
		}

		private void SetCurrency()
		{
			var value = _currencyService.Currency;
			var target = _currency;
			
			_currencyAnimation.SetCurrency(target, value);
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

		private void ChangeCurrency(int endValue)
		{
			_currencyAnimation.Animate(_currency, endValue);
		}

		public void ProductSelected(string name, string price, string description, string stats)
		{
			_name.text = name;
			_price.text = string.Format("Price: {0}", price);
			_description.text = string.Format("DESCRIPTION\n{0}", description);
			_stats.text = string.Format("STATS\n{0}", stats);
		}
	}
}