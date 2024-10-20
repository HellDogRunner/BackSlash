using UnityEngine;

public class StaggerState : IEnemyState
{
    private readonly EnemyController _enemy;

    private float _staggerCooldown;
    private float _getUpCooldown;

    public StaggerState(EnemyController enemy)
    {
        _enemy = enemy;
    }

    public void Enter()
    {
        _enemy.Animator.SetBool("Stagger",true);
    }

    public void Update()
    {
        _staggerCooldown += Time.deltaTime;
        if (_staggerCooldown >= _enemy.StaggerTimer)
        {
            _enemy.Animator.SetBool("Stagger", false);
            _getUpCooldown += Time.deltaTime;

            if (_getUpCooldown >= 0.9f)
            {
                _getUpCooldown = 0;
                _staggerCooldown = 0;
                _enemy.SetState(new ChaseState(_enemy));
            }
        }
    }

    public void Exit()
    {
        _enemy.Animator.SetBool("Stagger", false);
    }

}
