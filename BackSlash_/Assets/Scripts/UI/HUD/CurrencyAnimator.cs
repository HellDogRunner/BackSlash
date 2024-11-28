using System;
using TMPro;
using UnityEngine;

public class CurrencyAnimator : MonoBehaviour
{
	[SerializeField] private float _timeInterval;
	[SerializeField] private int _value;
	
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
			
			_startValue += _change * GetValue();
			_target.text = _startValue.ToString();
			_time = Time.time;
		}
	}

	public void Animate(TMP_Text target, int endValue)
	{	
		_startValue = int.Parse(target.text);
		_endValue = endValue;
		_change = _endValue > _startValue ? 1 : -1;
		_target = target;
		_target.text = _startValue.ToString();
		_time = Time.time;
		_canAnimate = true;
	}
	
	private int GetValue()
	{
		int value = 1;
		int abs = Math.Abs(_endValue - _startValue);
		
		while (value * _value < abs)
		{
			value *= _value;
		}
		return value;
	}
}