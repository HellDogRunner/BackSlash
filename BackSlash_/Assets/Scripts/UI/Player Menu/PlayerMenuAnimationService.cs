using DG.Tweening;
using UnityEngine;

public class PlayerMenuAnimationService : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private CanvasGroup _blinkCG;

    [Header("Animation Settings")]
    [SerializeField] private float _menuDuration = 0.1f;
    [SerializeField] private float _blinkDuration = 1;
    [SerializeField] private float _minBlinkAlpha = 0.5f;
    [SerializeField] private float _maxBlinkAlpha = 1;
    [SerializeField] private bool _animateBlink;

    private Sequence _blink;

    private bool _windowClosing;

    public void AnimateMenuShow(CanvasGroup cg)
    {
        StartBlink();

        cg.alpha = 0f;
        cg.DOFade(1f, _menuDuration).SetEase(Ease.InOutSine).SetUpdate(true);
    }

    public void AnimateMenuHide(CanvasGroup cg)
    {
        if (!_windowClosing)
        {
            StopBlink();

            _windowClosing = true;
            cg.DOFade(0f, _menuDuration).SetEase(Ease.InOutSine).SetUpdate(true).OnComplete(() => _windowClosing = false);
        }
    }

    public void StartBlink()
    {
        if (_animateBlink)
        {
            _blinkCG.alpha = _maxBlinkAlpha;

            _blink = DOTween.Sequence();
            _blink.Append(_blinkCG.DOFade(_minBlinkAlpha, _blinkDuration).SetUpdate(true));
            _blink.Append(_blinkCG.DOFade(_maxBlinkAlpha, _blinkDuration).SetUpdate(true));
            _blink.SetLoops(-1).SetUpdate(true);
        }
    }

    public void StopBlink()
    {
        _blink.Kill();
    }
}