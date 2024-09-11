using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Selectable))]
public class MenuTabAnimationService : MonoBehaviour, IPointerEnterHandler, IDeselectHandler
{
    [Header("Animation Components")]
    [SerializeField] private CanvasGroup _frameCG;
    [SerializeField] private CanvasGroup _selectCG;
    [SerializeField] private CanvasGroup _glowCG;
    [SerializeField] private TMP_Text _text;

    [Header("Animation Settings")]
    [SerializeField] private float _selectDuration = 0.35f;
    [SerializeField] private float _scale = 1.1f;
    [SerializeField] private float _decreaseScale = 0.7f;
    [SerializeField] private float _deselectAlpha = 0.25f;
    [SerializeField] private float _selectAlpha = 0.7f;
    [SerializeField] private float _activeAlpha = 1;
    [SerializeField] private float _selectBGAlpha = 0.05f;
    [SerializeField] private float _glowAlpha = 0.05f;

    private List<Tween> _tweens = new List<Tween>();
    private bool _isActive;

    private void Awake()
    {
        SetDefault();

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
            _tweens.Add(_text.DOFade(_selectAlpha, _selectDuration).SetUpdate(true));
            _tweens.Add(_selectCG.DOFade(_selectBGAlpha, _selectDuration).SetUpdate(true));
            _tweens.Add(_selectCG.transform.DOScale(_scale, _selectDuration).SetUpdate(true));
        }
    }

    private void DeselectTab()
    {
        if (!_isActive)
        {
            _tweens.Add(_text.DOFade(_deselectAlpha, _selectDuration).SetUpdate(true));
            _tweens.Add(_selectCG.DOFade(0, _selectDuration).SetUpdate(true));
            _tweens.Add(_selectCG.transform.DOScale(1, _selectDuration).SetUpdate(true));
        }
    }

    public void EnableTab()
    {
        KillTweens();
        _isActive = true;

        _selectCG.alpha = _selectBGAlpha;

        _tweens.Add(_glowCG.DOFade(_glowAlpha, _selectDuration).SetUpdate(true));
        _tweens.Add(_selectCG.DOFade(0, _selectDuration).SetUpdate(true));
        _tweens.Add(_selectCG.transform.DOScale(_decreaseScale, _selectDuration).SetUpdate(true));
        _tweens.Add(_text.DOFade(_activeAlpha, _selectDuration).SetUpdate(true));
        _tweens.Add(_text.transform.DOScale(_scale, _selectDuration).SetUpdate(true));
        _tweens.Add(_frameCG.DOFade(1, _selectDuration).SetUpdate(true));
        _tweens.Add(_frameCG.transform.DOScale(_scale, _selectDuration).SetUpdate(true));
    }

    public void DisableTab()
    {
        KillTweens();
        SetDefault();
    }

    private void SetDefault()
    {
        _isActive = false;

        _text.alpha = _deselectAlpha;
        _text.transform.localScale = Vector3.one;

        _frameCG.alpha = 0;
        _frameCG.transform.localScale = Vector3.one;

        _selectCG.alpha = 0;
        _selectCG.transform.localScale = Vector3.one;

        _glowCG.alpha = 0;
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