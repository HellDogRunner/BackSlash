using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ComboAnimationService : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private CanvasGroup _backslashCG;
    [SerializeField] private Image _fillImage;
    [SerializeField] private CanvasGroup _blinkCG;
    [SerializeField] private Image _blinkImage;
    [SerializeField] private Image _flash;

    [Header("Settings")]
    [SerializeField] private float _fillDuration = 0.5f;
    [SerializeField] private float _CancelDuration = 0.2f;
    [SerializeField] private float _blinkDuration = 0.5f;
    [SerializeField] private float _minBlinkFade = 0;
    [SerializeField] private float _maxBlinkFade = 1;
    [SerializeField] private Color _comboContinueColor = new Color(31, 52, 73, 1);
    [SerializeField] private Color _comboCancelColor = new Color(192, 57, 43, 1);
    [SerializeField] private Color _comboFinishColor = new Color(17, 255, 255, 1);

    private Sequence _blink;
    private Sequence _finish;

    public event Action OnAnimationFinished;

    public void SetStartAnimations()
    {
        _blink.Kill();
        _blinkCG.transform.localScale = Vector3.one;
        _blinkCG.alpha = 0f;

        ChangeBlinkColor(_comboContinueColor);
        HideCancelIndicator();
        EmptyIndicator();
    }

    public void FillIndicator(float value)
    {
        _fillImage.DOFillAmount(value, _fillDuration).SetEase(Ease.OutQuad);
    }

    public void EmptyIndicator()
    {
        _fillImage.fillAmount = 0;
    }

    public void ShowKey(GameObject key)
    {
        key.SetActive(true);
    }

    public void HideKey(GameObject key)
    {
        key.SetActive(false);
    }

    public void HideCancelIndicator()
    {
        _backslashCG.alpha = 0;
    }

    public void AnimateBlinkBackground()
    {
        if (!_blink.IsActive())
        {
            _blinkCG.alpha = 1;

            _blink = DOTween.Sequence();
            _blink.Append(_blinkImage.DOFade(_minBlinkFade, _blinkDuration));
            _blink.Append(_blinkImage.DOFade(_maxBlinkFade, _blinkDuration));
            _blink.SetLoops(-1);
        }
    }

    private void ChangeBlinkColor(Color color)
    {
        _blinkImage.color = color;
    }

    public void AnimateCancelCombo()
    {
        EmptyIndicator();
        ChangeBlinkColor(_comboCancelColor);
        AnimateBlinkBackground();

        Sequence sequence = DOTween.Sequence();
        sequence.AppendCallback(() =>
        {
            _backslashCG.DOFade(1, _CancelDuration);
            _backslashCG.transform.DOScale(1.5f, _CancelDuration);
        });
        sequence.AppendInterval(_CancelDuration);
        sequence.Append(_backslashCG.transform.DOScale(1f, _CancelDuration));
    }

    public void AnimateFinishCombo()
    {
        ChangeBlinkColor(_comboFinishColor);

        _finish = DOTween.Sequence();
        _finish.Append(_blinkCG.transform.DOScale(1.3f, _blinkDuration));
        _finish.Append(_blinkCG.transform.DOScale(0.8f, _blinkDuration)).OnComplete(() => OnAnimationFinished?.Invoke());
    }
}