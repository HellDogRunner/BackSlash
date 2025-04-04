using System.Collections.Generic;
using Scripts.Entity;
using UnityEngine;

public class AttackState : IEnemyState
{
    private readonly EnemyController _enemy;

    private Vector3 _offset;
    private RaycastWeapon _weapon;

    private List<AttackModel> _attacks;
    private AttackModel _currentAttack;
    
    private bool _isAttack;

    public AttackState(EnemyController enemy)
    {
        _enemy = enemy;
    }

    public void Enter()
    {
        _weapon = _enemy.GetComponentInChildren<RaycastWeapon>();
        _weapon.OnHit += HitTarget;
        
        _enemy.NavAgent.isStopped = true;
        _enemy.OnAttackCooldownOver += SetAttackReady;
        _enemy.OnRangeAttackReady += StartRangedAttack;
        
        _attacks = _enemy.Attack;
    }

    public void Update()
    {
        _weapon.UpdateBullets(Time.deltaTime);
    
        if (!IsPlayerInSight())
        {
            RotateTowardsTarget();
        }
    
        if (!IsInRange())
        {
            SwitchToChaseState();
            return;
        }
        
        if (IsInRange() && !_isAttack)
        {
            var attack = ChooseAttack(SortAttacks(GetAttackType()));
            if (attack != null) StartAttack(attack);
        }
    }

    private void SwitchToChaseState()
    {
        _isAttack = false;
        _enemy.SetState(new ChaseState(_enemy));
    }

    private void StartRangedAttack()
    {
        _weapon.FireBullet(_enemy.Target.position + _offset, _currentAttack);
    }

    private void StartAttack(AttackModel attack)
    {
        _currentAttack = new AttackModel();
        _currentAttack = attack;
        
        _enemy.SetCurrentAttack(attack);
        _isAttack = true;
        
        _enemy.Animator.SetTrigger(attack.Type.ToString());
        
        _currentAttack.LastUseTime = Time.time;
    
        if (!attack.Ranged)
        {
            HitTarget();
        }
    }

    private List<AttackModel> SortAttacks(EType type)
    {
        var attacks = new List<AttackModel>();
    
        foreach (var attack in _attacks)
        {
            if (type == attack.Type && IsAttackReady(attack))
            {
                attacks.Add(attack);
            }
        }
        return attacks;
    }
    
    private bool IsPlayerInSight()
    {
        Vector3 directionToTarget = (_enemy.Target.position - _enemy.transform.position).normalized;
        float angle = Vector3.Angle(_enemy.transform.forward, directionToTarget);

        float fieldOfView = 45f;
        float viewDistance = 10f;

        if (angle < fieldOfView / 2f && Vector3.Distance(_enemy.transform.position, _enemy.Target.position) <= viewDistance)
        {
            if (!Physics.Raycast(_enemy.transform.position, directionToTarget, out RaycastHit hit, viewDistance))
            {
                return true;
            }
        }
        return false;
    }

    private void RotateTowardsTarget()
    {
        Vector3 directionToPlayer = (_enemy.Target.position - _enemy.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);
        _enemy.transform.rotation = Quaternion.Slerp(_enemy.transform.rotation, lookRotation, Time.deltaTime * 5f);
    }
    
    private void HitTarget()
    {
        if (_enemy.Player == null) return;
        
        _enemy.Player.RegisterAttack(_currentAttack);
    }

    private void HitTarget(AttackModel attack)
    {
        if (_enemy.Player == null) return;
    
        _enemy.Player.RegisterAttack(attack);
    }

    private AttackModel ChooseAttack(List<AttackModel> attacks)
    {
        if (attacks.Count == 0) return null;
    
        return attacks[Random.Range(0, attacks.Count)];
    }
    
    private bool IsAttackReady(AttackModel attack)
    {
        return attack.LastUseTime + attack.Cooldown < Time.time;
    }
    
    private bool IsInRange()
    {
        return GetDistance() < _enemy.RangedRange;
    }

    private EType GetAttackType()
    {
        return GetDistance() <= _enemy.MeleeRange ? EType.Punch : EType.Shoot;
    }

    private float GetDistance()
    {
        return Vector3.Distance(_enemy.transform.position, _enemy.Target.position);
    }
    
    private void SetAttackReady() => _isAttack = false;

    public void Exit()
    {
        _weapon.OnHit -= HitTarget;
        _enemy.OnAttackCooldownOver -= SetAttackReady;
        _enemy.OnRangeAttackReady -= StartRangedAttack;
    }
}
