using UniRx;
using UniRx.Triggers;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

[RequireComponent(typeof(Selectable))]
public class ButtonAnimationService : MonoBehaviour, IPointerEnterHandler, IDeselectHandler
{
    [SerializeField] private bool _selectedOnEnable;

    [Header("Animation Components")]
    [SerializeField] private GameObject _frame;
    [SerializeField] private GameObject _indicator;
    [SerializeField] private GameObject _point;
    [SerializeField] private TMP_Text _text;

    [Header("Animation Settings")]
    [SerializeField] private float _fadeDuration = 0.35f;
    [SerializeField] private Vector2 _minStartAnchor = new Vector2(0.3f, 0);
    [SerializeField] private Vector2 _maxStartAnchor = new Vector2(0.7f, 1);
    [SerializeField] private Color _textSelectedColor;
    [SerializeField] private Color _textDeselectedColor;
    [Header("Indicator Animation")]
    [SerializeField] private Vector2 _punch = new Vector2(-15f, 0);
    [SerializeField] private int _vibrato = 12;

    private Vector2 _pointPosition =new Vector2(-20, 0);
    private Button _button;
    private Image _image;
    private RectTransform _frameRect;
    private RectTransform _pointRect;
    private CanvasGroup _frameCG;
    private CanvasGroup _indicatorCG;

    [SerializeField] private List<Tween> _tweens = new List<Tween>();

    private void Awake()
    {
        _button = GetComponent<Button>();
        _image = _frame.GetComponent<Image>();
        _frameRect = _frame.GetComponent<RectTransform>();
        _pointRect = _point.GetComponent<RectTransform>();
        _frameCG = _frame.GetComponent<CanvasGroup>();
        _indicatorCG = _indicator.GetComponent<CanvasGroup>();

        _frameCG.alpha = 0;
        _indicatorCG.alpha = 0;

        if (_selectedOnEnable)
        {
            ShowButton();
        }

        _button.OnSelectAsObservable().Subscribe(_ => { ShowButton(); }).AddTo(this);
    }

    private void OnDestroy()
    {
        KillTweens();
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        ShowButton();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        HideButton();
    }

    private void ShowButton()
    {
        _text.color = _textSelectedColor;
        _indicatorCG.alpha = 1;

        if (_frameCG.alpha == 0)
        {
            _pointRect.localPosition = _pointPosition;
            _tweens.Add(_frameCG.DOFade(1, _fadeDuration).SetUpdate(true));
            _tweens.Add(_frameRect.DOAnchorMin(Vector2.zero, _fadeDuration).SetUpdate(true));
            _tweens.Add(_frameRect.DOAnchorMax(Vector2.one, _fadeDuration).SetUpdate(true));
            _tweens.Add(_pointRect.DOPunchAnchorPos(_punch, _fadeDuration, _vibrato, 0, true).SetUpdate(true));
        }
    }

    private void HideButton()
    {
        _text.color = _textDeselectedColor;
        _frameCG.alpha = 0;
        _indicatorCG.alpha = 0;

        KillTweens();

        _frameRect.anchorMin = _minStartAnchor;
        _frameRect.anchorMax = _maxStartAnchor;
    }

    private void KillTweens()
    {
        foreach (var tween in _tweens)
        {
            if (tween.IsActive())
            {
                tween.Kill();
            }
        }
        _tweens.Clear();
    }
}
