using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

[RequireComponent(typeof(Selectable))]
public class SelectableAnimationService : MonoBehaviour, IPointerEnterHandler, IDeselectHandler, ISelectHandler
{
	[SerializeField] private bool _selectedOnEnable;

	[Header("Components")]
	[SerializeField] private CanvasGroup _frameCG;
	[SerializeField] private CanvasGroup _indicatorCG;
	[SerializeField] private RectTransform _frameRect;
	[SerializeField] private RectTransform _pointRect;
	[SerializeField] private TMP_Text _text;

	[Header("Settings")]
	[SerializeField] private float _fadeDuration = 0.35f;
	[SerializeField] private Vector2 _minStartAnchor = new Vector2(0.3f, 0);
	[SerializeField] private Vector2 _maxStartAnchor = new Vector2(0.7f, 1);
	[SerializeField] private Color _textSelectedColor;
	[SerializeField] private Color _textDeselectedColor;

	[Header("Indicator Animation")]
	[SerializeField] private Vector2 _punch = new Vector2(-15f, 0);
	[SerializeField] private int _vibrato = 12;

	private Vector2 _pointPosition =new Vector2(-20, 0);

	private List<Tween> _tweens = new List<Tween>();

	private void Awake()
	{
		_frameCG.alpha = 0;
		_indicatorCG.alpha = 0;
	}
	
	public void OnSelect(BaseEventData eventData) { ShowFrame(); }
	public void OnPointerEnter(PointerEventData eventData) { ShowFrame(); }
	public void OnDeselect(BaseEventData eventData) { HideFrame(); }

	private void OnEnable()
	{
		HideFrame();

		if (_selectedOnEnable)
		{
			ShowFrame();
		}
	}

	private void ShowFrame()
	{
		_text.color = _textSelectedColor;
		_indicatorCG.alpha = 1;

		if (_frameCG.alpha == 0)
		{
			_pointRect.localPosition = _pointPosition;
			_tweens.Add(_frameCG.DOFade(1, _fadeDuration).SetEase(Ease.InCubic).SetUpdate(true));
			_tweens.Add(_frameRect.DOAnchorMin(Vector2.zero, _fadeDuration).SetUpdate(true));
			_tweens.Add(_frameRect.DOAnchorMax(Vector2.one, _fadeDuration).SetUpdate(true));
			_tweens.Add(_pointRect.DOPunchAnchorPos(_punch, _fadeDuration, _vibrato, 0, true).SetUpdate(true));
		}
	}

	private void HideFrame()
	{
		_text.color = _textDeselectedColor;
		_frameCG.alpha = 0;
		_indicatorCG.alpha = 0;

		KillTweens();

		_frameRect.anchorMin = _minStartAnchor;
		_frameRect.anchorMax = _maxStartAnchor;
	}

	private void KillTweens()
	{
		foreach (var tween in _tweens)
		{
			if (tween.IsActive())
			{
				tween.Kill();
			}
		}
		_tweens.Clear();
	}

	private void OnDestroy()
	{
		KillTweens();
	}
}