using Scripts.Enemy;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthService : MonoBehaviour
{
    [SerializeField] private float _health;

    private Ragdoll _ragdoll;

    public Action<float> OnHealthChanged;
    public float Health => _health;

    private void Start() 
    { 
        _ragdoll = GetComponent<Ragdoll>();
    }

    public void TakeDamage(float damage) 
    {
        _health -= damage;
        OnHealthChanged?.Invoke(_health);
        if (_health <= 0)
        {
            Death();
        }
    }

    private void Death() 
    {
        //Destroy(gameObject);
        if (_ragdoll)
        {
            Debug.Log("dead");
            _ragdoll.ActivateRagdoll();
        }
    }
}
