using Scripts.Player;
using UnityEngine;
using Zenject;

namespace RedMoonGames.Window
{
	public class HUDController : MonoBehaviour
	{
		[SerializeField] private HUDAnimationService _animator;

		private bool _isLocked;

		private PlayerStateController _stateController;
		private CurrencyAnimator _currencyAnimation;
		private CurrencyService _currencyService;
		private TargetLock _targetLock;
		private TimeController _time;

		[Inject]
		private void Construct(TargetLock targetLock, TimeController time, PlayerStateController stateController, CurrencyAnimator currencyAnimation, CurrencyService currencyService)
		{
			_time = time;
			_targetLock = targetLock;
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
			_targetLock.OnSwitchLock += SwitchLock;
			_time.OnPause += Pause;
		}

		private void OnDisable()
		{
			_currencyService.OnCurrencyChanged -= ChangeCurrency;
			_stateController.OnInteract -= Interact;
			_targetLock.OnSwitchLock -= SwitchLock;
			_time.OnPause -= Pause;
		}

		private void Update()
		{
			if (_isLocked) _animator.Targeting();
		}

		private void SwitchLock(bool value)
		{
			if (value) _animator.SetTarget(_targetLock.Target.LookAt);
			_animator.ShowTargetIcon(value);
			_isLocked = value;
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
