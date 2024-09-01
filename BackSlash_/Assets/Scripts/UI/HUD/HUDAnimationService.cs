using DG.Tweening;
using System.Collections;
using UnityEngine;
using Zenject;

namespace RedMoonGames.Window
{
    public class HUDAnimationService : MonoBehaviour
    {
        [SerializeField] private GameObject _player;
        [Header("Animation Components")]
        [SerializeField] private CanvasGroup _canvasGroup;

        [Header("Animation Settings")]
        [SerializeField] private float _hideDelay = 2f;
        [SerializeField] private float _fadeDuration = 0.5f;

        private GameWindowsController _windowsController;
        private HealthController _healthController;

        [Inject]
        private void Construct(GameWindowsController windowsController)
        {
            _windowsController = windowsController;
            _windowsController.OnHUDShow += SetUnpause;
            _windowsController.OnHUDHide += SetPause;

            _healthController = _player.GetComponent<HealthController>();
            _healthController.OnHealthChanged += ShowHUD;
        }

        private void Awake()
        {
            _canvasGroup.alpha = 0;
        }

        private void ShowHUD(float health)
        {
            _canvasGroup.DOFade(1f, _fadeDuration).SetEase(Ease.InQuart);
            StartCoroutine(HideDelay());
        }

        private void HideHUD()
        {
            _canvasGroup.DOFade(0f, _fadeDuration).SetEase(Ease.InQuart);
        }

        IEnumerator HideDelay()
        {
            yield return new WaitForSeconds(_hideDelay);
            HideHUD();
        }

        private void SetPause()
        {
            _canvasGroup.gameObject.SetActive(false);
        }

        private void SetUnpause()
        {
            _canvasGroup.gameObject.SetActive(true);
        }

        protected virtual void OnDestroy()
        {
            _windowsController.OnHUDShow -= SetUnpause;
            _windowsController.OnHUDHide -= SetPause;

            _healthController.OnHealthChanged -= ShowHUD;
        }
    }
}