using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using Scripts.Player;
using Zenject;

public class TargetLock : MonoBehaviour
{
    [Header("Objects")]
    [Space]
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private CinemachineFreeLook _cinemachineFreeLook; 
    [Space]
    [Header("UI")]
    [SerializeField] private Image _aimIcon;  
    [Space]
    [Header("Settings")]
    [Space]
    [SerializeField] private string _enemyTag; 
    [SerializeField] private Vector2 _targetLockOffset;
    [SerializeField] private float _minDistance; 
    [SerializeField] private float _maxDistance;

    public bool isTargeting = false;

    private float _maxAngle;
    private Transform _currentTarget;
    private float _mouseX;
    private float _mouseY;

    private List<Target> enemies;

    private InputController _inputService;
    private EnemyService _enemyService;
    public Transform CurrentTarget => _currentTarget;

    [Inject]
    private void Construct(InputController inputService, EnemyService enemyService)
    {
        _inputService = inputService;
        _inputService.OnLockKeyPressed += AssignTarget;

        _enemyService = enemyService;
    }

    private void Awake()
    {
        _maxAngle = 90f;
        _cinemachineFreeLook.m_XAxis.m_InputAxisName = "";
        _cinemachineFreeLook.m_YAxis.m_InputAxisName = "";

        enemies = _enemyService.EnemyList;
    }

    private void OnDestroy()
    {
        _inputService.OnLockKeyPressed -= AssignTarget;
    }

    void Update()
    {
        if (!isTargeting)
        {
            _mouseX = Input.GetAxis("Mouse X");
            _mouseY = Input.GetAxis("Mouse Y");
        }
        else
        {
            NewInputTarget(_currentTarget);
        }

        if (_aimIcon)
        {
            _aimIcon.gameObject.SetActive(isTargeting);
        }

        _cinemachineFreeLook.m_XAxis.m_InputAxisValue = _mouseX;
        _cinemachineFreeLook.m_YAxis.m_InputAxisValue = _mouseY;
    }

    private void AssignTarget()
    {       
        if (isTargeting)
        {
            isTargeting = false;
            _currentTarget = null;
            return;
        }

        if (ClosestTarget())
        {
            _currentTarget = ClosestTarget().transform;
            var target = ClosestTarget();
            target.OnTargetDeath += ForceUnlock;
            isTargeting = true;
        }
    }

    private void NewInputTarget(Transform target) 
    {
        if (!_currentTarget)
        {
            isTargeting = false;
            return;
        }
        Vector3 viewPos = _mainCamera.WorldToViewportPoint(target.position);

        if (_aimIcon)
            _aimIcon.transform.position = _mainCamera.WorldToScreenPoint(new Vector3(target.position.x, 1 ,target.position.z));

        if ((target.position - transform.position).magnitude < _minDistance) return;
        _mouseX = (viewPos.x - 0.5f + _targetLockOffset.x) * 3f;             
        _mouseY = (viewPos.y - 0.5f + _targetLockOffset.y) * 3f;              
    }

    private void ForceUnlock(Target target)
    {
        _currentTarget = null;
        target.OnTargetDeath -= ForceUnlock;
    }


    private Target ClosestTarget() 
    {
        Target closest = null;
        float distance = _maxDistance;
        float currAngle = _maxAngle;
        Vector3 position = transform.position;
        foreach (Target target in enemies)
        {
            Vector3 diff = target.transform.position - position;
            float curDistance = diff.magnitude;
            if (curDistance < distance)
            {
                if (target.IsValid)
                {
                    Vector3 viewPos = _mainCamera.WorldToViewportPoint(target.transform.position);
                    Vector2 newPos = new Vector3(viewPos.x - 0.5f, viewPos.y - 0.5f);
                    if (Vector3.Angle(diff.normalized, _mainCamera.transform.forward) < _maxAngle)
                    {
                        closest = target;
                        currAngle = Vector3.Angle(diff.normalized, _mainCamera.transform.forward.normalized);
                        distance = curDistance;
                    }
                }
            }
        }
        return closest;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _maxDistance);
    }
}