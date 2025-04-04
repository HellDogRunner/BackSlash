using Scripts.Entity;
using UnityEngine;
using Zenject;

public class SwordWeapon : MonoBehaviour
{
    [SerializeField] private ParticleSystem _swordTrails;
    [SerializeField] float _attackRange = 3f;
    [SerializeField] LayerMask _hitboxlayer;

    private AttackModel _attack;

    private ComboSystem _comboSystem;

    [Inject]
    private void Construct(ComboSystem comboSystem)
    {
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
                    return;
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
