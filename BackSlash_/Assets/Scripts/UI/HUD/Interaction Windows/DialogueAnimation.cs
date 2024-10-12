using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueAnimation : MonoBehaviour
{
    [Header("Text")]
    [SerializeField] public TMP_Text Name;
    [SerializeField] private Text _phrase;

    [Header("Canvas Groups")]
    [SerializeField] private CanvasGroup _windowCG;
    [SerializeField] private CanvasGroup _talkCG;
    [SerializeField] private CanvasGroup _answerKeysCG;

    [Header("Settings")]
    [SerializeField] private float _startDelay = 0.3f;
    [SerializeField] private float _textDelay = 0.1f;
    [SerializeField] private float _charsPerSecond = 30;
    [Space]
    [SerializeField] private float _talkDuration = 0.1f;
    [SerializeField] private float _windowDuration = 0.1f;
    [SerializeField] private float _answerDuration = 0.1f;

    public Tween _text;
    private Tween _window;
    private Tween _talk;
    private Tween _answerKeys;

    [HideInInspector] public bool _firstPhrase;

    public event Action AnimationEnd;

    private void Awake()
    {
        _windowCG.alpha = 0;
        _talkCG.alpha = 0;
        _answerKeysCG.alpha = 0;

        _phrase.text = "";
    }

    public void PhraseAnimation(string text)
    {
        float duration = text.Length / _charsPerSecond;
        float delay = _textDelay;

        if (_firstPhrase) delay = _startDelay;
        _firstPhrase = false;

        _phrase.text = "";
        _text = _phrase.DOText(text, duration).SetEase(Ease.Flash).SetDelay(delay).
            OnComplete(() => AnimationEnd?.Invoke());
    }

    public void ShowWholePhrase(string text)
    {
        _text.Kill();
        _phrase.text = text;
        AnimationEnd?.Invoke();
    }

    public void InteractionKey(int fade = 0)
    {
        if (_talk.IsActive()) _talk.Kill();

        _talk = _talkCG.DOFade(fade, _talkDuration).SetEase(Ease.Flash);
    }

    public void Window(int fade = 0)
    {
        if (_window.IsActive()) _window.Kill();

        _window = _windowCG.DOFade(fade, _windowDuration).SetEase(Ease.Flash);
    }

    public void AnswerKeys(int fade = 0)
    {
        if (_answerKeys.IsActive()) _answerKeys.Kill();

        _answerKeys = _answerKeysCG.DOFade(fade, _answerDuration).SetEase(Ease.Flash);
    }
}