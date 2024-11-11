using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ProductAnimator : MonoBehaviour
{
	[Header("Components")]
	[SerializeField] private Image _icon;
	[SerializeField] private Image _sold;
	[SerializeField] private Image _frame;
	
	[Header("Settings")]
	[SerializeField] private float _duration;
	[SerializeField] private Color _textColor;
	[SerializeField] private Color _frameColor;
	[SerializeField] private Color _soldColor;
	[SerializeField] private Color _selectedColor;
	[SerializeField] private Color _needCurrencyColor;
	[SerializeField] private string _boughtText;
	
	private Sequence _select;
	private Sequence _deselect;
	private Tween _bought;
	private Sequence _needCurrency;
	private Sequence _buy;
	
	public event Action OnBuyComplete;
	
	public void Select() 
	{
		TryKill(_select);
		
		_select = DOTween.Sequence();
		_select.AppendCallback(() => 
		{
			_frame.DOColor(_selectedColor, _duration);
		}); 
	}
	
	public void Deselect()
	{
		TryKill(_deselect);
		
		_deselect = DOTween.Sequence();
		_deselect.AppendCallback(() => 
		{
			_frame.DOColor(_frameColor, _duration);
		}); 
	}
	
	public void Bought()
	{
		TryKill(_bought);
		_bought = _frame.transform.DOShakePosition(_duration);
	}
	
	public void NeedCurrency()
	{
		Debug.Log("Need Currency animation");
		
		// TryKill(_needCurrency);
		
		// _needCurrency = DOTween.Sequence();
		// _needCurrency.Append(_price.DOColor(_needCurrencyColor, _duration));
		// _needCurrency.AppendInterval(_duration);
		// _needCurrency.Append(_price.DOColor(_textColor, _duration));
	}
	
	public void Buy()
	{
		_sold.gameObject.SetActive(true);
		
		_buy = DOTween.Sequence();
		_buy.Append(_sold.DOColor(_selectedColor, _duration));
		_buy.Append(_sold.DOColor(_soldColor, _duration).OnComplete(() => OnBuyComplete?.Invoke()));
	}
	
	public void SetView(Sprite icon)
	{
		_icon.sprite = icon;
	}
	
	private void TryKill(Tween tween)
	{
		if (tween.IsActive()) tween.Kill();
	}
	
	private void OnDestroy()
	{
		DOTween.KillAll(true);
	}
}