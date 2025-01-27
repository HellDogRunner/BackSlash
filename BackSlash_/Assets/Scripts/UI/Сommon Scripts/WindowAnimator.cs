using DG.Tweening;
using System;
using UnityEngine;

namespace RedMoonGames.Window
{
	public class WindowAnimator : MonoBehaviour
	{
		[Header("Basic Settings")]
		[SerializeField] private float _fadeDuration = 0.1f;
		[SerializeField] private float _delayTime = 0.1f;

		private Tween _window;
 
		public event Action<WindowHandler> OnShowing;
		public event Action<WindowHandler> OnShowed;
		public event Action<WindowHandler> OnHiding;
		public event Action<WindowHandler> OnHided;

		public void ShowWindow(WindowHandler handler, CanvasGroup cg, bool delay = false)
		{
			OnShowing?.Invoke(handler);
			var d = delay ? _delayTime : 0;
			cg.alpha = 0;
			KillTween(_window);
			_window = cg.DOFade(1f, _fadeDuration).SetEase(Ease.InOutSine).SetUpdate(true).SetDelay(d).
			OnComplete(() => OnShowed?.Invoke(handler));
		}

		public void HideWindow(WindowHandler handler, CanvasGroup cg, bool delay = false)
		{
			OnHiding?.Invoke(handler);
			var d = delay ? _delayTime : 0;
			KillTween(_window);
			_window = cg.DOFade(0f, _fadeDuration).SetEase(Ease.InOutSine).SetUpdate(true).SetDelay(d).
			OnComplete(() => OnHided?.Invoke(handler));
		}

		public void KillTween(Tween tween)
		{
			if (tween.IsActive()) tween.Kill();
		}

		public bool Active()
		{
			return _window.IsActive() ? true : false;
		}
		
		private void OnDestroy()
		{
			KillTween(_window);
		}
	}
}
