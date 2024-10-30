using UnityEngine;
using Zenject;

public class HUDController : MonoBehaviour
{
	[SerializeField] private HUDAnimationService _animationService;
	
	private CurrencyService _currencyService;
	private CurrencyAnimation _currencyAnimation;
	private InteractionSystem _interactionSystem;
	
	[Inject]
	private void Construct(InteractionSystem interactionSystem, CurrencyAnimation currencyAnimation, CurrencyService currencyService)
	{
		_currencyService = currencyService;
		_currencyAnimation = currencyAnimation;
		_interactionSystem = interactionSystem;
	}
	
	private void Awake()
	{
		_currencyService.OnCurrencyChanged += ChangeCurrency;
		_interactionSystem.EndInteracting += SetCurrency;
		
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
	
	private void ChangeCurrency(int endValue)
	{
		_currencyAnimation.Animate(_animationService.GetCurrency(), endValue);
	}
	
	private void OnDestroy()
	{
		_currencyService.OnCurrencyChanged -= ChangeCurrency;
		_interactionSystem.EndInteracting -= SetCurrency;
	}
}