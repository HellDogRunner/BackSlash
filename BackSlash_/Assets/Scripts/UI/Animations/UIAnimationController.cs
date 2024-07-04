using DG.Tweening;
using UnityEngine;
using Zenject;

namespace RedMoonGames.Window
{
    public class UIAnimationController : MonoBehaviour
    {
        [Header("Animation Settings")]
        [SerializeField] private float _fadeDuration = 1f;

        private GameWindowsManager _windowsController;

        private WindowHandler _handler;

        private Sequence _sequence;

        [Inject]
        private void Construct(GameWindowsManager windowsController)
        {
            _windowsController = windowsController;
        }

        public void ShowWindowAnimation(CanvasGroup cg)
        {
            cg.alpha = 0f;
            cg.DOFade(1f, _fadeDuration).SetEase(Ease.InOutSine).SetUpdate(true);
        }

        public void HideWindowAnimation(CanvasGroup cg, WindowHandler handler)
        {
            _handler = handler;

            _sequence = DOTween.Sequence();
            _sequence.AppendCallback(() =>
            {
                cg.DOFade(0f, _fadeDuration).SetEase(Ease.InOutSine).SetUpdate(true).OnComplete(CloseHandle);
            }).SetUpdate(true);
        }
            
        private void CloseHandle()
        {
            _windowsController.CloseWindow(_handler);
        }
    }
}