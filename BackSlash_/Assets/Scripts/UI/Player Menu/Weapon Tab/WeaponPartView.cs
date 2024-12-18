using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class WeaponPartView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private MenuWeaponAnimation _weaponAnimation;
    [Space]
    [SerializeField] private RectTransform _object;
    [SerializeField] private RectTransform _parts;
    [SerializeField] private GameObject _pointer;

    [Header("Settings")]
    [SerializeField] private EItemType _partType;
    [SerializeField] private float _duration;
    [Space]
    [SerializeField] private Vector2 _defaultPosition;
    [SerializeField] private Vector2 _activePosition;
    [Space]
    [SerializeField] private Vector2 _partsOpenedSize;
    [SerializeField] private Vector2 _partsClosedSize;

    private bool _isActive;

    public event Action<bool, EItemType> OnViewActive;

    private void OnEnable()
    {
        _isActive = false;

        _object.anchoredPosition = _defaultPosition;

        _pointer.SetActive(false);

        _parts.gameObject.SetActive(false);
        _parts.sizeDelta = _partsClosedSize;
    }

    private void ShowPartsMenu()
    {
        _parts.gameObject.SetActive(true);

        _parts.DOSizeDelta(_partsOpenedSize, _duration).SetUpdate(true);
    }

    private void HidePartsMenu()
    {
        _parts.DOSizeDelta(_partsClosedSize, _duration).SetUpdate(true).OnComplete(() =>
        {
            _parts.gameObject.SetActive(false);
            _isActive = false;
        }) ;
    }

    private void MovePartsMenu(Vector2 position)
    {
        _object.DOAnchorPos(position, _duration).SetUpdate(true);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!_isActive) _pointer.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!_isActive) _pointer.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!_isActive)
        {
            _pointer.SetActive(false);
            _isActive = true;

            _weaponAnimation.SwitchChoosing(true);
            MovePartsMenu(_activePosition);
            ShowPartsMenu();
            OnViewActive?.Invoke(true, _partType);
        }
        else
        {
            _weaponAnimation.SwitchChoosing(false);
            MovePartsMenu(_defaultPosition);
            HidePartsMenu();
            OnViewActive?.Invoke(false, _partType);
        }
    }
}
