using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SkillAnimationService : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private RectTransform _skillRect;
    [SerializeField] private CanvasGroup _cooldownCG;
    [SerializeField] private Image _cooldownImage;
    [SerializeField] private Image _readyIndicator;

    [Header("Settings")]
    [SerializeField] private float _cooldownFade = 0.15f;
    [SerializeField] private float _fillDuration = 3;
    [SerializeField] private float _readyScale = 8;
    [SerializeField] private float _readyDuration = 0.15f;
    [SerializeField] private float _reloadingRate = 0.1f;
    [SerializeField] private int _reloadingLoops = 3;

    [Header("Colors")]
    [SerializeField] private Color _notReady = new Color(192, 57, 43, 255);

    private Sequence _ready;
    private Sequence _reloading;

    private Tween _cooldown;

    private void Awake()
    {
        _cooldownImage.fillAmount = 0;
        _cooldownCG.alpha = _cooldownFade;
    }

    public void AnimateCooldown()
    {
        _reloading.Kill();

        _cooldown = _cooldownImage.DOFillAmount(1, _fillDuration).SetEase(Ease.Flash).OnComplete(AnimateSkillReady);
    }

    public void AnimateSkillReady()
    {
        _cooldownCG.alpha = 0;
        _cooldownImage.fillAmount = 0;
        _cooldownCG.alpha = _cooldownFade;

        _ready = DOTween.Sequence();
        _ready.Append(_readyIndicator.transform.DOScaleY(_readyScale, _readyDuration).SetEase(Ease.OutQuart));
        _ready.AppendCallback(() =>
        {
            _readyIndicator.transform.localScale = Vector3.one;
        });
    }

    public void AnimateReloading()
    {
        _reloading = DOTween.Sequence();
        _reloading.AppendCallback(() => 
        {
            _cooldownCG.DOFade(1, _reloadingRate);
            _cooldownImage.DOColor(_notReady, _reloadingRate).SetEase(Ease.Flash);
        });
        _reloading.AppendInterval(_reloadingRate);
        _reloading.AppendCallback(() =>
        {
            _cooldownCG.DOFade(_cooldownFade, _reloadingRate);
            _cooldownImage.DOColor(Color.white, _reloadingRate).SetEase(Ease.Flash);
        });
        _reloading.AppendInterval(_reloadingRate);
        _reloading.SetLoops(_reloadingLoops);

        //_reloading.Append(_cooldownImage.DOColor(_notReady, _reloadingRate).SetEase(Ease.Flash));
        //_reloading.Append(_cooldownImage.DOColor(Color.white, _reloadingRate).SetEase(Ease.Flash));
    }
}