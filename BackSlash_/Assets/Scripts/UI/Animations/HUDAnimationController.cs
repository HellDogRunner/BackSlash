using DG.Tweening;
using System.Collections;
using UnityEngine;
using Zenject;

namespace RedMoonGames.Window
{
    public class HUDAnimationController : MonoBehaviour
    {
        [Header("Animation Components")]
        [SerializeField] private GameObject _background;

        [Header("Animation Settings")]
        [SerializeField] private float _showDelay = 2f;
        [SerializeField] private float _scaleDuration = 1f;
        [SerializeField] private float _fadeDuration = 0.5f;
        [SerializeField] private float _endBackgroundScale = 1.5f;

        private GameWindowsController _windowsController;

        private Sequence _sequence;

        private CanvasGroup _canvasGroup;
        private RectTransform _rect;

        [Inject]
        private void Construct(GameWindowsController windowsController)
        {
            _windowsController = windowsController;

            _windowsController.OnHUDShow += ShowHUD;
            _windowsController.OnHUDHide += HideHUD;

            _canvasGroup = _background.GetComponent<CanvasGroup>();
            _rect = _background.GetComponent<RectTransform>();
        }

        private void Start()
        {
            _canvasGroup.alpha = 0;
            _rect.localScale = new Vector3(_endBackgroundScale, _endBackgroundScale, _endBackgroundScale);
            StartCoroutine(HUDDelay());
        }

        private void ShowHUD()
        {
            _canvasGroup.alpha = 0;
            _canvasGroup.DOFade(1f, _fadeDuration).SetEase(Ease.InQuart).SetUpdate(true);
            _rect.DOScale(1f, _scaleDuration).SetEase(Ease.OutSine).SetUpdate(true);
        }

        private void HideHUD()
        {
            _canvasGroup.DOFade(0f, _fadeDuration).SetEase(Ease.OutQuart).SetUpdate(true);
            _rect.DOScale(_endBackgroundScale, _scaleDuration).SetEase(Ease.InSine).SetUpdate(true);
        }

        IEnumerator HUDDelay()
        {
            yield return new WaitForSeconds(_showDelay);
            ShowHUD();
        }

        protected virtual void OnDestroy()
        {
            _windowsController.OnHUDShow -= ShowHUD;
            _windowsController.OnHUDHide -= HideHUD;
        }
    }
}