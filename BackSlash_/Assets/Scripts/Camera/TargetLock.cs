using Scripts.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

public class TargetLock : MonoBehaviour
{
	[SerializeField] private SphereCollider _triggerCollider;

	[Header("Settings")]
	[SerializeField] private float _maxDistance;
	[SerializeField] private float _targetAngle;
	[SerializeField] private List<Target> _targets = new List<Target>();

	private Camera _mainCamera;
	private Target _currentTarget;
	private bool isTargeting = false;

	private InputController _inputController;

	public Target Target => _currentTarget;
	
	public event Action<bool> OnSwitchLock;

	[Inject]
	private void Construct(InputController inputService)
	{
		_inputController = inputService;
		_inputController.OnLockKeyPressed += AssignTarget;
	}

	private void Awake()
	{
		_mainCamera = Camera.main;
		_triggerCollider.radius = _maxDistance;

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
		_inputController.OnLockKeyPressed -= AssignTarget;
	}

	private void AssignTarget()
	{
		if (isTargeting)
		{
			isTargeting = false;
			_currentTarget = null;
			OnSwitchLock?.Invoke(false);
			return;
		}

		_currentTarget = ClosestTarget();

		if (_currentTarget)
		{
			isTargeting = true;
			OnSwitchLock?.Invoke(true);
		}
	}

	private void ForceUnlock(Target target)
	{
		if (target)
		{
			_currentTarget = null;
			
			OnSwitchLock?.Invoke(false);

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
