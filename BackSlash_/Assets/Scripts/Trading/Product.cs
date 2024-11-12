using RedMoonGames.Basics;
using Scripts.Inventory;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

[RequireComponent(typeof(Button))]
public class Product : MonoBehaviour, ISelectHandler, IPointerEnterHandler, IDeselectHandler, IPointerExitHandler
{
	[SerializeField] private ProductAnimator _animator;

	private Button _button;
	private bool _select;

	private Item _item;
	private EItemType _type;
	private InventoryDatabase _playerInventory;
	private CurrencyService _currencyService;

	public event Action<Item> ProductSelected;
	public event Action<Product> OnBoughtProduct;

	[Inject]
	private void Construct(InventoryDatabase data, CurrencyService currencyService)
	{
		_playerInventory = data;
		_currencyService = currencyService;
	}

	private void Awake()
	{
		_button = GetComponent<Button>();
		if (_select) SelectButton();
	}

	private void OnEnable()
	{
		_button.onClick.AddListener(BuyProduct);
		_animator.OnBuyComplete += ProductBought;
	}

	private void OnDisable()
	{
		_button.onClick.RemoveListener(BuyProduct);
		_animator.OnBuyComplete -= ProductBought;
	}

	private void AddItem(EItemType itemType, Item item)
	{
		var newInvenotryModel = new InventoryModel()
		{
			Type = itemType,
			Item = item,
		};
		_playerInventory.AddItem(newInvenotryModel);
	}

	public void SetValues(InventoryModel inventoryModel)
	{
		_type = inventoryModel.Type;
		_item = inventoryModel.Item;
		_item.GenerateStats();

		_animator.SetView(_item.Icon);
	}

	private void BuyProduct()
	{
		TryBuyProduct();
	}

	private TryResult TryBuyProduct()
	{
		if (_playerInventory.GetItemTypeModel(_item) != null)
		{
			_animator.Bought();
			return TryResult.Fail;
		}

		if (_currencyService.TryRemoveCurrency(_item.Price) == TryResult.Fail)
		{
			_animator.NeedCurrency();
			return TryResult.Fail;
		}

		_animator.Buy();
		AddItem(_type, _item);
		return TryResult.Successfully;
	}

	private void ProductBought()
	{
		OnBoughtProduct?.Invoke(this);
	}

	public void OnPointerEnter(PointerEventData eventData) { SelectButton(); }
	public void OnPointerExit(PointerEventData eventData) { DeselectButton(); }
	public void OnSelect(BaseEventData eventData) { SelectButton(); }
	public void OnDeselect(BaseEventData eventData) { DeselectButton(); }

	public void SelectFirst()
	{
		_select = true;
	}

	public void SelectButton()
	{
		if (!EventSystem.current.alreadySelecting) EventSystem.current.SetSelectedGameObject(gameObject);

		ProductSelected?.Invoke(_item);
		_animator.Select();
	}

	private void DeselectButton()
	{
		_animator.Deselect();
	}
}