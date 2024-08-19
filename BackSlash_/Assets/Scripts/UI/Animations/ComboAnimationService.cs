using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ComboAnimationService : MonoBehaviour
{
    [Header("Animation Settings")]
    //[SerializeField] private Color _inactiveIndicator = new Color(0.1f, 0.19f, 0.27f, 1);
    [SerializeField] private Color _fillIndicator = new Color(0.066f, 1, 1, 1);
    [SerializeField] private float _duration = 1;

    [Header("Animation Components")]
    [SerializeField] private CanvasGroup _canvasGroup;
    [Space]
    [SerializeField] private Image _fillImage;
    [SerializeField] private float _fillDuration = 0.5f;
    [Space]
    [SerializeField] private Image _frameImage;
    [Space]
    [SerializeField] private Image _flash;


    private Sequence _sequence;

    public event Action OnAnimationFinished;


    // Заполняет индикатор прогресса комбо
    public void IndicatorPosition(float value)
    {
        //_fillImage.fillAmount += isProgress;
        //ComboContinueFlash(_fillImage.fillAmount);
        if (value == 0)
        {
            _fillImage.fillAmount = 0;
        }
        else
        {
            _fillImage.DOFillAmount(value, _fillDuration).SetEase(Ease.OutExpo);
        }
    }

    // Показывает клавишу для продолжения комбо
    public void ShowKey(GameObject key)
    {
        key.SetActive(true);
    }

    // Скрывает клавишу для продолжения комбо
    public void HideKey(GameObject key)
    {
        key.SetActive(false);
    }

    private void ComboContinueFlash(float value)
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(_flash.DOFade(value, 0.1f));
        sequence.Append(_flash.DOFade(0, 0.01f));
    }

    private void FrameAnimation()
    {
        _sequence = DOTween.Sequence();
        _sequence.AppendCallback(() =>
        {
            _frameImage.DOFade(0, _duration).SetEase(Ease.Flash);
            _frameImage.transform.DOScale(0.75f, _duration).SetEase(Ease.Flash);
        });
        _sequence.AppendInterval(_duration + 0.03f);
        _sequence.AppendCallback(() =>
        {
            _frameImage.color = _fillIndicator;
            _frameImage.transform.localScale = Vector3.one;
        });
        _sequence.SetLoops(-1);
    }

    // Комбо прервано
    public void AnimateCancelCombo()
    {
        _canvasGroup.alpha = 1;
        _canvasGroup.DOFade(0.7f, 0.5f).OnComplete(() => OnAnimationFinished?.Invoke());
    }

    // Комбо закончено
    public void AnimateFinishCombo()
    {
        _canvasGroup.alpha = 0.7f;
        _canvasGroup.DOFade(1, 0.5f).OnComplete(() => OnAnimationFinished?.Invoke());
    }

    public void AnimateStartCombo()
    {
        _canvasGroup.alpha = 1;
    }
}