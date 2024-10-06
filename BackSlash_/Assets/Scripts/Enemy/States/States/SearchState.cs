using UnityEngine;

public class SearchState : IEnemyState
{
    private readonly EnemyController _enemy;
    private Vector3 _searchTarget;
    private int _searchAttempts;
    private const int MaxSearchAttempts = 5; // Максимальное количество попыток поиска
    private float _searchDuration = 3f; // Время ожидания на каждой точке поиска
    private float _searchTimer;

    public SearchState(EnemyController enemy)
    {
        _enemy = enemy;
    }

    public void Enter()
    {
        Debug.Log("Search state Entered");
        _enemy.NavAgent.isStopped = false;
        _searchAttempts = 0;
        SetNewSearchTarget();
        _searchTimer = 0f;
    }

    public void Update()
    {
        if (_searchAttempts >= MaxSearchAttempts)
        {
            _enemy.SetState(new PatrolState(_enemy));
            return;
        }

        _enemy.NavAgent.SetDestination(_searchTarget);

        if (Vector3.Distance(_enemy.transform.position, _searchTarget) <= 1f)
        {
            _searchTimer += Time.deltaTime;
            if (_searchTimer >= _searchDuration)
            {
                _searchAttempts++;
                SetNewSearchTarget();
                _searchTimer = 0f;
            }
        }

        float distanceToPlayer = Vector3.Distance(_enemy.transform.position, _enemy.Target.position);
        if (distanceToPlayer <= _enemy.DetectionRadius)
        {
            _enemy.SetState(new ChaseState(_enemy));
        }

        _enemy.Animator.SetFloat("Speed", _enemy.NavAgent.velocity.magnitude);
    }

    private void SetNewSearchTarget()
    {
        _searchTarget = _enemy.GetRandomPatrolPoint();
    }

    public void Exit()
    {
        // Завершение состояния поиска
    }
}
