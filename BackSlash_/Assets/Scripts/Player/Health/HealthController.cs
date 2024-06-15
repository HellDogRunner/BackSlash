using Scripts.Enemy;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    [SerializeField] private float _health;
    [SerializeField] private float _timeToDestroyLeft = 5f;

    private Ragdoll _ragdoll;
    private bool isDead;

    public float Health => _health;
    public bool IsDead => isDead;

    public event Action<float> OnHealthChanged;
    public event Action<GameObject> OnEnemyTakeDamage;
    public event Action OnDeath;

    private void Start() 
    { 
        _ragdoll = GetComponent<Ragdoll>();

        var rigidbodies = GetComponentsInChildren<Rigidbody>();
        foreach (var rigidbody in rigidbodies)
        {
            HitBox hitBox = rigidbody.gameObject.AddComponent<HitBox>();
            hitBox.Health = this;
        }
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
            OnEnemyTakeDamage?.Invoke(gameObject);
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
