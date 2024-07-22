using DG.Tweening;
using UnityEngine;
using Zenject;

namespace RedMoonGames.Window
{
    public class UIAnimationController : MonoBehaviour
    {
        [Header("Animation Settings")]
        [SerializeField] private float _fadeDuration = 0.1f;

        private GameWindowsController _windowsController;

        [Inject]
        private void Construct(GameWindowsController windowsController)
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
            cg.DOFade(0f, _fadeDuration).SetEase(Ease.InOutSine).SetUpdate(true).OnComplete(
                () => _windowsController.CloseWindow(handler));
            //_sequence = DOTween.Sequence();
            //_sequence.AppendCallback(() =>
            //{
            //    cg.DOFade(0f, _fadeDuration).SetEase(Ease.InOutSine).SetUpdate(true).OnComplete(CloseHandle);
            //}).SetUpdate(true);
        }
    }
}