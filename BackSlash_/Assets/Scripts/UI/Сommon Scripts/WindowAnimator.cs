using DG.Tweening;
using System;
using UnityEngine;

namespace RedMoonGames.Window
{
	public class WindowAnimator : MonoBehaviour
	{
		[Header("Basic Settings")]
		[SerializeField] private float _fadeDuration = 0.1f;

		private Tween _window;
		private Tween _windowDelay;

		public event Action<WindowHandler> OnWindowHided;
		public event Action OnWindowDelayShowed;

		public void ShowWindow(CanvasGroup cg)
		{
			cg.alpha = 0f;
			_window = cg.DOFade(1f, _fadeDuration).SetEase(Ease.InOutSine).SetUpdate(true);
		}

		public void HideWindow(CanvasGroup cg, WindowHandler window)
		{
			_window = cg.DOFade(0f, _fadeDuration).SetEase(Ease.InOutSine).SetUpdate(true).
			OnComplete(() => OnWindowHided?.Invoke(window));
		}

		public void ShowWindowWithDelay(CanvasGroup cg, float delay = 0)
		{
			cg.alpha = 0;
			_windowDelay = cg.DOFade(1f, _fadeDuration).SetEase(Ease.InOutSine).SetUpdate(true).SetDelay(delay).
			OnComplete(() => OnWindowDelayShowed?.Invoke());
		}

		public bool GetCanOpenWindow()
		{
			if (_window.IsActive()) return false;
			if (_windowDelay.IsActive()) return false;
			
			return true;
		}
	}
}
