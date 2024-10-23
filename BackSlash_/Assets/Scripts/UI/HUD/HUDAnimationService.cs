using DG.Tweening;
using TMPro;
using UnityEngine;

public class HUDAnimationService : MonoBehaviour
{
	[Header("Objects")]
	[SerializeField] private CanvasGroup _overlayCG;
	[SerializeField] private CanvasGroup _tradeCG;
	[SerializeField] private TMP_Text _currencyValue;

	[Header("Animation Settings")]
	[SerializeField] private float _showDelay = 1f;
	[SerializeField] private float _fadeDuration = 0.5f;

	private Tween _overlay;

	private void Awake()
	{
		_overlayCG.alpha = 0;
		_tradeCG.alpha = 0;
		SwitchOverlayView(1);
	}

	public void SwitchOverlayView(int fade)
	{
		if (_overlay.IsActive()) _overlay.Kill();

		if (fade == 0)
		{
			_overlay = _overlayCG.DOFade(fade, _fadeDuration).SetEase(Ease.Flash);
		}
		else _overlay = _overlayCG.DOFade(fade, _fadeDuration).SetEase(Ease.Flash).SetDelay(_showDelay);
	}

	public void CurrencyAnimation(int value)
	{
		// Анимация изменения значения валюты
		_currencyValue.text = value.ToString();
	}

	// Анимация для объекта с Canvas Group с переключением активности
	public void AnimateShow(CanvasGroup cg)
	{
		if (!cg.gameObject.activeSelf)
		{
			cg.gameObject.SetActive(true);
			cg.alpha = 0;
			cg.DOFade(1f, _fadeDuration).SetEase(Ease.InQuart);
		}
	}

	public void AnimateHide(CanvasGroup cg)
	{
		if (cg.gameObject.activeSelf)
		{
			cg.alpha = 1;
			cg.DOFade(0f, _fadeDuration).SetEase(Ease.InQuart).
				OnComplete(() => cg.gameObject.SetActive(false));
		}
	}
}