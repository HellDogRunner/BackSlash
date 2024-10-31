using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TradeAnimator : MonoBehaviour
{
	[Header("Settings")]
	[SerializeField] private float _duration;
	[SerializeField] private Color _textColor;
	[SerializeField] private Color _frameColor;
	[SerializeField] private Color _soldColor;
	[SerializeField] private Color _selectedColor;
	[SerializeField] private Color _needCurrencyColor;
	
	private Sequence _select;
	private Sequence _deselect;
	private Tween _bought;
	private Sequence _needCurrency;
	private Sequence _buy;
	
	public void Select(Image frame, TMP_Text name) 
	{
		_select = Sequence(_select);
		_select.AppendCallback(() => 
		{
			frame.DOColor(_selectedColor, _duration);
			name.DOColor(_selectedColor, _duration);
		}); 
	}
	
	public void Deselect(Image frame, TMP_Text name)
	{
		_deselect = Sequence(_deselect);
		_deselect.AppendCallback(() => 
		{
			frame.DOColor(_frameColor, _duration);
			name.DOColor(_textColor, _duration);
		}); 
	}
	
	public void Bought(Image frame)
	{
		TryKillTween(_bought);
		_bought = frame.transform.DOShakePosition(_duration);
	}
	
	public void NeedCurrency(TMP_Text price)
	{
		_needCurrency = Sequence(_needCurrency);
		_needCurrency.Append(price.DOColor(_needCurrencyColor, _duration));
		_needCurrency.AppendInterval(_duration);
		_needCurrency.Append(price.DOColor(_textColor, _duration));
	}
	
	public void Buy(Image sold)
	{
		sold.gameObject.SetActive(true);
		
		_buy = Sequence(_buy);
		_buy.Append(sold.DOColor(_selectedColor, _duration));
		_buy.Append(sold.DOColor(_soldColor, _duration));
	}
	
	private void TryKillTween(Tween tween)
	{
		if (tween.IsActive()) tween.Kill();
	}
	
	private Sequence Sequence(Sequence sequence) 
	{
		if (sequence.IsActive()) sequence.Kill();
		
		sequence = DOTween.Sequence();
		return sequence;
	}
	
	private void OnDestroy()
	{
		if (_buy.IsActive()) _buy.Kill();
		if (_bought.IsActive()) _bought.Kill();
		if (_select.IsActive()) _select.Kill();
		if (_deselect.IsActive()) _deselect.Kill();
		if (_needCurrency.IsActive()) _needCurrency.Kill();
	}
}