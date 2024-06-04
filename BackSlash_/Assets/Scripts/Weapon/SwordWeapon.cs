using Scripts.Weapon;
using Scripts.Weapon.Models;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using Zenject;

public class SwordWeapon : MonoBehaviour
{
    private bool isHit;

    private WeaponTypesDatabase _weaponTypesDatabase;
    private WeaponController _weaponController;

    [Inject]
    private void Construct(WeaponTypesDatabase weaponTypesDatabase, WeaponController weaponController)
    {
        _weaponTypesDatabase = weaponTypesDatabase;
        _weaponController = weaponController;
        _weaponController.IsAttacking += DealDamage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy" && isHit)
        {
            var hitbox = other.GetComponent<HitBox>();
            if (hitbox)
            {
                var weaponType = _weaponTypesDatabase.GetWeaponTypeModel(_weaponController.CurrentWeaponType);
                var weaponDamage = weaponType.LightAttackDamage;
                hitbox.OnSwordHit(weaponDamage);
            }
        }
    }

    private void DealDamage(bool isAble)
    {
        isHit = isAble;
    }

    private void OnDestroy()
    {
        _weaponController.IsAttacking -= DealDamage;
    }
}
