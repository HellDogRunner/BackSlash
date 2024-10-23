using UnityEngine;
using DG.Tweening;

public class TradeAnimation : MonoBehaviour
{
	[SerializeField] private CanvasGroup _tradeCG;
	
	[Header("Settings")]
	[SerializeField] private float _fadeDuration;
	
	private Tween _fade;
	
	private void Awake()
	{
		_tradeCG.alpha = 0;
		_tradeCG.gameObject.SetActive(false);
	}
	
	public void ShowWindow()
	{
		if (_fade.IsActive()) _fade.Kill();
		
		_tradeCG.gameObject.SetActive(true);
		_fade = _tradeCG.DOFade(1, _fadeDuration);
	}
	
	public void HideWindow()
	{
	if (_fade.IsActive()) _fade.Kill();

		_fade = _tradeCG.DOFade(0, _fadeDuration).
		OnComplete(() => _tradeCG.gameObject.SetActive(false));
	}
}
