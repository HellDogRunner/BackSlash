using Scripts.Entity;
using Scripts.Weapon;
using Scripts.Weapon.Models;
using UnityEngine;
using Zenject;

public class SwordWeapon : MonoBehaviour
{
    [SerializeField] private ParticleSystem _swordTrails;
    [SerializeField] float _attackRange = 3f;
    [SerializeField] LayerMask _hitboxlayer;

    private AttackModel _attack;

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
        _comboSystem.OnAttack += SetCurrentAttack;
    }

    private void Attack(bool isAttaking)
    {
        if (isAttaking)
        {
            Collider[] hitEnemies = Physics.OverlapSphere(gameObject.transform.position, _attackRange, _hitboxlayer);

            foreach (Collider enemy in hitEnemies)
            {
                if (enemy.TryGetComponent(out Entity entity))
                {
                    entity.RegisterAttack(_attack);
                }
            }
        }
    }

    private void SetCurrentAttack(AttackModel attack)
    {
        _attack = attack;
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
        _comboSystem.OnAttack -= SetCurrentAttack;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(gameObject.transform.position, _attackRange);
    }
}
