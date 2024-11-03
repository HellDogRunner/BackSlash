using RedMoonGames.Basics;
using Scripts.Inventory;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

[RequireComponent(typeof(Button))]
public class Product : MonoBehaviour, ISelectHandler, IPointerEnterHandler, IDeselectHandler, IPointerExitHandler
{
    [Header("Components")]
    [SerializeField] private Image _icon;
    [SerializeField] private Image _sold;
    [SerializeField] private Image _frame;
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _price;

    private Sprite _productIcon;
    private string _productName;
    private string _productDescription;
    private int _productPrice;
    private bool _productHave;
    private InventoryModel _productInventoryModel;

    private Button _button;
    private bool _select;

    private TradeAnimator _animator;
    private TradeWindow _tradeWindow;
    private InventoryDatabase _playerInventory;

    [Inject]
    private void Construct(TradeAnimator animator, InventoryDatabase data, TradeWindow tradeWindow)
    {
        _tradeWindow = tradeWindow;
        _playerInventory = data;
        _animator = animator;
    }

    private void Awake()
    {
        _button = GetComponent<Button>();
        if (_select) SelectButton();
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(BuyProduct);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(BuyProduct);
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

    private void SetProductValues(Item product)
    {
        _productName = product.Name;
        _productDescription = product.Description;
        _productIcon = product.Icon;
        _productPrice = product.Price;
    }

    private string GenerateStats() { return ""; }

    public void SetValues(InventoryModel inventoryModel)
    {
        SetProductValues(inventoryModel.Item);

        _icon.sprite = _productIcon;
        _name.text = _productName;
        _productInventoryModel = inventoryModel;

        if (_productHave)
        {
            _sold.gameObject.SetActive(true);
            _price.text = "Sold";
        }
        else
        {
            _sold.gameObject.SetActive(false);
            _price.text = _productPrice.ToString();
        }
    }

    private void BuyProduct()
    {
        TryBuyProduct();
    }

    private TryResult TryBuyProduct()
    {
        if (_productHave)
        {
            _animator.Bought(_frame);
            return TryResult.Fail;
        }

        if (!_tradeWindow.TruBuyProduct(_productPrice))
        {
            _animator.NeedCurrency(_price);
            return TryResult.Fail;
        }
        _productHave = true;
        _animator.Buy(_sold);
        _price.text = "Sold!";
        AddItem(_productInventoryModel.ItemType, _productInventoryModel.Item);
        return TryResult.Successfully;
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

        _tradeWindow.ProductSelected(_productName, _productPrice.ToString(), _productDescription, GenerateStats());

        _animator.Select(_frame, _name);
    }

    private void DeselectButton()
    {
        _animator.Deselect(_frame, _name);
    }
}