using System;
using UnityEngine;

public class Target : MonoBehaviour
{
	[SerializeField] Transform _lookAt;
	
	private bool isValid = true;

	private HealthController _health;

	public Vector3 LookAt => _lookAt.position;
	public bool IsValid => isValid;

	public event Action<Target> OnTargetDeath;

	private void Awake()
	{
		_health = gameObject.GetComponent<HealthController>();
		_health.OnDeath += ChangeValidation;
	}

	private void ChangeValidation()
	{
		if (_health.IsDead)
		{
			isValid = false;
			OnTargetDeath?.Invoke(this);
		}
	}

	private void OnDestroy()
	{
		_health.OnDeath -= ChangeValidation;
	}
}
