using DG.Tweening;
using UnityEngine;
using Zenject;

namespace RedMoonGames.Window
{
    public class WindowAnimationService : MonoBehaviour
    {
        //[Header("Components")]
        //[SerializeField] private CanvasGroup _blinkCG;

        [Header("Basic Settings")]
        [SerializeField] private float _fadeDuration = 0.1f;

        //[Header("Menu blink")]
        //[SerializeField] private bool _animateBlink;
        //[SerializeField] private float _blinkDuration = 1;
        //[SerializeField] private float _maxBlinkAlpha = 1;
        //[SerializeField] private float _minBlinkAlpha = 0.5f;

        //private Sequence _blink;

        private bool _windowClosing;

        private WindowService _windowService;

        [Inject]
        private void Construct(WindowService windowService)
        {
            _windowService = windowService;
        }

        public void ShowWindowAnimation(CanvasGroup cg)
        {
            cg.alpha = 0f;
            cg.DOFade(1f, _fadeDuration).SetEase(Ease.InOutSine).SetUpdate(true);
        }

        public void HideWindowAnimation(CanvasGroup cg, WindowHandler handler)
        {
            if (!_windowClosing)
            {
                _windowClosing = true;
                cg.DOFade(0f, _fadeDuration).SetEase(Ease.InOutSine).SetUpdate(true).OnComplete(() => 
                {
                    _windowClosing = false;
                    _windowService.CloseWindow(handler);
                });
            }
        }

        // Пока не знаю как это нормально запихнуть в общий скрипт
        //public void BlinkAnimation()
        //{
        //    if (_animateBlink)
        //    {
        //        _blinkCG.alpha = _maxBlinkAlpha;

        //        _blink = DOTween.Sequence();
        //        _blink.Append(_blinkCG.DOFade(_minBlinkAlpha, _blinkDuration).SetUpdate(true));
        //        _blink.Append(_blinkCG.DOFade(_maxBlinkAlpha, _blinkDuration).SetUpdate(true));
        //        _blink.SetLoops(-1).SetUpdate(true);
        //    }
        //}
    }
}