using UnityEngine;
using Zenject;
using static PlayerStates;

namespace RedMoonGames.Window
{
	public class HUDController : MonoBehaviour
	{
		[SerializeField] private HUDAnimationService _animator;

		private PlayerStateMachine _playerState;
		private CurrencyService _currencyService;
		private CurrencyAnimation _currencyAnimation;
		private InteractionSystem _interactionSystem;

		[Inject]
		private void Construct(PlayerStateMachine playerState, InteractionSystem interactionSystem, CurrencyAnimation currencyAnimation, CurrencyService currencyService)
		{
			_playerState = playerState;
			_currencyService = currencyService;
			_currencyAnimation = currencyAnimation;
			_interactionSystem = interactionSystem;
		}

		private void Awake()
		{
			SetCurrency();
		}

		private void OnEnable()
		{
			_currencyService.OnCurrencyChanged += ChangeCurrency;
			_interactionSystem.EndInteracting += SetCurrency;
			
			_playerState.OnExplore += _animator.ShowHUD;
			_playerState.OnExplore += _animator.ShowOverlay;
			_playerState.OnPause += _animator.HideHUD;
			_playerState.OnInteract += _animator.HideOverlay;	
		}

		private void OnDisable()
		{
			_currencyService.OnCurrencyChanged -= ChangeCurrency;
			_interactionSystem.EndInteracting -= SetCurrency;
			
			_playerState.OnExplore -= _animator.ShowHUD;
			_playerState.OnExplore -= _animator.ShowOverlay;
			_playerState.OnPause -= _animator.HideHUD;
			_playerState.OnInteract -= _animator.HideOverlay;
		}

		private void SetCurrency()
		{
			var value = _currencyService.Currency;
			var target = _animator.GetCurrency();

			_currencyAnimation.SetCurrency(target, value);
		}

		private void ChangeCurrency(int endValue)
		{
			_currencyAnimation.Animate(_animator.GetCurrency(), endValue);
		}
	}
}
