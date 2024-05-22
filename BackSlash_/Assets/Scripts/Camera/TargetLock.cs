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

    private InputService _inputService;
    private HealthService _health;

    public Transform CurrentTarget => _currentTarget;

    [Inject]
    private void Construct(InputService inputService)
    {
        _inputService = inputService;
        _inputService.OnLockKeyPressed += AssignTarget;

        _maxAngle = 90f;
        _cinemachineFreeLook.m_XAxis.m_InputAxisName = "";
        _cinemachineFreeLook.m_YAxis.m_InputAxisName = "";
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
            _health = _currentTarget.GetComponentInParent<HealthService>();
            _health.OnDeath += ForceUnlock;
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
            _aimIcon.transform.position = _mainCamera.WorldToScreenPoint(target.position);

        if ((target.position - transform.position).magnitude < _minDistance) return;
        _mouseX = (viewPos.x - 0.5f + _targetLockOffset.x) * 3f;             
        _mouseY = (viewPos.y - 0.5f + _targetLockOffset.y) * 3f;              
    }

    private void ForceUnlock()
    {
        _currentTarget = null;
        _health.OnDeath -= ForceUnlock;
    }


    private GameObject ClosestTarget() 
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag(_enemyTag);
        GameObject closest = null;
        float distance = _maxDistance;
        float currAngle = _maxAngle;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            if (go.GetComponentInParent<HealthService>().Health <= 0)
            {
                continue;
            }

            Vector3 diff = go.transform.position - position;
            float curDistance = diff.magnitude;
            if (curDistance < distance)
            {
                Vector3 viewPos = _mainCamera.WorldToViewportPoint(go.transform.position);
                Vector2 newPos = new Vector3(viewPos.x - 0.5f, viewPos.y - 0.5f);
                if (Vector3.Angle(diff.normalized, _mainCamera.transform.forward) < _maxAngle)
                {
                    closest = go;
                    currAngle = Vector3.Angle(diff.normalized, _mainCamera.transform.forward.normalized);
                    distance = curDistance;
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