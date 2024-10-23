using UnityEngine;
using Zenject;

public class HUDController : MonoBehaviour
{
	[SerializeField] private HUDAnimationService _animationService;
	
	private CurrencyService _currencyService;
	
	[Inject]
	private void Construct(CurrencyService currencyService)
	{
		_currencyService = currencyService;
	}

	private void Awake()
	{
		_currencyService.OnCurrencyChanged += ChangeCurrency;
	}

	private void ChangeCurrency(int value)
	{
		_animationService.CurrencyAnimation(value);
	}

	public void SwitchOverlay(int fade = 0)
	{
		_animationService.SwitchOverlayView(fade);
	}
	
	private void OnDestroy()
	{
		_currencyService.OnCurrencyChanged -= ChangeCurrency;
	}
}