using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealhService : MonoBehaviour
{
    [SerializeField] private int Health;

    public Action<int> OnHealthChanged;

    public void TakeDamage(int damage, object sender = null) 
    {
        Health -= damage;
        OnHealthChanged?.Invoke(Health);
        if (Health <= 0)
        {
            Death();
        }
    }

    private void Death() 
    {
        Destroy(gameObject);
    }
}
