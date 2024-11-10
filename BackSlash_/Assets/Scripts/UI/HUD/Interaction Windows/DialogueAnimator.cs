using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueAnimator : MonoBehaviour
{
	[SerializeField] private CanvasGroup _answerKeysCG;
	[Space]
	[SerializeField] private TMP_Text _name;
	[SerializeField] private Text _phrase;
	
	[Header("Settings")]
	[SerializeField] private float _startDelay = 0.3f;
	[SerializeField] private float _textDelay = 0.1f;
	[SerializeField] private float _charsPerSecond = 30;
	[SerializeField] private float _duration = 0.3f;

	private bool _firstPhrase;
	
	private Tween _answerKeys;
	private Tween _text;
	
	public event Action TextAnimationEnd;
	
	private void Awake()
	{
		_answerKeysCG.alpha = 0;
		_answerKeysCG.gameObject.SetActive(false);
	}
	
	public void SetName(string name)
	{
		_name.text = name + ":";
		_firstPhrase = true;
	}
	
	public void PhraseAnimation(string text)
	{
		float duration = text.Length / _charsPerSecond;
		float delay = _textDelay;

		if (_firstPhrase) delay = _startDelay;
		_firstPhrase = false;

		_phrase.text = "";
		_text = _phrase.DOText(text, duration).SetEase(Ease.Flash).SetDelay(delay).
			OnComplete(() => TextAnimationEnd?.Invoke());
	}
	
	public bool GetTextAnimationActive() 
	{
		return _text.IsActive();
	}

	public void ShowWholePhrase(string text)
	{
		TryKillTween(_text);
		_phrase.text = text;

		TextAnimationEnd?.Invoke();
	}
	
	public void ShowAnswerKeys()
	{
		TryKillTween(_answerKeys);
		
		_answerKeysCG.gameObject.SetActive(true);
		_answerKeys = _answerKeysCG.DOFade(1, _duration);
	}
	
	public void HideAnswerKeys()
	{
		TryKillTween(_answerKeys);
		
		_answerKeys = _answerKeysCG.DOFade(0, _duration).
		OnComplete(() => _answerKeysCG.gameObject.SetActive(false));
	}
	
	private void TryKillTween(Tween tween)
	{
		if (tween.IsActive()) tween.Kill();
	}
	
	private  void OnDestroy()
	{
		TryKillTween(_text);
		TryKillTween(_answerKeys);
	}
}
