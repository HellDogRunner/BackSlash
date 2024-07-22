using DG.Tweening;
using UnityEngine;
using Zenject;

namespace RedMoonGames.Window
{
    public class StartAnimationController : MonoBehaviour
    {
        [Header("Animation Settings")]
        [SerializeField] private float _fadeDuration = 1f;

        private StartWindowsController _windowsManager;

        private WindowHandler _handler;

        [Inject]
        private void Construct(StartWindowsController windowsManager)
        {
            _windowsManager = windowsManager;
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
            _windowsManager.CloseWindow(_handler);
        }
    }
}