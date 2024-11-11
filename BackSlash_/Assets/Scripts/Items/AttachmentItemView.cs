using Scripts.Inventory;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

[RequireComponent(typeof(Button))]
public class AttachmentItemView : MonoBehaviour, ISelectHandler, IPointerEnterHandler, IDeselectHandler, IPointerExitHandler
{
    [Header("Components")]
    [SerializeField] private Image _icon;
    [SerializeField] private Image _frame;
    [SerializeField] private TMP_Text _name;

    private Sprite _productIcon;
    private string _productName;
    private string _productDescription;
    private string _productStats;

    private Button _button;
    private bool _select;

    private ProductAnimator _animator;
    private InventoryModel _productInventoryModel;
    private InventoryDatabase _playerInventory;

    [Inject]
    private void Construct(ProductAnimator animator, InventoryDatabase data)
    {
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
        _button.onClick.AddListener(SelectItem);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(SelectItem);
    }

    private void SetProductValues(Item product)
    {
        _productName = product.Name;
        _productDescription = product.Description;
        _productIcon = product.Icon;
    }

    private string FormatStats(InventoryModel inventoryModel)
    {
        if (inventoryModel.Item is BuffItem)
        {
            var buff = inventoryModel.Item as BuffItem;
            if (buff.Damage > 0)
            {
                _productStats += $"Damage: {buff.Damage}\n";
            }

            if (buff.Resistance > 0)
            {
                _productStats += $"Resistance: {buff.Resistance}\n";
            }

            if (buff.AttackSpeed > 0)
            {
                _productStats += $"Attack Speed: {buff.AttackSpeed}\n";
            }
            return _productStats;
        }

        if (inventoryModel.Item is AttachmentItem)
        {
            var attachment = inventoryModel.Item as AttachmentItem;
            if (attachment.Damage > 0)
            {
                _productStats += $"Damage: {attachment.Damage}\n";
            }

            if (attachment.AttackSpeed > 0)
            {
                _productStats += $"Attack speed: {attachment.AttackSpeed}\n";
            }

            if (attachment.ElementalDamage > 0)
            {
                _productStats += $"Elemental damage: {attachment.ElementalDamage}\n";
            }
            return _productStats;
        }

        return "";
    }

    public void SetValues(InventoryModel inventoryModel)
    {
        SetProductValues(inventoryModel.Item);
        FormatStats(inventoryModel);

        _icon.sprite = _productIcon;
        _name.text = _productName;
        _productInventoryModel = inventoryModel;
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

        _animator.Select();
    }

    private void SelectItem()
    {
        //select item in slot
    }

    private void DeselectButton()
    {
        _animator.Deselect();
    }
}
