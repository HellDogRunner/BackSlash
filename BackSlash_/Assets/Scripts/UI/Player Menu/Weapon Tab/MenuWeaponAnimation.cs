using DG.Tweening;
using Scripts.Player;
using UnityEngine;
using Zenject;

public class MenuWeaponAnimation : MonoBehaviour
{
    [SerializeField] private GameObject _weaponProjection;

    [Header("Settings")]
    [SerializeField] private float _lookAtDuration = 1;
    [SerializeField] private float _lookAtOffsetX = 15;
    [SerializeField] private float _lookAtOffsetY = 30;

    private GameObject _spawnedWeapon;
    private Transform _weapon;

    private bool _isChoosing;

    private Tween _weaponTween;

    private UiInputsController _pauseInputs;

    [Inject]
    private void Construct(UiInputsController pauseInputs)
    {
        _pauseInputs = pauseInputs;
    }

    private void Awake()
    {
        SpawnWeapon();
        _pauseInputs.ShowCursor += AnimateLookAtMouse;
    }

    private void AnimateLookAtMouse(bool isMouse)
    {
        if (isMouse && !_isChoosing && _weapon != null)
        {
            if (_weaponTween.IsActive()) _weaponTween.Kill();
            _weaponTween = _weapon.DOLocalRotate(GetWeaponRotation(), _lookAtDuration).SetUpdate(true).SetEase(Ease.OutSine);
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

    private void SpawnWeapon()
    {
        _spawnedWeapon = Instantiate(_weaponProjection);
        _weapon = _spawnedWeapon.transform.Find("Weapon");
    }

    private void OnDestroy()
    {
        if (_weaponTween.IsActive()) _weaponTween.Kill();
        Destroy(_spawnedWeapon);
        _pauseInputs.ShowCursor -= AnimateLookAtMouse;
    }
}