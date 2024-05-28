using Scripts.Weapon;
using Scripts.Weapon.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class SwordWeapon : MonoBehaviour
{
    private WeaponController _weaponController;
    private WeaponTypeModel _swordType;

    private bool isHit;

    private void Start()
    {
        _weaponController = GetComponentInParent<WeaponController>();
        _weaponController.OnAttackPerformed += Hit;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy" && isHit)
        {
            var hitbox = other.GetComponent<HitBox>();
            if (hitbox)
            {
                hitbox.OnSwordHit(_swordType);
            }
            isHit = false;
        }
    }

    private void Hit(WeaponTypeModel weaponType)
    {
        _swordType = weaponType;
        isHit = true;
    }

    private void OnDestroy()
    {
        _weaponController.OnAttackPerformed -= Hit;
    }
}
