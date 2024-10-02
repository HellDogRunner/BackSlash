using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class WeaponPartAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private RectTransform _object;
    [SerializeField] private RectTransform _parts;
    [SerializeField] private GameObject _pointer;

    [Header("Settings")]
    [SerializeField] private float _duration;
    [Space]
    [SerializeField] private Vector2 _defaultPosition;
    [SerializeField] private Vector2 _activePosition;
    [Space]
    [SerializeField] private Vector2 _partsOpenedSize;
    [SerializeField] private Vector2 _partsClosedSize;

    private bool _isActive;

    private MenuWeaponAnimation _weaponAnimation;

    [Inject]
    private void Construct(MenuWeaponAnimation weaponAnimation)
    {
        _weaponAnimation = weaponAnimation;
    }

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
        }
        else
        {
            _weaponAnimation.SwitchChoosing(false);
            MovePartsMenu(_defaultPosition);
            HidePartsMenu();
        }
    }
}
