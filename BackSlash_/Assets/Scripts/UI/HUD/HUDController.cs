using UnityEngine;
using Zenject;

namespace RedMoonGames.Window
{
	public class HUDController : MonoBehaviour
	{
		[SerializeField] private HUDAnimationService _animator;

		private bool _isLocked;
		
		private GameMenuController _menuController;
		private CurrencyAnimator _currencyAnimation;
		private CurrencyService _currencyService;
		private TargetLock _targetLock;

		[Inject]
		private void Construct(GameMenuController menuController, TargetLock targetLock, CurrencyAnimator currencyAnimation, CurrencyService currencyService)
		{
			_targetLock = targetLock;
			_menuController = menuController;
			_currencyService = currencyService;
			_currencyAnimation = currencyAnimation;
		}

		private void Awake()
		{
			SetCurrency();
			_animator.ShowHUD();
		}

		private void OnEnable()
		{
			_currencyService.OnCurrencyChanged += ChangeCurrency;
			_targetLock.OnSwitchLock += SwitchLock;
			_menuController.OnShowUI += ShowHUD;
		}

		private void OnDisable()
		{
			_currencyService.OnCurrencyChanged -= ChangeCurrency;
			_targetLock.OnSwitchLock -= SwitchLock;
			_menuController.OnShowUI -= ShowHUD;
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

		private void ShowHUD(bool value)
		{
			if (value)
			{
				_animator.HideHUD();
			}
			else
			{
				_animator.ShowHUD();
			}
			
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
