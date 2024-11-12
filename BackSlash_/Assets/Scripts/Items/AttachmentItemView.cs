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

	private Button _button;
	private bool _select;

	private Item _item;
	private EItemType _type;
	
	private ProductAnimator _animator;
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


	public void SetValues(InventoryModel inventoryModel)
	{
		_item = inventoryModel.Item;
		_type = inventoryModel.Type;

		_icon.sprite = _item.Icon;	// Set mod values
		_name.text = _item.Name;
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
