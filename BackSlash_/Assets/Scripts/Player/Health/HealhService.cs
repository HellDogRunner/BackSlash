using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealhService : MonoBehaviour
{
    [SerializeField] private int health;

    public Action<int> OnHealthChanged;

    public void TakeDamage(int damage, object sender = null) 
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
        Destroy(gameObject);
    }
}
