using DG.Tweening;
using UnityEngine;
using Zenject;

namespace RedMoonGames.Window
{
    public class MainAnimationService : MonoBehaviour
    {
        [Header("Animation Settings")]
        [SerializeField] private float _fadeDuration = 1f;

        private bool _windowClosing;

        private MainWindowsController _windowsController;

        [Inject]
        private void Construct(MainWindowsController windowsManager)
        {
            _windowsController = windowsManager;
        }

        public void ShowWindowAnimation(CanvasGroup cg)
        {
            cg.alpha = 0;
            cg.DOFade(1f, _fadeDuration);
        }

        public void HideWindowAnimation(CanvasGroup cg, WindowHandler close, WindowHandler open)
        {
            if (!_windowClosing)
            {
                cg.DOFade(0f, _fadeDuration).OnComplete(() => CloseHandle(close, open));
                _windowClosing = true;
            }
        }

        private void CloseHandle(WindowHandler close, WindowHandler open)
        {
            _windowClosing = false;
            _windowsController.SwitchWindows(close, open);
        }
    }
}