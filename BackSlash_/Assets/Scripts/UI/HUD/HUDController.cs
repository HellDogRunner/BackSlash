using UnityEngine;
using Zenject;

public class HUDController : MonoBehaviour
{
	[SerializeField] private HUDAnimationService _animationService;
	
	private CurrencyService _currencyService;
	private CurrencyAnimation _currencyAnimation;
	
	[Inject]
	private void Construct(CurrencyAnimation currencyAnimation, CurrencyService currencyService)
	{
		_currencyService = currencyService;
		_currencyAnimation = currencyAnimation;
	}
	
	private void Awake()
	{
		_currencyService.OnCurrencyChanged += ChangeCurrency;
		
		SetCurrency();
	}
	
	public void SwitchOverlay(int fade = 0)
	{
		_animationService.SwitchOverlayView(fade);
	}
	
	private void SetCurrency()
	{
		var value = _currencyService.GetCurrentCurrency();
		var target = _animationService.GetCurrency();
		
		_currencyAnimation.SetCurrency(target, value);
	}
	
	private void ChangeCurrency(int startValue, int endValue)
	{
		_currencyAnimation.Animate(_animationService.GetCurrency(), startValue, endValue);
	}
	
	private void OnDestroy()
	{
		_currencyService.OnCurrencyChanged -= ChangeCurrency;
	}
}