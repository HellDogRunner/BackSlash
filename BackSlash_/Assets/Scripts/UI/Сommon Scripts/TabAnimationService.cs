using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Selectable))]
public class TabAnimationService : MonoBehaviour, IPointerEnterHandler, IDeselectHandler
{
    [SerializeField] private bool _selectedOnEnable;

    [Header("Animation Components")]
    [SerializeField] private RectTransform _rect;
    [SerializeField] private Image _image;
    [SerializeField] private CanvasGroup _backgroundCG;
    [SerializeField] private TMP_Text _text;

    [Header("Animation Settings")]
    [SerializeField] private float _fadeDuration = 0.35f;
    [SerializeField] private Vector2 _minEndAnchor = new Vector2(0.05f, 0);
    [SerializeField] private Vector2 _maxEndAnchor = new Vector2(0.95f, 0.05f);
    [SerializeField] private Vector2 _minStartAnchor = new Vector2(0.5f, 0);
    [SerializeField] private Vector2 _maxStartAnchor = new Vector2(0.5f, 0.05f);
    [SerializeField] private Color _deselectColor = new Color(0.745f, 0.745f, 0.745f, 1);
    [SerializeField] private Color _selectColor = Color.white;
    [SerializeField] private Color _activeColor = new Color(0.066f, 1, 1, 1);

    private List<Tween> _tweens = new List<Tween>();
    private bool _isActive;

    private void Awake()
    {
        _rect.anchorMin = _minStartAnchor;
        _rect.anchorMax = _maxStartAnchor;
        _backgroundCG.alpha = 0f;

        if (_selectedOnEnable)
        {
            EnableTab();
        }

        GetComponent<Selectable>().OnSelectAsObservable().Subscribe(_ => { SelectTab(); }).AddTo(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SelectTab();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        DeselectTab();
    }

    private void SelectTab()
    {
        if (!_isActive)
        {
            _text.color = _selectColor;
            _image.color = _selectColor;
            _tweens.Add(_rect.DOAnchorMin(_minEndAnchor, _fadeDuration).SetUpdate(true));
            _tweens.Add(_rect.DOAnchorMax(_maxEndAnchor, _fadeDuration).SetUpdate(true));
        }
    }

    private void DeselectTab()
    {
        if (!_isActive)
        {
            _text.color = _deselectColor;
            _image.color = _deselectColor;
            _tweens.Add(_rect.DOAnchorMin(_minStartAnchor, _fadeDuration).SetUpdate(true));
            _tweens.Add(_rect.DOAnchorMax(_maxStartAnchor, _fadeDuration).SetUpdate(true));
        }
    }

    private void EnableTab()
    {
        SelectTab();

        _isActive = true;

        _text.color = _activeColor;
        _image.color = _activeColor;
        _backgroundCG.alpha = 1;
    }

    private void DisableTab()
    {
        KillTweens();

        _isActive = false;

        _rect.anchorMin = _minStartAnchor;
        _rect.anchorMax = _maxStartAnchor;
        _text.color = _deselectColor;
        _image.color = _deselectColor;
        _backgroundCG.alpha = 0;
    }

    public void SwitchTab()
    {
        if (_isActive)
        {
            DisableTab();
        }
        else
        {
            EnableTab();
        }
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

    private void OnDestroy()
    {
        KillTweens();
    }
}
