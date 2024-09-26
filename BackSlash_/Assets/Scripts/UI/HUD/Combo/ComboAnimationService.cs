using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ComboAnimationService : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private GameObject _indicator;
    [SerializeField] private RectTransform _cross;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private CanvasGroup _cancelCG;
    [SerializeField] private CanvasGroup _finishFrameCG;
    [SerializeField] private Image _fill;

    [Header("Mouse")]
    [SerializeField] private GameObject _mouse;
    [SerializeField] private GameObject _leftButton;
    [SerializeField] private GameObject _middleButton;
    [SerializeField] private GameObject _rightButton;

    [Header("Keyboard")]
    [SerializeField] private GameObject _keyboard;
    [SerializeField] private TMP_Text _text;

    [Header("Settings")]
    [SerializeField] private float _cancelScale = 1.5f;
    [SerializeField] private float _cancelDuration = 0.2f;

    [SerializeField] private float _InactiveFade = 0.3f;

    [SerializeField] private float _finishFade = 0.5f;
    [SerializeField] private float _finishDuration = 0.2f;

    private float _fillValue;

    private Sequence _cancel;
    private Tween _finish;
    private Tween _inactive;

    public void SetStartAnimations()
    {
        KillAnimations();

        _canvasGroup.alpha = 1f;

        _indicator.SetActive(true);
        _fill.fillAmount = 0;

        _cancelCG.gameObject.SetActive(false);
        _cancelCG.alpha = 0;

        _cross.localScale = Vector3.one;

        _finishFrameCG.gameObject.SetActive(false);
        _finishFrameCG.alpha = 0;
    }

    public void FillIndicator()
    {
        _fill.fillAmount += _fillValue;
    }

    public void SetFillVolume(int indicatorsCount)
    {
        _fillValue = 1f / indicatorsCount;
    }

    public void ShowNextComboKey(InputAction action)
    {
        HideAllKeys();

        string actionBind = action.GetBindingDisplayString(0);

        if (action.bindings[0].path.Contains("Mouse"))
        {
            ShowMouseButton(actionBind);
        }
        else
        {
            _text.text = actionBind;
            _keyboard.SetActive(true);
        }
    }

    private void ShowMouseButton(string actionBind)
    {
        switch (actionBind)
        {
            case "LMB":
                _leftButton.SetActive(true);
                break;
            case "MMB":
                _middleButton.SetActive(true);
                break;
            case "RMB":
                _rightButton.SetActive(true);
                break;
        }

        _mouse.SetActive(true);
    }

    private void HideAllKeys()
    {
        _leftButton.SetActive(false);
        _middleButton.SetActive(false);
        _rightButton.SetActive(false);
        _mouse.SetActive(false);
        _keyboard.SetActive(false);
    }

    public void AnimateCancelCombo()
    {
        HideAllKeys();

        _cancelCG.gameObject.SetActive(true);
        _indicator.SetActive(false);

        _cancel = DOTween.Sequence();
        _cancel.AppendCallback(() =>
        {
            _cancelCG.DOFade(1, _cancelDuration / 2).SetEase(Ease.Flash);
            _cross.DOScale(_cancelScale, _cancelDuration);
        });
        _cancel.AppendInterval(_cancelDuration);
        _cancel.Append(_cross.DOScale(1f, _cancelDuration));
        _cancel.AppendInterval(_cancelDuration).OnComplete(() => AnimateInactive());
    }

    private void AnimateInactive()
    {
        _cancelCG.gameObject.SetActive(false);

        _inactive = _canvasGroup.DOFade(_InactiveFade, _cancelDuration).SetEase(Ease.Flash);
    }

    public void AnimateFinishCombo()
    {
        HideAllKeys();

        _indicator.SetActive(false);
        _finishFrameCG.gameObject.SetActive(true);

        _finish = _finishFrameCG.DOFade(_finishFade, _finishDuration).SetEase(Ease.Flash);
    }

    private void KillAnimations()
    {
        if (_cancel != null) _cancel.Kill();
        if (_finish != null) _finish.Kill();
        if (_inactive != null) _inactive.Kill();
    }

    private void OnDestroy()
    {
        KillAnimations();
    }
}