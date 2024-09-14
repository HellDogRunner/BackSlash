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

    [Header("Mouse")]
    [SerializeField] private GameObject _mouse;
    [SerializeField] private GameObject _leftButton;
    [SerializeField] private GameObject _middleButton;
    [SerializeField] private GameObject _rightButton;

    [Header("Keyboard")]
    [SerializeField] private GameObject _keyboard;
    [SerializeField] private RectTransform _background;
    [SerializeField] private TMP_Text _text;

    [Header("Settings")]
    [SerializeField] private float _fillDuration = 0.5f;
    [SerializeField] private float _CancelDuration = 0.2f;
    [SerializeField] private float _blinkDuration = 0.5f;
    [SerializeField] private float _minBlinkFade = 0;
    [SerializeField] private float _maxBlinkFade = 1;
    [SerializeField] private float _blinkScale = 1.1f;

    [Header("Colors")]
    [SerializeField] private Color _comboContinueColor = new Color(31, 52, 73, 1);
    [SerializeField] private Color _comboCancelColor = new Color(192, 57, 43, 1);
    [SerializeField] private Color _comboFinishColor = new Color(17, 255, 255, 1);

    private float _fillValue;

    private Sequence _blink;
    private Sequence _finish;
    private Tween _fill;

    public void SetStartAnimations()
    {
        _blink.Kill();

        _blinkImage.transform.localScale = Vector3.one;
        _blinkImage.gameObject.SetActive(false);
        _blinkImage.DOColor(_comboContinueColor, _fillDuration);

        _cancelCG.gameObject.SetActive(false);
        _cancelCG.alpha = 0;

        _fillImage.fillAmount = 0;
    }

    public void AnimateProgressCombo()
    {
        FillIndicator();
        AnimateBlinkBackground();
    }

    public void FillIndicator()
    {
        _fill =  _fillImage.DOFillAmount(_fillImage.fillAmount + _fillValue, _fillDuration).SetEase(Ease.OutQuad);
    }

    public void SetFillVolume(int indicatorsCount)
    {
        _fillValue = 1f / indicatorsCount;
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
                    _leftButton.SetActive(true);
                    break;
                case 1:
                    _middleButton.SetActive(true);
                    break;
                case 2:
                    _rightButton.SetActive(true);
                    break;
            }

            _mouse.SetActive(true);
        }
        else
        {
            string keyText;
            _keys.KeysText.TryGetValue(actionBind, out keyText);

            _background.sizeDelta = _keys.Frames[keyText.Length - 1];
            _text.text = keyText;
            _keyboard.SetActive(true);
        }
    }

    private void HideAllKeys()
    {
        _leftButton.SetActive(false);
        _middleButton.SetActive(false);
        _rightButton.SetActive(false);
        _mouse.SetActive(false);
        _keyboard.SetActive(false);
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

        _fill.Kill();
        _fillImage.fillAmount = 0;

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