using System;
using System.Collections;
using System.Collections.Generic;
using Scripts.Entity;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

[RequireComponent(typeof(Entity))]
public class EnemyController : MonoBehaviour
{
    [field: SerializeField] public Entity Entity { get; private set; }
    [SerializeField] private List<Transform> _patrolPoints;

    [Header("Settings")]
    [SerializeField] private float _speed = 3.5f;
    [SerializeField] private float _meleeRange = 2f;
    [SerializeField] private float _rangedRange = 10f;
    [SerializeField] private float _detectionRadius = 10f;
    [SerializeField] private int _currentPatrolIndex = 0;

    private IEnemyState _currentState;
    private HealthController _healthController;
    private List<AttackModel> _attack;
    private StabilityModel _stability;
    private AttackModel _currentAttack;

    public Entity PlayerEntity { get; private set; }
    public float Speed => _speed;
    public float MeleeRange => _meleeRange;
    public float RangedRange => _rangedRange;
    public float DetectionRadius => _detectionRadius;
    
    public List<AttackModel> Attack => _attack;
    public StabilityModel Stability => _stability;

    public NavMeshAgent NavAgent { get; private set; }
    public Animator Animator { get; private set; }
    public Transform Target { get; private set; }
    
    public event Action OnAttackCooldownOver;

    [Inject]
    private void Construct(Entity player)
    {
        PlayerEntity = player;
    }

    private void Awake()
    {
        NavAgent = GetComponent<NavMeshAgent>();
        Animator = GetComponent<Animator>();
        _healthController = GetComponent<HealthController>();
    
        _attack = Entity.Setup.GetAttack();
        _stability = Entity.Setup.GetStability();
        
        NavAgent.speed = Speed;
        Target = PlayerEntity.transform;
        SetState(new IdleState(this));
    }

    void OnEnable()
    {
        _healthController.OnDeath += SwitchToDeathState;
        Entity.OnStun += SwitchToStunState;
    }

    void OnDisable()
    {
        _healthController.OnDeath -= SwitchToDeathState;
        Entity.OnStun -= SwitchToStunState;
    }

    private void Update()
    {
        if (_currentState != null)
        {
            _currentState.Update();
        }
        
        CheckAttackStarted();
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

    private void CheckAttackStarted()
    {
        if (_currentAttack == null) return;
    
        var state = Animator.GetCurrentAnimatorStateInfo(0);

        if (state.IsName(_currentAttack.Type.ToString()))
        {
            StartCoroutine(AttackCooldown(state.length + _currentAttack.TimeAfter));
            _currentAttack = null;
        }
    }

    private void SwitchToDeathState() => SetState(new DeathState(this));

    private void SwitchToStunState() => SetState(new StunState(this));

    // ���������� ��������� ����� ��� �������������� � �������� ������������ ����
    public Vector3 GetRandomPatrolPoint()
    {
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * 10f;
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

    public void SetCurrentAttack(AttackModel attack) => _currentAttack = attack;

    private void OnStartAttack() => Entity.StartAttack();
    private void OnEndAttack() => Entity.EndAttack();

    private IEnumerator AttackCooldown(float time)
    {
        yield return new WaitForSeconds(time);
        OnAttackCooldownOver?.Invoke();
    }

    public void Disable()
    {
        NavAgent.enabled = false;
        Animator.enabled = false; 
    }
}
