using Scripts.Inventory;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

[RequireComponent(typeof(Button))]
public class BasicProduct : MonoBehaviour, ISelectHandler, IPointerEnterHandler, IDeselectHandler, IPointerExitHandler
{
	[Header("Components")]
	[SerializeField] protected Image _icon;
	[SerializeField] protected Image _sold;
	[SerializeField] protected Image _frame;
	[SerializeField] protected TMP_Text _name;
	[SerializeField] protected TMP_Text _price;
	
	protected Button _button;
	protected bool select;
	
	protected TradeAnimator _animator;
	protected TradeWindow _tradeWindow;
	protected InventoryDatabase _playerInventory;
	
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
		_button.onClick.AddListener(TryBuyProduct);
		
		if (select) SelectButton();
		
		SetValues();
	}
	
	public void OnPointerEnter(PointerEventData eventData) { SelectButton(); }
	public void OnPointerExit(PointerEventData eventData) { DeselectButton(); }
	public void OnSelect(BaseEventData eventData) { SelectButton(); }
	public void OnDeselect(BaseEventData eventData) { DeselectButton(); }
	
	protected virtual void AddItem() {}
	
	protected virtual (string, string, int, bool, Sprite) GetValues()
	{  
		return (null, null, 0, false, null);
	}
	
	public void SetValues()
	{
		var product = GetValues();
		
		_icon.sprite = product.Item5;
		_name.text = product.Item1;
		
		if (product.Item4)
		{
			_sold.gameObject.SetActive(true);
			_price.text = "Sold";
		}
		else
		{
			_sold.gameObject.SetActive(false);
			_price.text = product.Item3.ToString();
		}
	}
	
	protected void TryBuyProduct()
	{
		var _product = GetValues();
		
		if (_product.Item4)
		{
			_animator.Bought(_frame);
			return;
		}
		
		if (!_tradeWindow.TruBuyProduct(_product.Item3))
		{
			_animator.NeedCurrency(_price);
			return;
		}
		
		AddItem();
		_animator.Buy(_sold);
		_price.text = "Sold!";
	}
	
	public void SelectFirst()
	{
		select = true;
	}
	
	public void SelectButton()
	{
		if (!EventSystem.current.alreadySelecting) EventSystem.current.SetSelectedGameObject(gameObject);
		
		_tradeWindow.ProductSelected(GetValues().Item2);
		
		_animator.Select(_frame, _name);
	}
	
	protected void DeselectButton()
	{
		_animator.Deselect(_frame, _name);
	}
	
	protected void OnDestroy()
	{
		_button.onClick.RemoveListener(TryBuyProduct);
	}
}