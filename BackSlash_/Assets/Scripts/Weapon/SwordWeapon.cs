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
    private CombatSystem _cobatSystem;

    [Inject]
    private void Construct(WeaponTypesDatabase weaponTypesDatabase, CombatSystem cobatSystem)
    {
        _weaponTypesDatabase = weaponTypesDatabase;
        _cobatSystem = cobatSystem;
        _cobatSystem.IsAttacking += DealDamage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy" && isHit)
        {
            var hitbox = other.GetComponent<HitBox>();
            if (hitbox)
            {
                var weaponType = _weaponTypesDatabase.GetWeaponTypeModel(EWeaponType.Melee);
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
        _cobatSystem.IsAttacking -= DealDamage;
    }
}
