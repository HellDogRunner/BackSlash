using DG.Tweening;
using UnityEngine;

public class PlayerMenuAnimationService : MonoBehaviour
{
    [Header("Animation Settings")]
    [SerializeField] private float _fadeDuration = 0.1f;

    private bool _windowClosing;

    public void AnimateMenuShow(CanvasGroup cg)
    {
        cg.alpha = 0f;
        cg.DOFade(1f, _fadeDuration).SetEase(Ease.InOutSine).SetUpdate(true);
    }

    public void AnimateMenuHide(CanvasGroup cg)
    {
        if (!_windowClosing)
        {
            _windowClosing = true;
            cg.DOFade(0f, _fadeDuration).SetEase(Ease.InOutSine).SetUpdate(true).OnComplete(() => _windowClosing = false);
        }
    }
}
