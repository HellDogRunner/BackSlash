using Scripts.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UniRx.Triggers;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class TargetLock : MonoBehaviour
{
	[Header("Objects")]
	[SerializeField] private CinemachineCamera _lockOnCamera;
	[SerializeField] private CinemachineRotationComposer _rotationComposer;
	[SerializeField] private SphereCollider _triggerCollider;
	[Space]
	[SerializeField] private Image _aimIcon;

	[Header("Settings")]
	[SerializeField] private float _maxDistance;
	[SerializeField] private float _targetAngle;
	[SerializeField] private List<Target> _targets = new List<Target>();

	private Camera _mainCamera;
	private Target _currentTarget;
	private Vector3 TargetOffsetY;
	private bool isTargeting = false;

	private InputController _inputService;

	public event Action<bool> OnSwitchLock;

	public Target Target => _currentTarget;

	[Inject]
	private void Construct(InputController inputService)
	{
		_inputService = inputService;
		_inputService.OnLockKeyPressed += AssignTarget;
	}

	private void Awake()
	{
		_mainCamera = Camera.main;
		_triggerCollider.radius = _maxDistance;
		TargetOffsetY = new Vector3(0, _rotationComposer.TargetOffset.y, 0);

		_triggerCollider.OnTriggerEnterAsObservable()
		   .Subscribe(other =>
		   {
			   AddTargets(other);
		   }).AddTo(this);

		_triggerCollider.OnTriggerExitAsObservable()
		   .Subscribe(other =>
		   {
			   RemoveTargets(other);
		   }).AddTo(this);
	}

	private void Update()
	{
		if (_currentTarget)
		{
			if (_aimIcon != null) _aimIcon.transform.position = _mainCamera.WorldToScreenPoint(_currentTarget.LookAt);
		}	
	}

	private void AddTargets(Collider other)
	{
		if (!other.TryGetComponent<Target>(out Target target)) return;

		_targets.Add(target);
		target.OnTargetDeath += ForceUnlock;
	}

	private void RemoveTargets(Collider other)
	{
		if (!other.TryGetComponent<Target>(out Target target)) return;

		if (target == _currentTarget)
		{
			ForceUnlock(target);
			return;
		}

		_targets.Remove(target);
	}

	private void OnDestroy()
	{
		_inputService.OnLockKeyPressed -= AssignTarget;
	}

	private void AssignTarget()
	{
		if (isTargeting)
		{
			isTargeting = false;
			ShowIcon(false);
			_currentTarget = null;
			UnlockCamera();
			return;
		}

		_currentTarget = ClosestTarget();

		if (_currentTarget)
		{
			isTargeting = true;
			ShowIcon(true);
			LockOnTarget(_currentTarget.transform);
		}
	}

	private void LockOnTarget(Transform target)
	{
		_lockOnCamera.Target.LookAtTarget = target;
		_lockOnCamera.gameObject.SetActive(true);
		OnSwitchLock?.Invoke(true);
	}

	private void UnlockCamera()
	{
		_lockOnCamera.gameObject.SetActive(false);
		OnSwitchLock?.Invoke(false);
	}

	private void ShowIcon(bool show)
	{
		if (_aimIcon == null)
		{
			Debug.LogWarning("Aim Icon not set");
			return;
		}
		_aimIcon.gameObject.SetActive(show);
	}

	private void ForceUnlock(Target target)
	{
		if (target)
		{
			_currentTarget = null;

			_aimIcon.transform.position = new Vector3(0, 0, 0);
			
			UnlockCamera();

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
		float distance = _maxDistance;
		Vector3 position = transform.position;
		Target closest = null;

		_targets = _targets.Where(x => x != null).ToList();

		foreach (Target target in _targets)
		{
			Vector3 diff = target.transform.position - position;
			float curDistance = diff.magnitude;
			bool correctAngel = Vector3.Angle(diff.normalized, _mainCamera.transform.forward) < _targetAngle;

			if (curDistance < distance && correctAngel && target.IsValid)
			{
				closest = target;
				distance = curDistance;
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
