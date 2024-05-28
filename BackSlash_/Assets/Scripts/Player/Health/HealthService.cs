using Scripts.Enemy;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthService : MonoBehaviour
{
    [SerializeField] private float _health;
    [SerializeField] private float _timeToDestroyLeft = 5f;

    private Ragdoll _ragdoll;
    private bool isDead;

    public float Health => _health;
    public bool IsDead => isDead;

    public event Action<float> OnHealthChanged;
    public event Action OnDeath;

    private void Start() 
    { 
        _ragdoll = GetComponent<Ragdoll>();
    }

    public void TakeDamage(float damage) 
    {
        if (!isDead)
        {
            _health -= damage;

            if (_health <= 0)
            {
                Death();
                _health = 0;
            }
            OnHealthChanged?.Invoke(_health);
        }
    }

    private void Death() 
    {
        isDead = true;
        if (_ragdoll)
        {
            _ragdoll.ActivateRagdoll();
            OnDeath?.Invoke();

            if (gameObject.tag == "Enemy")
            {
                StartCoroutine(DestroyObject());
            }
        }
    }

    private IEnumerator DestroyObject()
    {
        yield return new WaitForSeconds(_timeToDestroyLeft);
        Destroy(gameObject);
    }
}
