using Scripts.Entity;
using UnityEngine;
using Zenject;

public class PlayerWeapon : MonoBehaviour
{
    [SerializeField] private EAttackType _attackType;
    [SerializeField] private ParticleSystem _swordTrails;
    // [SerializeField] float _attackRange = 3f;
    // [SerializeField] LayerMask _hitboxlayer;
    
    private AttackModel _attack;

    private Entity _entity;
    private ComboSystem _comboSystem;

    [Inject]
    private void Construct(ComboSystem comboSystem, Entity entity)
    {
        _comboSystem = comboSystem;
        _entity = entity;
    }

    private void OnEnable()
    {
        _comboSystem.IsAttacking += AttackFlag;
        _comboSystem.IsAttacking += ShowParticles;
        _comboSystem.OnAttack += SetCurrentAttack;
    }

    private void OnDisable()
    {
        _comboSystem.IsAttacking -= AttackFlag;
        _comboSystem.IsAttacking -= ShowParticles;
        _comboSystem.OnAttack -= SetCurrentAttack;
    }

    private void Attack(Collider collider)
    {
        if (_attackType == EAttackType.None) return;
        
        if (collider.TryGetComponent(out HitBox hitBox))
        {
            if (hitBox.GetGameObject() == _entity.gameObject) return;
        
            hitBox.AttackTaken(_attack);
            _attackType = EAttackType.None;
        }
    }
    
    private void OnTriggerEnter(Collider other) => Attack(other);

    private void SetCurrentAttack(AttackModel attack)
    {
        _attack = attack;
        _attackType = attack.Type;
    }

    private void AttackFlag(bool isAttacking)
    {
        if (!isAttacking) _attackType = EAttackType.None;
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
    
    // private void Attack(bool isAttaking)
    // {
    //     if (isAttaking)
    //     {
    //         Collider[] hitEnemies = Physics.OverlapSphere(gameObject.transform.position, _attackRange, _hitboxlayer);

    //         foreach (Collider enemy in hitEnemies)
    //         {
    //             if (enemy.TryGetComponent(out Entity entity))
    //             {
    //                 entity.RegisterAttack(_attack);
    //                 return;
    //             }
    //         }
    //     }
    // }
}
