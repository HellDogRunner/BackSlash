using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace RedMoonGames.Window
{
    public class LoadingSceneAnimation : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Image _fill;
        [SerializeField] private Image _glow;

        [Header("Animation Settings")]
        [SerializeField] private float _transitionDuration = 1f;
        [SerializeField] private float _fillDuration = 3f;

        public event Action OnLoadingEnd;
        public event Action OnOpeningEnd;
        public event Action OnClosingEnd;
 
        public void AnimateLoading()
        {
            _fill.DOFillAmount(1, _fillDuration).SetUpdate(true).SetEase(Ease.Flash);
            _glow.DOFillAmount(1, _fillDuration).SetUpdate(true).SetEase(Ease.Flash).
                OnComplete(() => OnLoadingEnd?.Invoke());
        }

        public void AnimateOpening()
        {
            _canvasGroup.alpha = 1;
            _glow.fillAmount = 1;
            _fill.fillAmount = 1;

            Sequence sequence = DOTween.Sequence();
            sequence.AppendInterval(_transitionDuration);
            sequence.Append(_canvasGroup.DOFade(0, _transitionDuration).
                OnComplete(() => OnOpeningEnd?.Invoke()));
        }

        public void AnimateClosing()
        {
            _canvasGroup.alpha = 0;
            _glow.fillAmount = 0;
            _fill.fillAmount = 0;

            _canvasGroup.DOFade(1, _transitionDuration).SetUpdate(true).
                OnComplete(() =>
                {
                    OnClosingEnd?.Invoke();
                    AnimateLoading();
                });
        }
    }
}