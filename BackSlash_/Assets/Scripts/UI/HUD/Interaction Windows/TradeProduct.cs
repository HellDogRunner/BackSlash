using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(Image))]
public class TradeProduct : MonoBehaviour, IPointerEnterHandler
{
	[SerializeField] private TraderProduct _product;
	
	private Image _image;
	private Button _button;
	
	private TradeWindow _tradeWindow;
	
	[Inject]
	private void Construct(TradeWindow tradeWindow)
	{
		_tradeWindow = tradeWindow;
	}
	
	private void Awake()
	{
		_image = GetComponent<Image>();
		_button = GetComponent<Button>();
		_button.onClick.AddListener(TryBuyProduct);
		
		if (_product.Sold) ChangeIcon(_tradeWindow.GetSoldSprite());
	}
	
	public void OnPointerEnter(PointerEventData eventData)
	{
		if (!EventSystem.current.alreadySelecting) 
		{
			EventSystem.current.SetSelectedGameObject(gameObject);
			_tradeWindow.ProductSelected(_product.Description);
		}
	}
	
	private void ChangeIcon(Sprite sprite) 
	{
		_image.sprite = sprite;
	}
	
	private void TryBuyProduct()
	{
		if (!_product.Sold && _tradeWindow.TruBuyProduct(_product))
		{
			_product.Sold = true;
			ChangeIcon(_tradeWindow.GetSoldSprite());
		}
	}
	
	private void OnDestroy()
	{
		_button.onClick.RemoveListener(TryBuyProduct);
	}
}