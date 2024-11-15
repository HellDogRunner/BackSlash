using DG.Tweening;
using System;
using UnityEngine;

namespace RedMoonGames.Window
{
	public class WindowAnimator : MonoBehaviour
	{
		[Header("Basic Settings")]
		[SerializeField] private float _fadeDuration = 0.1f;
		
		private bool _windowClosing;

		private Tween _window;
		private Tween _windowDelay;

		public event Action<WindowHandler> OnWindowHided;
		public event Action OnWindowDelayShowed;

		public void ShowWindow(CanvasGroup cg)
		{
			if (!_windowClosing)
			{
				TryKillTween(_window);
				
				cg.alpha = 0f;
				_window = cg.DOFade(1f, _fadeDuration).SetEase(Ease.InOutSine).SetUpdate(true);
			}
		}

		public void HideWindow(CanvasGroup cg, WindowHandler window)
		{
			if (!_windowClosing)
			{
				TryKillTween(_window);
				
				_windowClosing = true;
				_window = cg.DOFade(0f, _fadeDuration).SetEase(Ease.InOutSine).SetUpdate(true).OnComplete(() => 
				{
					_windowClosing = false;
					OnWindowHided?.Invoke(window);
				});
			}
		}

		public void ShowWindowWithDelay(CanvasGroup cg, float delay = 0)
		{
			TryKillTween(_windowDelay);

			cg.alpha = 0;
			_windowDelay = cg.DOFade(1f, _fadeDuration).SetEase(Ease.InOutSine).SetUpdate(true).SetDelay(delay).
			OnComplete(() => OnWindowDelayShowed?.Invoke());
		}
		
		private void TryKillTween(Tween tween)
		{
			if (tween.IsActive()) tween.Kill();
		}
	}
}