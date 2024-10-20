using Scripts.Weapon;
using Scripts.Weapon.Models;
using UnityEngine;
using Zenject;

public class SwordWeapon : MonoBehaviour
{
    [SerializeField] private ParticleSystem _swordTrails;

    private WeaponTypesDatabase _weaponTypesDatabase;
    private ComboSystem _comboSystem;
    [SerializeField] private bool _isAttacking;

    [Inject]
    private void Construct(WeaponTypesDatabase weaponTypesDatabase, ComboSystem comboSystem)
    {
        _weaponTypesDatabase = weaponTypesDatabase;
        _comboSystem = comboSystem;
        _comboSystem.IsAttacking += AttackFlag;
        _comboSystem.IsAttacking += ShowParticles;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy" && _isAttacking)
        {
            var hitbox = other.GetComponent<HitBox>();
            if (hitbox)
            {
                var weaponType = _weaponTypesDatabase.GetWeaponTypeModel(EWeaponType.Melee);
                var weaponDamage = weaponType.LightAttackDamage;
                hitbox.OnMeleeHit(weaponDamage);
                _isAttacking = false;
            }
        }
    }

    private void AttackFlag(bool isAttacking)
    {
        _isAttacking = isAttacking;
    }

    private void ShowParticles(bool isAttacking)
    {
        if (isAttacking)
        {
            _swordTrails.Play();
        }
        else
        {
            _swordTrails.Stop();
        }
    }

    private void OnDestroy()
    {
        _comboSystem.IsAttacking -= AttackFlag;
        _comboSystem.IsAttacking -= ShowParticles;
    }
}
