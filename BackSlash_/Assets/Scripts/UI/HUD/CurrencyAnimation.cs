using TMPro;
using UnityEngine;

public class CurrencyAnimation : MonoBehaviour
{
	public void Animate(TMP_Text target, int startValue, int endValue)
	{
		// Анимация изменения значения валюты;
		
		target.text = endValue.ToString();
	}
}
