using UnityEngine;

public class AttackState : IEnemyState
{
    private readonly EnemyController _enemy;

    private Vector3 _offset;
    private RaycastWeapon _weapon;

    private float _attackCooldown = 2f;
    private float _attackTimer;
    private bool _isAttack;

    public AttackState(EnemyController enemy)
    {
        _enemy = enemy;
    }

    public void Enter()
    {
        _enemy.NavAgent.isStopped = true;
        _weapon = _enemy.GetComponentInChildren<RaycastWeapon>();
    }

    public void Update()
    {
        if (IsInRangeForMelee())
        {
            _attackTimer += Time.deltaTime;
            PerformMeleeAttack();
            PerformMeleeAtackAnimation();
        }
        else if (IsInRangeForRanged())
        {
            ShootTarget(_enemy.Target);
            PerformRangeAtackAnimation();
        }
        else
        {
            _isAttack = false;
            _weapon.StopFiring();
            _enemy.SetState(new ChaseState(_enemy));
        }
        if (!IsPlayerInSight())
        {
            RotateTowardsTarget();
        }
    }

    private bool IsInRangeForMelee()
    {
        return Vector3.Distance(_enemy.transform.position, _enemy.Target.position) < _enemy.MeleeRange;
    }

    private bool IsInRangeForRanged()
    {
        return Vector3.Distance(_enemy.transform.position, _enemy.Target.position) < _enemy.RangedRange;
    }

    private float DistanceToTarget()
    {
       return Vector3.Distance(_enemy.transform.position, _enemy.Target.position);
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

    private void ShootTarget(Transform target)
    {
        if (!_isAttack)
        {
            _weapon.StartFiring();
            _isAttack = true;
        }

        if (_weapon.IsFiring)
        {
            _weapon.UpdateFiring(Time.deltaTime, target.position + _offset);
        }
        _weapon.UpdateBullets(Time.deltaTime);
    }

    private void PerformMeleeAttack()
    {
        if (_attackTimer >= _attackCooldown)
        {
            if (IsInRangeForMelee())
            {
                DealDamageToTarget();
            }
            _attackTimer = 0f;
        }
    }

    private void PerformMeleeAtackAnimation()
    {
        _enemy.Animator.SetBool("Punch", true);
        _enemy.Animator.SetBool("Shoot", false);
    }

    private void PerformRangeAtackAnimation()
    {
        _enemy.Animator.SetBool("Punch", false);
        _enemy.Animator.SetBool("Shoot", true);
    }

    private void DealDamageToTarget()
    {
        if (_enemy.Target.TryGetComponent(out HealthController playerHealth))
        {
            playerHealth.TakeDamage(_enemy.MeleeDamage);
        }
    }

    public void Exit()
    {
        _enemy.Animator.SetBool("Shoot", false);
        _enemy.Animator.SetBool("Punch", false);
    }
}
