using Scripts.Weapon;
using Scripts.Weapon.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class HitBox : MonoBehaviour
{
    [SerializeField] private List<Collider> _hitboxes;

    private HealthService _health;
    private WeaponController _weaponController;
    private WeaponTypesDatabase _weaponTypesDatabase;

    private bool isHit;

    [Inject]
    private void Construct(WeaponController weaponController, WeaponTypesDatabase weaponTypesDatabase)
    {
        _weaponController = weaponController;
        _weaponTypesDatabase = weaponTypesDatabase;

        _health = GetComponent<HealthService>();
        _weaponController.OnAttack += Hit;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Sword" && isHit)
        {
            var weaponType = _weaponTypesDatabase.GetWeaponTypeModel(_weaponController.CurrentWeaponType);
            var weaponDamage = weaponType.LightAttackDamage;

            _health.TakeDamage(weaponDamage);
            isHit = false;
        }
    }

    private void Hit(int currentAttack)
    {
        isHit = true;
    }

    private void OnDestroy()
    {
        _weaponController.OnAttack -= Hit;
    }
}
