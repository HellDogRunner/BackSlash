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
        [SerializeField] private float _scaleDuration = 1f;
        [SerializeField] private float _fadeDuration = 0.5f;
        [SerializeField] private float _endBackgroundScale = 1.5f;

        private GameWindowsManager _windowsController;

        private Sequence _sequence;

        private CanvasGroup _backgroundCG;
        private Transform _backgroundTransform;

        [Inject]
        private void Construct(GameWindowsManager windowsController)
        {
            _windowsController = windowsController;

            _windowsController.OnHUDShow += ShowHUD;
            _windowsController.OnHUDHide += HideHUD;
        }

        private void Awake()
        {
            _backgroundCG = _background.GetComponent<CanvasGroup>();
            _backgroundTransform = _background.GetComponent<Transform>();
        }

        private void Start()
        {
            StartCoroutine(ShowHUDDelay());
        }

        private void ShowHUD()
        {
            _sequence = DOTween.Sequence();
            _sequence.AppendCallback(() =>
            {
                _backgroundCG.DOFade(1f, _fadeDuration).SetEase(Ease.InQuart).SetUpdate(true);
                _backgroundTransform.DOScale(1f, _scaleDuration).SetEase(Ease.OutSine).SetUpdate(true);
            }).SetUpdate(true);
        }

        private void HideHUD()
        {
            _sequence = DOTween.Sequence();
            _sequence.AppendCallback(() =>
            {
                _backgroundCG.DOFade(0f, _fadeDuration).SetEase(Ease.OutQuart).SetUpdate(true);
                _backgroundTransform.DOScale(_endBackgroundScale, _scaleDuration).SetEase(Ease.InSine).SetUpdate(true);
            }).SetUpdate(true);
        }

        IEnumerator ShowHUDDelay()
        {
            yield return new WaitForSeconds(3f);
            ShowHUD();
        }

        protected virtual void OnDestroy()
        {
            _windowsController.OnHUDShow -= ShowHUD;
            _windowsController.OnHUDHide -= HideHUD;
        }
    }
}