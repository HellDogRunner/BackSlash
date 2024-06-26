using DG.Tweening;
using System;
using UnityEngine;

namespace Scripts.UI
{
    public class PauseWindowAnimation : MonoBehaviour
    {
        [Header("Rects")]
        //[SerializeField] private RectTransform _healthSegment;
        //[SerializeField] private RectTransform _dodjeSegment;
        [SerializeField] private RectTransform _background;

        //[Header("Segments Position")]
        //[SerializeField] private Vector2 _startHealth;
        //[SerializeField] private Vector2 _endHealth;
        //[Space]
        //[SerializeField] private Vector2 _startDodge;
        //[SerializeField] private Vector2 _endDodge;
       
        [Header("Animation Settings")]
        [SerializeField] private float _fadeDuration = 1f;

        [SerializeField] private UIManager _manager;

        private Sequence _sequence;
        private CanvasGroup _canvasAlpha;

        //private Canvas _canvas;

        public event Action<Canvas> OnAnimationHide;
        public event Action<Canvas> OnAnimationShow;

        public void HideAnimation(Canvas canvas)
        {
            _canvasAlpha = canvas.gameObject.GetComponent<CanvasGroup>();

            _sequence = DOTween.Sequence();
            _sequence.AppendCallback(() =>
            {
                _canvasAlpha.DOFade(0f, _fadeDuration).SetUpdate(true).OnComplete(Zatichka);
            });
            AnimationHide(canvas);
        }

        public void ShowAnimation(Canvas canvas)
        {
            _canvasAlpha = canvas.gameObject.GetComponent<CanvasGroup>();
            _canvasAlpha.DOFade(1f, _fadeDuration).SetUpdate(true);
            AnimationShow(canvas);

            //_sequence = DOTween.Sequence();
            //_sequence.AppendCallback(() =>
            //{
            //    _canvasAlpha.DOFade(1f, _fadeDuration).SetUpdate(true);
            //}).SetUpdate(true).OnComplete(AnimationShow);
        }

        //public void HideHUD()
        //{
        //    _sequence = DOTween.Sequence();
        //    _sequence.AppendCallback(() =>
        //    {
        //        _background.DOScale(1.5f, _fadeDuration).SetUpdate(true);
        //        _background.GetComponent<CanvasGroup>().DOFade(0f, _fadeDuration).SetUpdate(true);
        //    }).SetUpdate(true).OnComplete(AnimationHide);
        //}

        public void ShowHUD()
        {

        }

        private void AnimationHide(Canvas canvas)
        {
            OnAnimationHide(canvas);
        }

        private void AnimationShow(Canvas canvas)
        {
            OnAnimationShow(canvas);
        }

        private void Zatichka()
        {

        }
    }
}

//canvasRect.DOAnchorPos(_endPosition, _animationDuration, false).SetUpdate(true);
//canvasRect.DOScale(_endWindowScale, _animationDuration).SetUpdate(true);