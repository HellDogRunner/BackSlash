using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ComboAnimationService : MonoBehaviour
{
    [SerializeField] private KeysStash _keys;

    [Header("Components")]
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private CanvasGroup _cancelCG;
    [SerializeField] private Image _fillImage;
    [SerializeField] private Image _blinkImage;
    [SerializeField] private Image _flash;

    [Header("Settings")]
    [SerializeField] private float _fillDuration = 0.5f;
    [SerializeField] private float _CancelDuration = 0.2f;
    [SerializeField] private float _blinkDuration = 0.5f;
    [SerializeField] private float _minBlinkFade = 0;
    [SerializeField] private float _maxBlinkFade = 1;
    [SerializeField] private float _blinkScale = 1.1f;

    [Header("Keys")]
    [SerializeField] private GameObject _mouseObject;
    [SerializeField] private GameObject _leftMouseButton;
    [SerializeField] private GameObject _middleMouseButton;
    [SerializeField] private GameObject _rightMouseButton;
    [Space]
    [SerializeField] private GameObject _keyboardObject;
    [SerializeField] private RectTransform _frameRT;
    [SerializeField] private TMP_Text _keyText;

    [Header("Colors")]
    [SerializeField] private Color _comboContinueColor = new Color(31, 52, 73, 1);
    [SerializeField] private Color _comboCancelColor = new Color(192, 57, 43, 1);
    [SerializeField] private Color _comboFinishColor = new Color(17, 255, 255, 1);

    private float _fill;

    private Sequence _blink;
    private Sequence _finish;

    public void SetStartAnimations()
    {
        _blink.Kill();

        _blinkImage.transform.localScale = Vector3.one;
        _blinkImage.gameObject.SetActive(false);
        _blinkImage.DOColor(_comboContinueColor, _fillDuration);

        _cancelCG.gameObject.SetActive(false);
        _cancelCG.alpha = 0;

        FillIndicator(0);
    }

    public void AnimateProgressCombo()
    {
        FillIndicator(_fill);
        AnimateBlinkBackground();
    }

    public void FillIndicator(float value)
    {
        if (value != 0)
        {
            value = _fillImage.fillAmount + _fill;
        }

        _fillImage.DOFillAmount(value, _fillDuration).SetEase(Ease.OutQuad);
    }

    public void SetFillVolume(int indicatorsCount)
    {
        _fill = 1f / indicatorsCount;
    }

    public void ShowNextComboKey(InputAction action)
    {
        HideAllKeys();

        string actionBind = action.GetBindingDisplayString(0);
        List<string> mouseBinds = new List<string>();
        _keys.KeysInputs.TryGetValue("Mouse", out mouseBinds);

        if (mouseBinds.Contains(actionBind))
        {
            int index;
            _keys.MouseButtons.TryGetValue(actionBind, out index);

            switch (index)
            {
                case 0:
                    _leftMouseButton.SetActive(true);
                    break;
                case 1:
                    _middleMouseButton.SetActive(true);
                    break;
                case 2:
                    _rightMouseButton.SetActive(true);
                    break;
            }

            _mouseObject.SetActive(true);
        }
        else
        {
            string keyText;
            _keys.KeysText.TryGetValue(actionBind, out keyText);

            _frameRT.sizeDelta = _keys.Frames[keyText.Length - 1];
            _keyText.text = keyText;
            _keyboardObject.SetActive(true);
        }
    }

    private void HideAllKeys()
    {
        _leftMouseButton.SetActive(false);
        _middleMouseButton.SetActive(false);
        _rightMouseButton.SetActive(false);
        _mouseObject.SetActive(false);
        _keyboardObject.SetActive(false);
    }

    public void AnimateBlinkBackground()
    {
        if (!_blink.IsActive())
        {
            _blinkImage.gameObject.SetActive(true);

            _blink = DOTween.Sequence();
            _blink.Append(_blinkImage.DOFade(_minBlinkFade, _blinkDuration));
            _blink.Append(_blinkImage.DOFade(_maxBlinkFade, _blinkDuration));
            _blink.SetLoops(-1);
        }
    }

    public void AnimateCancelCombo()
    {
        AnimateBlinkBackground();
        HideAllKeys();
        FillIndicator(0);

        _blinkImage.DOColor(_comboCancelColor, _fillDuration);
        _cancelCG.gameObject.SetActive(true);

        Sequence sequence = DOTween.Sequence();
        sequence.AppendCallback(() =>
        {
            _cancelCG.DOFade(1, _CancelDuration);
            _cancelCG.transform.DOScale(1.5f, _CancelDuration);
        });
        sequence.AppendInterval(_CancelDuration);
        sequence.Append(_cancelCG.transform.DOScale(1f, _CancelDuration));
    }

    public void AnimateFinishCombo()
    {
        HideAllKeys();

        _blinkImage.DOColor(_comboFinishColor, _fillDuration);

        _finish = DOTween.Sequence();
        _finish.Append(_blinkImage.transform.DOScale(_blinkScale, _blinkDuration));
        _finish.Append(_blinkImage.transform.DOScale(0.8f, _blinkDuration)).OnComplete(() => _blinkImage.gameObject.SetActive(false));
    }
}