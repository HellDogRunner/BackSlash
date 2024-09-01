using DG.Tweening;
using TMPro;
using UnityEngine;
using Zenject;

namespace RedMoonGames.Window
{
    public class StartAnimationService : MonoBehaviour
    {
        [Header("Animation Settings")]
        [SerializeField] private float _fadeDuration = 1f;

        private MainWindowsController _windowsManager;

        private TMP_Text _text;
        private Sequence _sequence;

        [Inject]
        private void Construct(MainWindowsController windowsManager)
        {
            _windowsManager = windowsManager;
        }

        private void Awake()
        {
            _text = GetComponent<TMP_Text>();

            PlayAnimation();
        }

        private void PlayAnimation()
        {
            _text.alpha = 0;
            _sequence = DOTween.Sequence();
            _sequence.Append(_text.DOFade(1, _fadeDuration));
            _sequence.Append(_text.DOFade(0, _fadeDuration));
            _sequence.SetLoops(-1);
        }

        private void StopAnimation()
        {
            _sequence.Kill();
        }

        private void OnDestroy()
        {
            _sequence.Kill();
        }
    }
}