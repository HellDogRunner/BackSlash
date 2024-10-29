using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

[RequireComponent(typeof(Button))]
public class TradeProduct : MonoBehaviour, IPointerEnterHandler
{
	[SerializeField] private Image _icon;
	[SerializeField] private Image _sold;
	[SerializeField] private TMP_Text _name;
	[SerializeField] private TMP_Text _price;
	
	private Button _button;
	private TraderProduct _product;
	
	private InteractionAnimator _animator;
	private TradeWindow _tradeWindow;
	
	[Inject]
	private void Construct(InteractionAnimator animator, TradeWindow tradeWindow)
	{
		_tradeWindow = tradeWindow;
		_animator = animator;	// Create product animations
	}
	
	private void Awake()
	{
		_button = GetComponent<Button>();
		_button.onClick.AddListener(TryBuyProduct);
		
		_icon.sprite = _product.Icon;
		_name.text = _product.Name;
		
		if (_product.Sold)
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
	
	public void OnPointerEnter(PointerEventData eventData)
	{
		if (!EventSystem.current.alreadySelecting) 
		{
			EventSystem.current.SetSelectedGameObject(gameObject);
			_tradeWindow.ProductSelected(_product.Description);
		}
	}
	
	private void TryBuyProduct()
	{		
		if (_product.Sold)
		{
			// продано
			return;
		}
		
		if (!_tradeWindow.TruBuyProduct(_product))
		{
			// не хватает денег
			return;
		}
		
		// покупка совершена
		_price.text = "Sold!";
		_product.Sold = true;
		_sold.gameObject.SetActive(true);
	}
	
	public void SetProduct(TraderProduct product) 
	{
		_product = product;
	}
	
	private void OnDestroy()
	{
		_button.onClick.RemoveListener(TryBuyProduct);
	}
}