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
        // �������� �������� ������
        _enemy.Disable();
    }

    public void Update()
    {
        // ������ �� ������� ����������
    }

    public void Exit()
    {
        // ������� ���� �� ������ �������� �� ����� ���������
    }
}
