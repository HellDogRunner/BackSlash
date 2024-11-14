using RedMoonGames.Basics;
using Scripts.Inventory;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

[RequireComponent(typeof(Button))]
public class Product : MonoBehaviour, ISelectHandler, IPointerEnterHandler, IDeselectHandler
{
	[SerializeField] private ProductAnimator _animator;

	private Button _button;
	private bool _select;

	private Item _item;
	private EItemType _type;
	private InventoryDatabase _playerInventory;
	private CurrencyService _currencyService;

	public event Action<Item, Product> ProductSelected;

	[Inject]
	private void Construct(InventoryDatabase data, CurrencyService currencyService)
	{
		_playerInventory = data;
		_currencyService = currencyService;
	}

	private void Awake()
	{
		_button = GetComponent<Button>();
	}

	public void OnPointerEnter(PointerEventData eventData) { SelectButton(); }
	public void OnSelect(BaseEventData eventData) { SelectButton(); }
	public void OnDeselect(BaseEventData eventData) { _animator.Deselect(); }

	private void OnEnable()
	{
		_button.onClick.AddListener(TryBuyProduct);
	}

	private void OnDisable()
	{
		_button.onClick.RemoveListener(TryBuyProduct);
	}

	public void SetValues(InventoryModel inventoryModel, bool bought)
	{
		_type = inventoryModel.Type;
		_item = inventoryModel.Item;

		_item.SetValues();
		_animator.SetView(_item.Icon, bought);
	}

	public void TryBuyProduct()
	{
		if (_playerInventory.GetItemTypeModel(_item) != null)
		{
			_animator.Bought();
			return;
		}

		if (_currencyService.TryRemoveCurrency(_item.Price) == TryResult.Fail)
		{
			_animator.NeedCurrency();
			return;
		}

		_animator.Buy();
		AddItem(_type, _item);
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

	public void SelectButton()
	{
		if (!EventSystem.current.alreadySelecting) EventSystem.current.SetSelectedGameObject(gameObject);

		ProductSelected?.Invoke(_item, this);
		_animator.Select();
	}
}