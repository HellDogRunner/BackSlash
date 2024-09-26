using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SceneTransitionAnimationService : MonoBehaviour
{
    [SerializeField] private SceneTransitionService _transitionService;

    [Header("Components")]
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private Image _fill;
    [SerializeField] private Image _glow;

    [Header("Animation Settings")]
    [SerializeField] private float _transitionDuration = 1f;
    [SerializeField] private float _fillDuration = 3f;

    public Sequence _loading;
    public Sequence _transition;

    public void AnimateLoading()
    {
        _loading = DOTween.Sequence();
        _loading.AppendCallback(() =>
        {
            _fill.DOFillAmount(1, _fillDuration).SetUpdate(true).SetEase(Ease.Flash);
            _glow.DOFillAmount(1, _fillDuration).SetUpdate(true).SetEase(Ease.Flash);
        });
        _loading.AppendInterval(_fillDuration + 0.05f);
        _loading.SetUpdate(true).OnComplete(() => 
        { 
            _transitionService.ChangeScene();
        });
    }

    public void AnimateOpening()
    {
        _canvasGroup.alpha = 1;
        _glow.fillAmount = 1;
        _fill.fillAmount = 1;

        _transition = DOTween.Sequence();
        _transition.AppendInterval(_transitionDuration);
        _transition.Append(_canvasGroup.DOFade(0, _transitionDuration));
        _transition.SetUpdate(true);
    }

    public void AnimateClosing()
    {
        _canvasGroup.alpha = 0;
        _glow.fillAmount = 0;
        _fill.fillAmount = 0;

        _transition = DOTween.Sequence();
        _transition.Append(_canvasGroup.DOFade(1, _transitionDuration));
        _transition.AppendInterval(_transitionDuration);
        _transition.SetUpdate(true).OnComplete(() => AnimateLoading());
    }

    private void OnDestroy()
    {
        if (_loading.IsActive()) _loading.Kill();
    }
}