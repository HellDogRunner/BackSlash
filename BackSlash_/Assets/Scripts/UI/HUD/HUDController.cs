using UnityEngine;
using Zenject;

namespace RedMoonGames.Window
{
	public class HUDController : MonoBehaviour
	{
		[SerializeField] private HUDAnimationService _animator;

		private PlayerStateMachine _playerState;
		private CurrencyService _currencyService;
		private CurrencyAnimator _currencyAnimation;

		[Inject]
		private void Construct(PlayerStateMachine playerState, CurrencyAnimator currencyAnimation, CurrencyService currencyService)
		{
			_playerState = playerState;
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
			
			_playerState.OnExplore += ShowOverlay;
			_playerState.OnPause += _animator.HideHUD;
			_playerState.OnInteract += _animator.HideOverlay;	
		}

		private void OnDisable()
		{
			_currencyService.OnCurrencyChanged -= ChangeCurrency;
			
			_playerState.OnExplore -= ShowOverlay;
			_playerState.OnPause -= _animator.HideHUD;
			_playerState.OnInteract -= _animator.HideOverlay;
		}

		private void ShowOverlay()
		{
			_animator.ShowHUD();
			_animator.ShowOverlay();
			SetCurrency();
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
