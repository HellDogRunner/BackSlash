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
 
		public event Action OnHided;
		public event Action OnShowed;

		public void ShowWindow(CanvasGroup cg, float delay = 0)
		{
			cg.alpha = 0;
			KillTween(_window);
			_window = cg.DOFade(1f, _fadeDuration).SetEase(Ease.InOutSine).SetUpdate(true).SetDelay(delay).
			OnComplete(() => OnShowed?.Invoke());
		}

		public void HideWindow(CanvasGroup cg, float delay = 0)
		{
			KillTween(_window);
			_window = cg.DOFade(0f, _fadeDuration).SetEase(Ease.InOutSine).SetUpdate(true).SetDelay(delay).
			OnComplete(() => OnHided?.Invoke());
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
