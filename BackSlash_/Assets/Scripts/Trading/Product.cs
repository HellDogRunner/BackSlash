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
	private string _stats;
	private bool _select;

	private InventoryModel _inventoryModel;
	private Item _item;
	private InventoryDatabase _playerInventory;
	private CurrencyService _currencyService;

	public event Action<Item, string> ProductSelected;
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
			ItemType = itemType,
			Item = item,
		};
		_playerInventory.AddItem(newInvenotryModel);
	}

	private string FormatStats(InventoryModel inventoryModel)
	{
		if (inventoryModel.ItemType == EItemType.BuffItem && inventoryModel.Item is BuffItem)
		{
			var buff = inventoryModel.Item as BuffItem;
			if (buff.Damage > 0)
			{
				_stats += $"Damage: {buff.Damage}\n";
			}

			if (buff.Resistance > 0)
			{
				_stats += $"Resistance: {buff.Resistance}\n";
			}

			if (buff.AttackSpeed > 0)
			{
				_stats += $"Attack Speed: {buff.AttackSpeed}\n";
			}
			return _stats;
		}

		if (inventoryModel.ItemType == EItemType.Blade && inventoryModel.Item is AttachmentItem)
		{
			var attachment = inventoryModel.Item as AttachmentItem;
			if (attachment.Damage > 0)
			{
				_stats += $"Damage: {attachment.Damage}\n";
			}

			if (attachment.AttackSpeed > 0)
			{
				_stats += $"Attack speed: {attachment.AttackSpeed}\n";
			}

			if (attachment.ElementalDamage > 0)
			{
				_stats += $"Elemental damage: {attachment.ElementalDamage}\n";
			}
			return _stats;
		}

		return "";
	}

	public void SetValues(InventoryModel inventoryModel)
	{
		FormatStats(inventoryModel);
		_inventoryModel = inventoryModel;
		_item = inventoryModel.Item;

		_animator.SetView(_item.Icon);
	}

	private void BuyProduct()
	{
		TryBuyProduct();
	}

	private TryResult TryBuyProduct()
	{
		if (_playerInventory.GetItemTypeModel(_inventoryModel.Item) != null)
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
		AddItem(_inventoryModel.ItemType, _inventoryModel.Item);
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

		ProductSelected?.Invoke(_item, _stats);
		_animator.Select();
	}

	private void DeselectButton()
	{
		_animator.Deselect();
	}
}