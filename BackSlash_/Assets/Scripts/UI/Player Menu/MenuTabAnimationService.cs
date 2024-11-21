using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Selectable))]
public class MenuTabAnimationService : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	[Header("Animation Components")]
	[SerializeField] private CanvasGroup _frameCG;
	[SerializeField] private CanvasGroup _selectCG;
	[SerializeField] private CanvasGroup _glowCG;
	[SerializeField] private TMP_Text _text;

	[Header("Animation Settings")]
	[SerializeField] private float _selectDuration = 0.35f;
	[SerializeField] private float _scale = 1.1f;
	[SerializeField] private float _decreaseScale = 0.7f;
	[SerializeField] private float _deselectAlpha = 0.25f;
	[SerializeField] private float _selectAlpha = 0.7f;
	[SerializeField] private float _activeAlpha = 1;
	[SerializeField] private float _selectBGAlpha = 0.05f;
	[SerializeField] private float _glowAlpha = 0.05f;

	private List<Tween> _tweens = new List<Tween>();
	private Sequence _select;
	private Sequence _deselect;
	private Sequence _enable;

	private bool _isActive;

	private void Awake()
	{
		SetDefault();
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		SelectTab();
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		DeselectTab();
	}

	private void SelectTab()
	{
		if (!_isActive)
		{
			_select = DOTween.Sequence();
			_select.AppendCallback(() =>
			{
				_text.DOFade(_selectAlpha, _selectDuration).SetUpdate(true);
				_selectCG.DOFade(_selectBGAlpha, _selectDuration).SetUpdate(true);
				_selectCG.transform.DOScale(_scale, _selectDuration).SetUpdate(true);
			}).SetUpdate(true);
		}
	}

	private void DeselectTab()
	{
		if (!_isActive)
		{
			_deselect = DOTween.Sequence();
			_deselect.AppendCallback(() =>
			{
				_text.DOFade(_deselectAlpha, _selectDuration).SetUpdate(true);
				_selectCG.DOFade(0, _selectDuration).SetUpdate(true);
				_selectCG.transform.DOScale(1, _selectDuration).SetUpdate(true);
			}).SetUpdate(true);
		}
	}

	public void EnableTab()
	{
		Debug.Log("Enable " + gameObject.name);
		_isActive = true;

		_selectCG.alpha = _selectBGAlpha;

		_enable = DOTween.Sequence();
		_enable.AppendCallback(() =>
		{
			_glowCG.DOFade(_glowAlpha, _selectDuration).SetUpdate(true);
			_selectCG.DOFade(0, _selectDuration).SetUpdate(true);
			_selectCG.transform.DOScale(_decreaseScale, _selectDuration).SetUpdate(true);
			_text.DOFade(_activeAlpha, _selectDuration).SetUpdate(true);
			_text.transform.DOScale(_scale, _selectDuration).SetUpdate(true);
			_frameCG.DOFade(1, _selectDuration).SetUpdate(true);
			_frameCG.transform.DOScale(_scale, _selectDuration).SetUpdate(true);
		}).SetUpdate(true);
	}

	public void DisableTab()
	{
		Debug.Log("Disable " + gameObject.name);
		SetDefault();
	}

	private void SetDefault()
	{
		_enable.Kill();
		
		_isActive = false;

		_text.alpha = _deselectAlpha;
		_text.transform.localScale = Vector3.one;

		_frameCG.alpha = 0;
		_frameCG.transform.localScale = Vector3.one;

		_selectCG.alpha = 0;
		_selectCG.transform.localScale = Vector3.one;

		_glowCG.alpha = 0;
	}

	private void TryKill(Tween tween)
	{
		if (tween.IsActive()) tween.Kill();
	}

	private void OnDestroy()
	{
		TryKill(_select);
		TryKill(_deselect);
		TryKill(_enable);
	}
}
