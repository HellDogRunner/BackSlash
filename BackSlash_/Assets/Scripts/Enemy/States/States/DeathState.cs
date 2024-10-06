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
        Debug.Log("death state Entered");
        // Включить анимацию смерти
        _enemy.Disable();
    }

    public void Update()
    {
        // Смерть не требует обновления
    }

    public void Exit()
    {
        // Мертвый враг не должен выходить из этого состояния
    }
}
