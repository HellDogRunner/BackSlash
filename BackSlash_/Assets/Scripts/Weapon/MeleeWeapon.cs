using Scripts.Entity;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    [SerializeField] private EAttackType _attackType;

    private Entity _entity;
    private AttackModel _attack;

    private void Awake()
    {
        _entity = GetComponentInParent<Entity>();
    }

    private void OnEnable()
    {
        _entity.OnSetAttack += SetAttack;
        _entity.OnStartAttack += StartAttack;
        _entity.OnEndAttack += EndAttack;
    }

    private void OnDisable()
    {
        _entity.OnSetAttack -= SetAttack;
        _entity.OnStartAttack -= StartAttack;
        _entity.OnEndAttack -= EndAttack;
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
    public void SetAttack(AttackModel attack) => _attack = attack;
    private void StartAttack() => _attackType = _attack.Type;
    private void EndAttack() => _attackType = EAttackType.None;
}
