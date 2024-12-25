using Scripts.Player;
using UnityEngine;
using Zenject;

namespace RedMoonGames.Window
{
	public class HUDController : MonoBehaviour
	{
		[SerializeField] private HUDAnimationService _animator;

		private PlayerStateController _stateController;
		private CurrencyAnimator _currencyAnimation;
		private CurrencyService _currencyService;
		protected TimeController _time;

		[Inject]
		private void Construct(TimeController time, PlayerStateController stateController, CurrencyAnimator currencyAnimation, CurrencyService currencyService)
		{
			_time = time;
			_stateController = stateController;
			_currencyService = currencyService;
			_currencyAnimation = currencyAnimation;
		}

		private void Awake()
		{
			SetCurrency();
		}

		private void OnEnable()
		{
			_currencyService.OnCurrencyChanged += ChangeCurrency;
			_stateController.OnInteract += Interact;	
			_time.OnPause += Pause;
		}

		private void OnDisable()
		{
			_currencyService.OnCurrencyChanged -= ChangeCurrency;
			_stateController.OnInteract -= Interact;
			_time.OnPause -= Pause;
		}

		private void Pause(bool pause)
		{
			if (pause)_animator.HideHUD();
			else 
			{
				_animator.ShowHUD();
				_animator.ShowOverlay();
				SetCurrency();	
			}
		}

		private void Interact(bool interact)
		{
			if (interact) _animator.HideOverlay();
		}

		private void SetCurrency()
		{
			_animator.SetCurrency(_currencyService.Currency);
		}

		private void ChangeCurrency(int endValue)
		{
			_currencyAnimation.Animate(_animator.GetCurrency(), endValue);
		}
	}
}
