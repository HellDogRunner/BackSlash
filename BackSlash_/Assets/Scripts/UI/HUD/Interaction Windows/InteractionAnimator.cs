using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InteractionAnimator : MonoBehaviour
{
	[Header("Interaction Objects")]
	[SerializeField] private CanvasGroup _tradeCG;
	[SerializeField] private CanvasGroup _dialogueCG;
	[SerializeField] private CanvasGroup _talkCG;
	[SerializeField] private CanvasGroup _answerKeysCG;
	
	[Header("Texts")]
	[SerializeField] private TMP_Text _name;
	[SerializeField] private Text _phrase;
	[Space]
	[SerializeField] private TMP_Text _currency;
	
	[Header("Settings")]
	[SerializeField] private float _startDelay = 0.3f;
	[SerializeField] private float _textDelay = 0.1f;
	[SerializeField] private float _charsPerSecond = 30;
	[Space]
	[SerializeField] private float _showDuration = 0.2f;
	[SerializeField] private float _hideDuration = 0.1f;
	[SerializeField] private float _lookAtDuration = 0.3f;

	private Transform _npcTR;
	private Vector3 _defaultRotation;
	private bool _firstPhrase;

	private Tween _dialogue;
	private Tween _trade;
	private Tween _talk;
	private Tween _answerKeys;
	private Tween _text;
	
	public event Action TextAnimationEnd;
	
	private void Awake()
	{
		_answerKeysCG.alpha = 0;
		
		_tradeCG.alpha = 0;
		_tradeCG.gameObject.SetActive(false);
		
		_dialogueCG.alpha = 0;
		_dialogueCG.gameObject.SetActive(false);
		
		_talkCG.alpha = 0;
		_talkCG.gameObject.SetActive(false);
		
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

	public void Dialogue(int alpha = 0)
	{
		SwitchAlphaAndSetActive(_dialogue, _dialogueCG, alpha);
	}

	public void Trade(int alpha = 0)
	{
		SwitchAlphaAndSetActive(_trade, _tradeCG, alpha);
	}

	public void TalkKey(int alpha = 0)
	{
		SwitchAlphaAndSetActive(_talk, _talkCG, alpha);
	}

	public void AnswerKeys(int alpha = 0)
	{
		SwitchObjectAlpha(_answerKeys, _answerKeysCG, alpha);
	}

	private void SwitchAlphaAndSetActive(Tween tween, CanvasGroup cg, float alpha = 0)
	{
		TryKillTween(tween);
		
		if (alpha == 0)
		{
			tween = cg.DOFade(alpha, _hideDuration).
			OnComplete(() => cg.gameObject.SetActive(false));
		}
		else
		{
			cg.gameObject.SetActive(true);
			tween = cg.DOFade(alpha, _showDuration);
		}
	}

	private void SwitchObjectAlpha(Tween tween, CanvasGroup cg, float alpha, bool isWindow = false) 
	{
		float duration;
		duration = isWindow ? _hideDuration : _showDuration;
		
		TryKillTween(tween);
		tween = cg.DOFade(alpha, duration).SetEase(Ease.Flash);
	}

	public void SetTransform(Transform transform, Vector3 rotation)
	{
		_npcTR = transform;
		_defaultRotation = rotation;
	}

	public void LookAtEachOther(Transform playerTR)
	{
		Vector3 playerPos = new Vector3(playerTR.position.x, _npcTR.position.y, playerTR.position.z);
		Vector3 npcPos = new Vector3(_npcTR.position.x, playerTR.position.y, _npcTR.position.z);

		_npcTR.DOLookAt(playerPos, _lookAtDuration);
		playerTR.DOLookAt(npcPos, _lookAtDuration);
	}

	public void RotateToDefault()
	{
		_npcTR.DORotate(_defaultRotation, _lookAtDuration);
	}
	
	private void TryKillTween(Tween tween)
	{
		if (tween.IsActive()) tween.Kill();
	}
	
	public TMP_Text GetCurrencyText()
	{
		return _currency;
	}
}