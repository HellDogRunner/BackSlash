using UnityEngine;

public class ChaseState : IEnemyState
{
    private readonly EnemyController _enemy;
    private readonly float _chaseDuration = 2f;
    private float _chaseTimer;

    public ChaseState(EnemyController enemy)
    {
        _enemy = enemy;
    }

    public void Enter()
    {
        _enemy.NavAgent.isStopped = false;
        _chaseTimer = 0f;
    }

    public void Update()
    {
        float distanceToPlayer = Vector3.Distance(_enemy.transform.position, _enemy.Target.position);

        _enemy.Animator.SetFloat("Speed", _enemy.NavAgent.velocity.magnitude);
        if (distanceToPlayer > _enemy.DetectionRadius)
        {
            _chaseTimer += Time.deltaTime;

            if (_chaseTimer >= _chaseDuration)
            {
                _enemy.SetState(new SearchState(_enemy));
            }
        }
        else
        {
            _enemy.NavAgent.SetDestination(_enemy.Target.position);
            _chaseTimer = 0f;
        }

        if (distanceToPlayer <= _enemy.RangedRange)
        {
            _enemy.SetState(new AttackState(_enemy));
        }
    }

    public void Exit()
    {
        // ���������� �������������
    }
}
