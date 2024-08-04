using DG.Tweening;
using UnityEngine;
using Zenject;

namespace RedMoonGames.Window
{
    public class UIAnimationController : MonoBehaviour
    {
        [Header("Animation Settings")]
        [SerializeField] private float _fadeDuration = 0.1f;

        private bool _windowClosing;

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
            if (!_windowClosing)
            {
                cg.DOFade(0f, _fadeDuration).SetEase(Ease.InOutSine).SetUpdate(true).OnComplete(
                () => ClosingApprove(handler));
                _windowClosing = true;
            }
        }

        private void ClosingApprove(WindowHandler handler)
        {
            _windowClosing = false;
            _windowsController.CloseWindow(handler);
        }
    }
}



// Callback sequence

//_sequence = DOTween.Sequence();
//_sequence.AppendCallback(() =>
//{
//    cg.DOFade(0f, _fadeDuration).SetEase(Ease.InOutSine).SetUpdate(true).OnComplete(CloseHandle);
//}).SetUpdate(true);