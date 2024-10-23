using Scripts.Player;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class TradeWindow : MonoBehaviour
{
	[SerializeField] private TradeAnimation _animation;
	
	[Header("Products")]
	[SerializeField] private TraderProductScriptable _product;
	
	[Header("Buttons")]
	[SerializeField] private Button _button;
	
	private InteractionSystem _interactionSystem;
	private TradeSystem _tradeSystem;
	private UIActionsController _uiInputs;
	
	[Inject]
	private void Construct(UIActionsController uiInputs, InteractionSystem interactionSystem, TradeSystem tradeSystem)
	{
		_interactionSystem = interactionSystem;
		_tradeSystem = tradeSystem;
		_uiInputs = uiInputs;
	}
	
	private void Awake()
	{
		_uiInputs.OnBackKeyPressed += HideWindow;
		_uiInputs.OnTradeKeyPressed += OpenWindow;
		_interactionSystem.OnOpenTradeWindow += OpenWindow;
		
		_button.onClick.AddListener(TruBuyProduct);
	}
	
	private void OpenWindow()
	{
		_animation.ShowWindow();
	}
	
	private void HideWindow()
	{
		_animation.HideWindow();
	}
	
	private void TruBuyProduct() 
	{
		_tradeSystem.TryBuy(_product.Price);
	}
	
	private void OnDestroy()
	{
		_uiInputs.OnBackKeyPressed -= HideWindow;
		_uiInputs.OnTradeKeyPressed -= OpenWindow;
		_interactionSystem.OnOpenTradeWindow -= OpenWindow;
		
		_button.onClick.RemoveListener(TruBuyProduct);
	}
}