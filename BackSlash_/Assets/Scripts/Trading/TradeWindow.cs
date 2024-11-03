using RedMoonGames.Basics;
using Scripts.Inventory;
using TMPro;
using UnityEngine;
using Zenject;

public class TradeWindow : MonoBehaviour
{
	[SerializeField] private TradeProductSetter _setter;
	
	[Header("Description Texts")]
	[SerializeField] private TMP_Text _price;
	[SerializeField] private TMP_Text _name;
	[SerializeField] private TMP_Text _stats;
	[SerializeField] private TMP_Text _description;
	[Space]
	[SerializeField] private GameObject _buyButton;
	
	[Header("Trade Button Texts")]
	[SerializeField] private GameObject _showTrade;
	[SerializeField] private GameObject _hideTrade;
	
	private bool _windowVisible;
	
	private InteractionSystem _interactionSystem;
	private InteractionAnimator _animator;
	private CurrencyAnimation _currencyAnimation;
	private CurrencyService _currencyService;
	
	[Inject]
	private void Construct(InteractionAnimator animator, CurrencyService currencyService, CurrencyAnimation currencyAnimation, InteractionSystem interactionSystem)
	{
		_currencyService = currencyService;
		_interactionSystem = interactionSystem;
		_currencyAnimation = currencyAnimation;
		_animator = animator;
	}
	
	private void Awake()
	{
		_interactionSystem.ShowDialogue += HideTrade;
		_interactionSystem.ShowTrade += ShowTrade;
		_interactionSystem.EndInteracting += HideTrade;
		_interactionSystem.SetTradeInventories += SetInventory;
		_interactionSystem.OnExitTrigger += ResetInventory;
		
		_currencyService.OnCurrencyChanged += ChangeCurrency;
		
		SwitchTradeText(true);
		_buyButton.SetActive(false);
	}

    private void OnDisable()
    {
        _interactionSystem.ShowDialogue -= HideTrade;
        _interactionSystem.ShowTrade -= ShowTrade;
        _interactionSystem.EndInteracting -= HideTrade;
        _interactionSystem.SetTradeInventories -= SetInventory;
        _interactionSystem.OnExitTrigger -= ResetInventory;

        _currencyService.OnCurrencyChanged -= ChangeCurrency;
    }

    private void SetInventory(InventoryDatabase traderInventory, InventoryDatabase playerInventory) 
	{
		_setter.SetTradeInventory(traderInventory, playerInventory);
	}
	
	private void SwitchTradeText(bool enable)
	{
		_showTrade.SetActive(enable);
		_hideTrade.SetActive(!enable);
	}
	
	private void ResetInventory()
	{
		_setter.ResetInventory();
	}
	
	private void SetCurrency() 
	{
		var value = _currencyService.Currency;
		var target = _animator.GetCurrency();
		
		_windowVisible = true;
		_currencyAnimation.SetCurrency(target, value);
	}
	
	private void ShowTrade()
	{
		SwitchTradeText(false);
		_animator.Trade(1);
		_buyButton.SetActive(true);
		
		SetCurrency();
	}
	
	private void HideTrade()
	{
		_windowVisible = false;
		
		SwitchTradeText(true);
		_animator.Trade();
		_buyButton.SetActive(false);
	}
	
	public bool TruBuyProduct(int price) 
	{
        var result = _currencyService.TryRemoveCurrency(price);

        if (result == TryResult.Successfully)
		{
			return true;
		}
		return false;
	}

	private void ChangeCurrency(int endValue)
	{
		if (_windowVisible) _currencyAnimation.Animate(_animator.GetCurrency(), endValue);
	}

	public void ProductSelected(string name, string price, string description, string stats) 
	{
		_name.text = name;
		_price.text = string.Format("Price: {0}", price);
		_stats.text = string.Format("STATS\n{0}", stats);
		_description.text = string.Format("DESCRIPTION\n{0}", description);
	}
}