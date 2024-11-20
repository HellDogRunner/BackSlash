using UnityEngine;

public class PatrolState : IEnemyState
{
    private readonly EnemyController _enemy;
    private Vector3 _patrolTarget;
    private float _waitTime = 2f;
    private float _waitTimer;

    public PatrolState(EnemyController enemy)
    {
        _enemy = enemy;
    }

    public void Enter()
    {
        Debug.Log("Patrol state Entered");
        _enemy.NavAgent.isStopped = false;
        SetNewPatrolTarget();
        _waitTimer = 0f;
    }

    public void Update()
    {
        float distanceToPlayer = Vector3.Distance(_enemy.transform.position, _enemy.Target.position);

        if (distanceToPlayer <= _enemy.DetectionRadius)
        {
            _enemy.SetState(new ChaseState(_enemy));
            return;
        }

        if (Vector3.Distance(_enemy.transform.position, _patrolTarget) <= 1f)
        {
            _waitTimer += Time.deltaTime;

            if (_waitTimer >= _waitTime)
            {
                SetNewPatrolTarget();
                _enemy.AdvancePatrolPoint();
                _waitTimer = 0f;
            }
        }
        else
        {
            _enemy.NavAgent.SetDestination(_patrolTarget);
        }

        _enemy.Animator.SetFloat("Speed", _enemy.NavAgent.velocity.magnitude);
        Debug.Log(Vector3.Distance(_enemy.transform.position, _patrolTarget));
        Debug.Log(_waitTimer +" timer");
    }

    private void SetNewPatrolTarget()
    {
        _patrolTarget = _enemy.GetCurrentPatrolPoint();
        if (_patrolTarget == Vector3.zero) 
        {
            _enemy.SetState(new IdleState(_enemy, _enemy.DetectionRadius));
        }
    }

    public void Exit()
    {
        // ���������� ��������������
    }
}
