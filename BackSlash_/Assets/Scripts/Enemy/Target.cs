using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    private bool isValid = true;

    private HealthService _health;

    public bool IsValid => isValid;

    public event Action<Target> OnTargetDeath;

    private void Awake()
    {
        _health = gameObject.GetComponent<HealthService>();
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
}
