using System.Collections.Generic;
using Scripts.Entity;
using UnityEngine;

public class AttackState : IEnemyState
{
    private readonly EnemyController _enemy;

    private Vector3 _offset;
    private AttackModel _currentAttack;
    
    private bool _isAttack;

    public AttackState(EnemyController enemy)
    {
        _enemy = enemy;
    }

    public void Enter()
    {
        //Debug.Log("Attack state enter");
        _enemy.NavAgent.isStopped = true;
        
        Subscribe();
    }

    private void Subscribe()
    {
        _enemy.OnAttackCooldownOver += SetAttackReady;
    }

    private void Unsubscribe()
    {
        _enemy.OnAttackCooldownOver -= SetAttackReady;
    }

    public void Update()
    {
        if (!IsPlayerInSight())
        {
            RotateTowardsTarget();
        }
    
        if (!IsInRange() && !_isAttack)
        {
            SwitchToChaseState();
            return;
        }
        
        if (IsInRange() && !_isAttack)
        {
            var attack = ChooseAttack(SortAttacks(GetRangedAttack()));
            if (attack != null) Attack(attack);
        }
    }

    private void SwitchToChaseState()
    {
        _enemy.SetState(new ChaseState(_enemy));
    }

    private void Attack(AttackModel attack)
    {
        _isAttack = true;
        
        _currentAttack = attack;
        _currentAttack.LastUseTime = Time.time;
        
        _enemy.SetCurrentAttack(attack);
        _enemy.Animator.SetTrigger(attack.Type.ToString());
        _enemy.Entity.SetAttack(attack);
        _enemy.Entity.SetTarget(_enemy.Target.position + _offset);
    }

    private List<AttackModel> SortAttacks(bool ranged)
    {
        var attacks = new List<AttackModel>();
    
        foreach (var attack in _enemy.Attack)
        {
            if (ranged == attack.Ranged && IsAttackReady(attack))
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

        if (angle < _enemy.FieldOfView / 2f && GetDistance() <= _enemy.ViewDistance)
        {
            if (!Physics.Raycast(_enemy.transform.position, directionToTarget, _enemy.ViewDistance))
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

    private bool GetRangedAttack()
    {
        return GetDistance() <= _enemy.MeleeRange ? false : true;
    }

    private float GetDistance()
    {
        return Vector3.Distance(_enemy.transform.position, _enemy.Target.position);
    }
    
    private void SetAttackReady() => _isAttack = false;

    public void Exit()
    {
        _enemy.Entity.EndAttack();
        
        Unsubscribe();
    }
}
