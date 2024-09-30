using DG.Tweening;
using System.Collections;
using UnityEngine;

namespace RedMoonGames.Window
{
    public class HUDAnimationService : MonoBehaviour
    {
        [SerializeField] private HealthController _playerHealth;
        [Header("Animation Components")]
        [SerializeField] private CanvasGroup _canvasGroup;

        [Header("Animation Settings")]
        [SerializeField] private float _hideDelay = 2f;
        [SerializeField] private float _fadeDuration = 0.5f;

        private void Awake()
        {
            _playerHealth.OnHealthChanged += ShowHUD;
            _canvasGroup.gameObject.SetActive(true);
            //_canvasGroup.alpha = 0;
        }

        private void ShowHUD(float health)
        {
            _canvasGroup.DOFade(1f, _fadeDuration).SetEase(Ease.InQuart);
            //StartCoroutine(HideDelay());
        }

        private void HideHUD()
        {
            _canvasGroup.DOFade(0f, _fadeDuration).SetEase(Ease.InQuart);
        }

        private IEnumerator HideDelay()
        {
            yield return new WaitForSeconds(_hideDelay);
            HideHUD();
        }

        public void HideOnPause()
        {
            _canvasGroup.gameObject.SetActive(false);
        }

        public void ShowOnUnpause()
        {
            _canvasGroup.gameObject.SetActive(true);
        }

        private void OnDestroy()
        {
            _playerHealth.OnHealthChanged -= ShowHUD;
        }
    }
}