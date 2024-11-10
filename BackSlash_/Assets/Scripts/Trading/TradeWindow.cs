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
        [SerializeField] private RectTransform _productsRoot;
        [SerializeField] private GameObject _productPrefab;
		[SerializeField] private WindowHandler _dialogueWindow;

        [Header("Description Texts")]
        [SerializeField] private TMP_Text _price;
        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _stats;
        [SerializeField] private TMP_Text _description;

        [Header("Buttons")]
        [SerializeField] private Button _leaveButton;
        [SerializeField] private Button _buyButton;
        [SerializeField] private Button _tradeButton;

        private List<Product> _products = new List<Product>();
        private InventoryDatabase _traderInventory;
        private InventoryDatabase _playerInventory;

        private InteractionSystem _interactionSystem;
        private CurrencyAnimation _currencyAnimation;
        private CurrencyService _currencyService;
        private InteractionAnimator _interactionAnimator;
        private DiContainer _diContainer;

        [Inject]
        private void Construct(
            DiContainer diContainer,
            InteractionSystem interactionSystem,
            CurrencyService currencyService,
            CurrencyAnimation currencyAnimation,
            InteractionAnimator interactionAnimator
            )
        {
            _diContainer = diContainer;
            _interactionSystem = interactionSystem;
            _currencyService = currencyService;
            _currencyAnimation = currencyAnimation;
            _interactionAnimator = interactionAnimator;
        }

        private void Awake()
        {
            _currencyService.OnCurrencyChanged += ChangeCurrency;

            _leaveButton.onClick.AddListener(LeaveButton);
            _buyButton.onClick.AddListener(BuyButton);
            _tradeButton.onClick.AddListener(TradeButton);

            SetInventories();
        }

        private void OnDestroy()
        {
            ResetItems();

            _currencyService.OnCurrencyChanged -= ChangeCurrency;

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

            _interactionSystem.OpenWindow();
        }
		private void SetCurrency()
		}
		{
			var value = _currencyService.Currency;
			_currencyAnimation.SetCurrency(target, value);
			
			var target = _currency;

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
            foreach (var item in _traderInventory.GetData())
            {
                var isPlayerHaveItem = _playerInventory.GetItemTypeModel(item.Item);

                if (isPlayerHaveItem == null)
                {
                    var product = _diContainer.InstantiatePrefabForComponent<Product>(_productPrefab, _productsRoot);

                    product.ProductSelected += ProductSelected;
                    product.SetValues(item);

                    if (_products.Count == 0) product.SelectFirst();

                    _products.Add(product);
                }
            }
        }

        private void ResetItems()
        {
            foreach (var product in _products)
            {
                product.ProductSelected -= ProductSelected;
                Destroy(product);
            }

            _traderInventory = null;
            _products.Clear();
        }

        private void SetCurrency()
        {
            var value = _currencyService.Currency;
            var target = _interactionAnimator.GetCurrency();

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
            _currencyAnimation.Animate(_interactionAnimator.GetCurrency(), endValue);
        }

        public void ProductSelected(Product product)
        {
            _name.text = product.ProductName;
            _price.text = string.Format("Price: {0}", product.ProductPrice);
            _description.text = string.Format("DESCRIPTION\n{0}", product.ProductDescription);
            _stats.text = string.Format("STATS\n{0}", product.ProductStats);
        }
    }
}