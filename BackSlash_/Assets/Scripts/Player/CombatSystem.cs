using Scripts.Player;
using Scripts.Weapon;
using UnityEngine;
using Zenject;
using System;

public class CombatSystem : MonoBehaviour
{
    private InputController _inputController;
    private WeaponController _weaponController;
    private ComboSystem _comboSystem;

    public event Action OnPrimaryAttack;
    public event Action OnComboAttack;
    public event Action OnJumpComboAttack;
    public event Action<bool> IsBlocking;
    public event Action<bool> IsAttacking;

    [Inject]
    private void Construct(InputController inputController, WeaponController weaponController, ComboSystem comboSystem)
    {
        _inputController = inputController;
        _weaponController = weaponController;
        _comboSystem = comboSystem;

    }

    private void Update()
    {
    }

    private void OnLightAttack(bool isAttaking)
    {
        if (_weaponController.CurrentWeaponType != EWeaponType.None && isAttaking)
        {
            OnPrimaryAttack?.Invoke();
            IsAttacking?.Invoke(true);
        }
    }


    private void ResetCounter()
    {
        IsAttacking?.Invoke(false);
    }

    private void OnSpecial_1()
    {
        if (_weaponController.CurrentWeaponType != EWeaponType.None)
        {
            OnComboAttack?.Invoke();

            Debug.Log("light combo");
        }
    }

    private void OnSpecial_3()
    {
        if (_weaponController.CurrentWeaponType != EWeaponType.None)
        {
            OnJumpComboAttack?.Invoke();

            Debug.Log("Jump combo");
        }
    }

    private void Block() 
    {
        if (_weaponController.CurrentWeaponType != EWeaponType.None)
        {
            IsBlocking?.Invoke(true);
        }
    }
}
