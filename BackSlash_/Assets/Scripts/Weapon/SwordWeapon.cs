using Scripts.Weapon;
using Scripts.Weapon.Models;
using UnityEngine;
using Zenject;

public class SwordWeapon : MonoBehaviour
{
    [SerializeField] private ParticleSystem _swordTrails;
    [SerializeField] float _attackRange = 3f;
    [SerializeField] LayerMask _hitboxlayer;

    private WeaponTypesDatabase _weaponTypesDatabase;
    private ComboSystem _comboSystem;
    private bool _isAttacking;

    [Inject]
    private void Construct(WeaponTypesDatabase weaponTypesDatabase, ComboSystem comboSystem)
    {
        _weaponTypesDatabase = weaponTypesDatabase;
        _comboSystem = comboSystem;
        _comboSystem.IsAttacking += AttackFlag;
        _comboSystem.IsAttacking += ShowParticles;
    }

    private void Attack(bool isAttaking)
    {
        if (isAttaking)
        {
            Collider[] hitEnemies = Physics.OverlapSphere(gameObject.transform.position, _attackRange, _hitboxlayer);

            foreach (Collider enemy in hitEnemies)
            {
                var weaponType = _weaponTypesDatabase.GetWeaponTypeModel(EWeaponType.Melee);
                var weaponDamage = weaponType.LightAttackDamage;
                if (enemy.TryGetComponent(out HealthController health))
                {
                    health.TakeDamage(weaponDamage);
                }
            }
        }
    }

    private void AttackFlag(bool isAttacking)
    {
        _isAttacking = isAttacking;
        Attack(isAttacking);
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(gameObject.transform.position, _attackRange);
    }
}
