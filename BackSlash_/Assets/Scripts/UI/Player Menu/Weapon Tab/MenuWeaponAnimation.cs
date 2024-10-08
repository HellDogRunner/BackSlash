using UnityEngine;
using DG.Tweening;
using Scripts.Player;
using Zenject;
using RedMoonGames.Window;

public class MenuWeaponAnimation : MonoBehaviour
{
    [SerializeField] private GameObject _weaponProjection;

    [Header("Settings")]
    [SerializeField] private float _lookAtDuration = 1;
    [SerializeField] private float _lookAtOffsetX = 15;
    [SerializeField] private float _lookAtOffsetY = 30;

    private GameObject _weaponInst;
    [SerializeField] private Transform _weapon;

    private bool _isChoosing;

    private Tween _lookAtMouse;

    private UIActionsController _pauseInputs;
    private GameMenuController _menuController;

    [Inject]
    private void Construct(UIActionsController pauseInputs, GameMenuController menuController)
    {
        _pauseInputs = pauseInputs;
        _menuController = menuController;
    }

    private void Awake()
    {
        _pauseInputs.ShowCursor += AnimateLookAtMouse;

        SpawnWeapon();
    }

    private void AnimateLookAtMouse(bool isMouse)
    {
        if (isMouse && !_isChoosing && _weapon != null)
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

    private void SpawnWeapon()
    {
        _weaponInst = Instantiate(_weaponProjection);
        _weapon = _weaponInst.transform.Find("Weapon");
    }

    private void OnDestroy()
    {
        _pauseInputs.ShowCursor -= AnimateLookAtMouse;
        Destroy(_weaponInst);
    }
}
