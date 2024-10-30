using DG.Tweening;
using Scripts.Inventory;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

[RequireComponent(typeof(Button))]
public class TradeProduct : MonoBehaviour, ISelectHandler, IPointerEnterHandler, IDeselectHandler, IPointerExitHandler
{
	[Header("Components")]
	[SerializeField] private Image _icon;
	[SerializeField] private Image _sold;
	[SerializeField] private Image _frame;
	[SerializeField] private TMP_Text _name;
	[SerializeField] private TMP_Text _price;
	
	[Header("Animation")]
	[SerializeField] private float _duration;
	[SerializeField] private Color _defaultTextColor;
	[SerializeField] private Color _defaultFrameColor;
	[SerializeField] private Color _selectedColor;
	
	private Button _button;
	private WeaponBladeTypeModel _product;
	
	private InteractionAnimator _animator;
	private TradeWindow _tradeWindow;
	
	[Inject]
	private void Construct(InteractionAnimator animator, TradeWindow tradeWindow)
	{
		_tradeWindow = tradeWindow;
		_animator = animator;	// Create product animations
	}
	
	public void OnPointerEnter(PointerEventData eventData) { SelectButton(); }
	public void OnPointerExit(PointerEventData eventData) { DeselectButton(); }
	public void OnSelect(BaseEventData eventData) { SelectButton(); }
	public void OnDeselect(BaseEventData eventData) { DeselectButton(); }
	
	private void Awake()
	{
		_button = GetComponent<Button>();
		_button.onClick.AddListener(TryBuyProduct);
		
		_icon.sprite = _product.Icon;
		_name.text = _product.Name;
		
		if (_product.Have)
		{
			_sold.gameObject.SetActive(true);
			_price.text = "Sold";
		}
		else
		{
			_sold.gameObject.SetActive(false);
			_price.text = _product.Price.ToString();
		}
	}
	
	public void SelectButton()
	{
		if (!EventSystem.current.alreadySelecting) EventSystem.current.SetSelectedGameObject(gameObject);
		
		_tradeWindow.ProductSelected(_product.Description);
		
		_frame.DOColor(_selectedColor, _duration);
		_name.DOColor(_selectedColor, _duration);
	}
	
	private void DeselectButton()
	{
		_frame.DOColor(_defaultFrameColor, _duration);
		_name.DOColor(_defaultTextColor, _duration);
	}
	
	private void TryBuyProduct()
	{		
		if (_product.Have)
		{
			// уже есть
			return;
		}
		
		if (!_tradeWindow.TruBuyProduct(_product.Price))
		{
			// не хватает денег
			return;
		}
		
		// покупка совершена
		
		//_tradeSystem.AddItem(_product);
		// Добавление предмета в инвентарь
		
		
		_price.text = "Sold!";
		_product.Have = true;
		_sold.gameObject.SetActive(true);
	}
	
	public void SetProduct(WeaponBladeTypeModel product) 
	{
		_product = product;
	}
	
	private void OnDestroy()
	{
		_button.onClick.RemoveListener(TryBuyProduct);
	}
}