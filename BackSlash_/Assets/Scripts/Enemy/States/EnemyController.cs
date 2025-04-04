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
    [Space]
    [Header("Settings")]
    [SerializeField] private float _speed = 3.5f;
    [SerializeField] private float _meleeRange = 2f;
    [SerializeField] private float _rangedRange = 10f;
    [SerializeField] private float _detectionRadius = 10f;
    [SerializeField] private int _currentPatrolIndex = 0;

    private IEnemyState _currentState;
    private HealthController _healthController;
    private AttackModel _currentAttack;

    public Entity Player { get; private set; }
    public List<AttackModel> Attacks { get; private set; }
    public float Speed => _speed;
    public float MeleeRange => _meleeRange;
    public float RangedRange => _rangedRange;
    public float DetectionRadius => _detectionRadius;
    
    public List<AttackModel> Attack;
    public StabilityModel Stability;
    //FIXME
    [HideInInspector] public EntityDatabase Setup;

    public NavMeshAgent NavAgent { get; private set; }
    public Animator Animator { get; private set; }
    public Transform Target { get; private set; }
    
    public event Action OnAttackCooldownOver;
    public event Action OnRangeAttackReady;

    [Inject]
    private void Construct(Entity player)
    {
        Player = player;
    }

    private void Awake()
    {
        NavAgent = GetComponent<NavMeshAgent>();
        Animator = GetComponent<Animator>();
        _healthController = GetComponent<HealthController>();
    
        NavAgent.speed = Speed;

        Setup = Entity.Setup.GetData();

        Stability = Entity.Setup.Stability;
        Attack = Entity.Setup.Attack;
        Target = Player.transform;
        SetState(new IdleState(this));
    }

    void OnEnable()
    {
        _healthController.OnDeath += SwitchToDeathState;
        
        Entity.OnStun += SwitchToStunState;
        Entity.OnSetAttack += SetAttacks;
    }

    void OnDisable()
    {
        _healthController.OnDeath -= SwitchToDeathState;
        
        Entity.OnStun -= SwitchToStunState;
        Entity.OnSetAttack -= SetAttacks;
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

    private void SwitchToDeathState()
    {
        SetState(new DeathState(this));
    }

    private void SwitchToStunState()
    {
        Debug.Log("stun");
        SetState(new StunState(this));
    }

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

    private void SetAttacks(List<AttackModel> attacks)
    {
        Attacks = attacks;
    }

    public void SetCurrentAttack(AttackModel attack)
    {
        _currentAttack = attack;
    }

    private void OnReadyToRangeAttack()
    {
        OnRangeAttackReady?.Invoke();
    }

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
