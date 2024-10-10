using DG.Tweening;
using System.Collections;
using UnityEngine;

public class HUDAnimationService : MonoBehaviour
{
    [Header("Animation Settings")]
    [SerializeField] private float _hideDelay = 2f;
    [SerializeField] private float _fadeDuration = 0.5f;

    public void AnimateShow(CanvasGroup cg)
    {
        if (!cg.gameObject.activeSelf)
        {
            cg.gameObject.SetActive(true);
            cg.alpha = 0;
            cg.DOFade(1f, _fadeDuration).SetEase(Ease.InQuart);
        }
    }

    public void AnimateHide(CanvasGroup cg)
    {
        if (cg.gameObject.activeSelf)
        {
            cg.alpha = 1;
            cg.DOFade(0f, _fadeDuration).SetEase(Ease.InQuart).
                OnComplete(() => cg.gameObject.SetActive(false));
        }
    }

    public IEnumerator HideDelay(CanvasGroup cg)
    {
        yield return new WaitForSeconds(_hideDelay);
        AnimateHide(cg);
    }
}