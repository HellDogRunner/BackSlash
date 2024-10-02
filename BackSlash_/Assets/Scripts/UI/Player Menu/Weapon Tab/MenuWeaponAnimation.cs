using UnityEngine;
using DG.Tweening;
using Scripts.Player;
using Zenject;

public class MenuWeaponAnimation : MonoBehaviour
{
    [SerializeField] private Transform _weapon;

    [Header("Settings")]
    [SerializeField] private float _lookAtDuration = 1;
    [SerializeField] private float _lookAtOffsetX = 15;
    [SerializeField] private float _lookAtOffsetY = 30;

    private bool _isChoosing;

    private Tween _lookAtMouse;

    private UIMenuInputs _menuInputs;

    [Inject]
    private void Construct(UIMenuInputs menuInputs)
    {
        _menuInputs = menuInputs;
    }

    private void AnimateLookAtMouse(bool isMouse)
    {
        if (isMouse && !_isChoosing)
        {
            _lookAtMouse = _weapon.DOLocalRotate(GetWeaponRotation(), _lookAtDuration).SetUpdate(true).SetEase(Ease.OutSine);
        }
    }

    private Vector3 GetWeaponRotation()
    {
        Resolution resolution = Screen.currentResolution;
        var mousePos = Input.mousePosition;
        Vector3 weaponRotation;

        float xRotation = _lookAtOffsetY / 2 - mousePos.y / (resolution.height / _lookAtOffsetY);
        float yRotation = mousePos.x / (resolution.width / _lookAtOffsetX) - _lookAtOffsetX / 2;

        weaponRotation = new Vector3(xRotation, yRotation, 0);

        return weaponRotation;
    }

    public void SwitchChoosing(bool _)
    {
        _isChoosing = _;
    }

    private void OnEnable()
    {
        _menuInputs.OnHideCursor += AnimateLookAtMouse;
    }

    private void OnDisable()
    {
        _menuInputs.OnHideCursor -= AnimateLookAtMouse;
    }

}
