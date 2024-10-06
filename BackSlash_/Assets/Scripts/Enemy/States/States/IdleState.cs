using UnityEngine;

public class IdleState : IEnemyState
{
    private readonly EnemyController _enemy;
    private readonly float _detectionRadius;

    public IdleState(EnemyController enemy, float detectionRadius)
    {
        _enemy = enemy;
        _detectionRadius = detectionRadius;
    }

    public void Enter()
    {
        Debug.Log("idle state Entered");
        _enemy.NavAgent.isStopped = true;
        _enemy.Animator.SetFloat("Speed", 0);
    }

    public void Update()
    {
        float distanceToPlayer = Vector3.Distance(_enemy.transform.position, _enemy.Target.position);

        if (distanceToPlayer <= _detectionRadius)
        {
            _enemy.SetState(new ChaseState(_enemy));
        }
    }

    public void Exit()
    {
        
    }
}
