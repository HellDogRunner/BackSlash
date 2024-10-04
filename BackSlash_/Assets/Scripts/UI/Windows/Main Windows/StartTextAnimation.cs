using DG.Tweening;
using TMPro;
using UnityEngine;

namespace RedMoonGames.Window
{
    public class StartTextAnimation : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        [Header("Animation Settings")]
        [SerializeField] private float _fadeDuration = 1f;
        [SerializeField] private float _minFadeValue = 0f;
        [SerializeField] private float _maxFadeValue = 1f;

        private Sequence _sequence;

        private void Awake()
        {
            PlayAnimation();
        }

        private void PlayAnimation()
        {
            _text.alpha = _minFadeValue;
            _sequence = DOTween.Sequence();
            _sequence.Append(_text.DOFade(_maxFadeValue, _fadeDuration));
            _sequence.Append(_text.DOFade(_minFadeValue, _fadeDuration));
            _sequence.SetLoops(-1);
        }

        private void OnDestroy()
        {
            _sequence.Kill();
        }
    }
}