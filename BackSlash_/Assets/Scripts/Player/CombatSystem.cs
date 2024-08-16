using Scripts.Player;
using Scripts.Weapon;
using UnityEngine;
using Zenject;
using System;

public class CombatSystem : MonoBehaviour
{
    private InputController _inputController;
    private WeaponController _weaponController;

    public event Action OnPrimaryAttack;
    public event Action OnComboAttack;
    public event Action OnJumpComboAttack;
    public event Action<bool> IsBlocking;
    public event Action<bool> IsAttacking;

    [Inject]
    private void Construct(InputController inputController, WeaponController weaponController)
    {
        _inputController = inputController;

        _weaponController = weaponController;
    }

    private void Update()
    {
    }

    private void OnLightAttack()
    {
        if (_weaponController.CurrentWeaponType != EWeaponType.None)
        {
            OnPrimaryAttack?.Invoke();
            IsAttacking?.Invoke(true);
            Debug.Log("Standart attack");
        }
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
