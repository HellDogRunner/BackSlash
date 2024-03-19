using Scripts.Enemy;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealhService : MonoBehaviour
{
    [SerializeField] private float health;

    private Ragdoll _ragdoll;

    public Action<float> OnHealthChanged;
    public float Health => health;

    private void Start() 
    { 
        _ragdoll = GetComponent<Ragdoll>();
    }

    public void TakeDamage(float damage) 
    {
        health -= damage;
        OnHealthChanged?.Invoke(health);
        if (health <= 0)
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
