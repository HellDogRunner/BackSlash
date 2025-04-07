using UnityEngine;

public class DeathState : IEnemyState
{
    private readonly EnemyController _enemy;

    public DeathState(EnemyController enemy)
    {
        _enemy = enemy;
    }

    public void Enter()
    {
        _enemy.Collider.enabled = false;
        _enemy.NavAgent.enabled = false;
        _enemy.Animator.enabled = false; 
    }

    public void Update()
    {
    }

    public void Exit()
    {     
    }
}
