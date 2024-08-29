using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SceneTransitionAnimationService : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private Image _loadingImage;
    [SerializeField] private TMP_Text _loadingText;

    [Header("Animation Settings")]
    [SerializeField] private float _fadeDuration = 1f;
    [SerializeField] private float _loadingImageDuration = 1f;

    public Sequence _sequence;

    private bool _isClockwise;

    private SceneTransitionService _transitionManager;

    private void Awake()
    {
        _transitionManager = gameObject.GetComponent<SceneTransitionService>();

        _isClockwise = true;
    }

    public void PlayOpeningAnimation()
    {
        _canvasGroup.alpha = 1f;
        _loadingText.alpha = 1f;

        _sequence = DOTween.Sequence();
        _sequence.AppendCallback(() =>
        {
            _loadingText.DOFade(0f, _fadeDuration);
            _loadingImage.DOFade(0f, _fadeDuration);
        });
        _sequence.AppendInterval(_fadeDuration);
        _sequence.Append(_canvasGroup.DOFade(0f, _fadeDuration).SetEase(Ease.InExpo));
    }

    public void PlayClosingAnimation()
    {
        _canvasGroup.alpha = 0f;
        _loadingText.alpha = 0f;

        _sequence = DOTween.Sequence();
        _sequence.Append(_canvasGroup.DOFade(1f, _fadeDuration)).SetEase(Ease.OutExpo).SetUpdate(true);
        _sequence.AppendInterval(_fadeDuration);
        _sequence.AppendCallback(() =>
        {
            _loadingText.DOFade(1f, _fadeDuration).SetUpdate(true);
            _loadingImage.DOFade(1f, _fadeDuration).SetUpdate(true).OnComplete(LoadingAnimation);
        });
    }

    private void LoadingAnimation()
    {
        _sequence = DOTween.Sequence();
        _sequence.Append(_loadingImage.DOFillAmount(1f, _loadingImageDuration)).SetEase(Ease.Flash).SetUpdate(true);
        _sequence.AppendCallback(LoadingImageRotate).SetUpdate(true);
        _sequence.Append(_loadingImage.DOFillAmount(0f, _loadingImageDuration)).SetEase(Ease.Flash).SetUpdate(true);
        _sequence.AppendCallback(LoadingImageRotate).SetUpdate(true);
        _sequence.AppendCallback(_transitionManager.CheckSceneLoaded).SetUpdate(true);
        _sequence.SetLoops(-1);
    }

    private void LoadingImageRotate()
    {
        if (_isClockwise)
        {
            _isClockwise = false;
        }
        else
        {
            _isClockwise = true;
        }
        _loadingImage.fillClockwise = _isClockwise;
    }

    public void KillSequense()
    {
        _sequence.Kill();
    }
}
