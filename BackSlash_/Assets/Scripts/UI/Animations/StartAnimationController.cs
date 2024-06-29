using DG.Tweening;
using UnityEngine;
using Zenject;

namespace RedMoonGames.Window
{
    public class StartAnimationController : MonoBehaviour
    {
        [Header("Animation Settings")]
        [SerializeField] private float _fadeDuration = 1f;

        private StartWindowsManager _windowsController;

        private WindowHandler _handler;

        //private Sequence _sequence;

        [Inject]
        private void Construct(StartWindowsManager windowsController)
        {
            _windowsController = windowsController;
        }

        public void ShowWindowAnimation(CanvasGroup cg)
        {
            cg.DOFade(1f, _fadeDuration);
        }

        public void HideWindowAnimation(CanvasGroup cg, WindowHandler handler)
        {
            _handler = handler;

            cg.DOFade(0f, _fadeDuration).OnComplete(CloseHandle);
        }

        private void CloseHandle()
        {
            _windowsController.CloseWindow(_handler);
        }
    }
}