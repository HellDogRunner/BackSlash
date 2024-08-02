using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using Scripts.Player;
using Zenject;
using System;

public class TargetLock : MonoBehaviour
{
    [Header("Objects")]
    [Space]
    [SerializeField] private CinemachineFreeLook _cinemachineFreeLook;
    [SerializeField] private SphereCollider _triggerCollider;
    [Space]
    [Header("UI")]
    [SerializeField] private Image _aimIcon;  
    [Space]
    [Header("Settings")]
    [Space]
    [SerializeField] private Vector2 _targetLockOffset; 
    [SerializeField] private float _maxDistance;

    public bool isTargeting = false;

    public event Action<GameObject> OnStartTargeting;
    public event Action<GameObject> OnStopTarteting;

    private Camera _mainCamera;
    private Transform _currentTargetTransform;
    private Target _currentTarget;
    private float _maxAngle;
    private float _mouseX;
    private float _mouseY;

    private List<Target> _targets = new List<Target>();

    private InputController _inputService;

    public Transform CurrentTargetTransform => _currentTargetTransform;

    [Inject]
    private void Construct(InputController inputService)
    {
        _inputService = inputService;
        _inputService.OnLockKeyPressed += AssignTarget;
    }

    private void Awake()
    {
        _mainCamera = Camera.main;
        _maxAngle = 90f;
        _triggerCollider.radius = _maxDistance;
        _cinemachineFreeLook.m_XAxis.m_InputAxisName = "";
        _cinemachineFreeLook.m_YAxis.m_InputAxisName = "";
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<Target>(out Target target)) { return; }

        _targets.Add(target);
        target.OnTargetDeath += ForceUnlock;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent<Target>(out Target target)) { return; }
        _targets.Remove(target);
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
            NewInputTarget(_currentTargetTransform);
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
            _currentTargetTransform = null;
            OnStopTarteting?.Invoke(_currentTarget.gameObject);
            return;
        }

        if (ClosestTarget())
        {
            _currentTarget = ClosestTarget();
            _currentTargetTransform = _currentTarget.transform;
            isTargeting = true;
            OnStartTargeting?.Invoke(_currentTarget.gameObject);
        }
    }

    private void NewInputTarget(Transform target) 
    {
        if (!_currentTargetTransform)
        {
            isTargeting = false;
            return;
        }

        float distance = Vector3.Distance(_currentTargetTransform.position, gameObject.transform.position);
        if (distance > _maxDistance)
        {
            ForceUnlock(_currentTarget);
        }

        Vector3 viewPos = _mainCamera.WorldToViewportPoint(target.position);

        if (_aimIcon)
            _aimIcon.transform.position = _mainCamera.WorldToScreenPoint(new Vector3(target.position.x, 1 ,target.position.z));

        _mouseX = (viewPos.x - 0.5f + _targetLockOffset.x) * 3f;
        _mouseY = (viewPos.y - 0.5f + _targetLockOffset.y) * 3f;              
    }

    private void ForceUnlock(Target target)
    {
        if (target)
        {
            _currentTargetTransform = null;
            _targets.Remove(target);
            target.OnTargetDeath -= ForceUnlock;
            if (isTargeting)
            {
                isTargeting = false;
                AssignTarget();
            }
        }
    }


    private Target ClosestTarget() 
    {
        Target closest = null;
        float distance = _maxDistance;
        float currAngle = _maxAngle;
        Vector3 position = transform.position;
        foreach (Target target in _targets)
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