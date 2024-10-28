using TMPro;
using UnityEngine;

public class CurrencyAnimation : MonoBehaviour
{
	[SerializeField] private float _timeInterval;
	
	private TMP_Text _target;
	private float _time;
	private int _change;
	private int _startValue;
	private int _endValue;
	private bool _canAnimate;
	
	private void Update()
	{
		if (_canAnimate && _time + _timeInterval <= Time.time) 
		{
			if (_startValue == _endValue)
			{
				_canAnimate = false;
				return;
			}
			
			_startValue += _change;
			_target.text = _startValue.ToString();
			_time = Time.time;
		}
	}

	public void Animate(TMP_Text target, int startValue, int endValue)
	{	
		_startValue = startValue;
		_endValue = endValue;
		_change = _endValue > _startValue ? 1 : -1;
		_target = target;
		_target.text = _startValue.ToString();
		_time = Time.time;
		_canAnimate = true;
	}
	
	public void SetCurrency(TMP_Text target, int value)
	{
		target.text = value.ToString();
	}
}