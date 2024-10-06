using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private List<Transform> _patrolPoints;
    [SerializeField] private Transform _player;
    [Space]
    [Header("Settings")]
    [SerializeField] private float _speed = 3.5f;
    [SerializeField] private float _meleeRange = 2f;
    [SerializeField] private float _rangedRange = 10f;
    [SerializeField] private float _detectionRadius = 10f;
    [SerializeField] private float _meleeDamage = 5f;
    [SerializeField] private int _currentPatrolIndex = 0;

    private IEnemyState _currentState;
    private HealthController _healthController;

    public float Speed => _speed;
    public float MeleeRange => _meleeRange;
    public float RangedRange => _rangedRange;
    public float DetectionRadius => _detectionRadius;
    public float MeleeDamage => _meleeDamage;

    public NavMeshAgent NavAgent { get; private set; }
    public Animator Animator { get; private set; }
    public Transform Target { get; private set; }

    private void Awake()
    {
        NavAgent = GetComponent<NavMeshAgent>();
        Animator = GetComponent<Animator>();

        _healthController = GetComponent<HealthController>();
        _healthController.OnDeath += SwitchToDeathState;

        NavAgent.speed = Speed;

        SetTarget(_player);
        SetState(new IdleState(this, DetectionRadius));
    }

    private void Update()
    {
        if (_currentState != null)
        {
            _currentState.Update();
        }
    }

    private void OnDestroy()
    {
        _healthController.OnDeath -= SwitchToDeathState;
    }

    public void SetState(IEnemyState newState)
    {
        if (_currentState != null)
        {
            _currentState.Exit();
        }

        _currentState = newState;
        _currentState.Enter();
    }

    private void SwitchToDeathState()
    {
        SetState(new DeathState(this));
    }

    public void SetTarget(Transform target)
    {
        Target = target;
    }

    public void Disable()
    {
        NavAgent.enabled = false;
        Animator.enabled = false; 
    }

    // Возвращает случайную точку для патрулирования в пределах определенной зоны
    public Vector3 GetRandomPatrolPoint()
    {
        Vector3 randomDirection = Random.insideUnitSphere * 10f;
        randomDirection += transform.position;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, 10f, 1))
        {
            return hit.position;
        }
        return transform.position;
    }

    public Vector3 GetCurrentPatrolPoint()
    {
        if (_patrolPoints != null && _patrolPoints.Count > 0)
        {
            return _patrolPoints[_currentPatrolIndex].position;
        }
        return Vector3.zero;
    }

    public void AdvancePatrolPoint()
    {
        _currentPatrolIndex = (_currentPatrolIndex + 1) % _patrolPoints.Count;
    }

}
