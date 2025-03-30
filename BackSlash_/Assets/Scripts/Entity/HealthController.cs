using System;
using System.Collections;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    [field: SerializeField]public int Health { get; private set; }
    [SerializeField] private float _timeToDestroyLeft;
    [SerializeField] private float _blinkIntensity;
    [SerializeField] private float _blinkDuration;

    private SkinnedMeshRenderer _skinnedMeshRenderer;
    private Ragdoll _ragdoll;
    
    private float _blinkTimer;
    
    private Coroutine _blink;
    
    public bool IsDead { get; private set; }

    public event Action<int, int> OnHealthChanged;
    public event Action OnDamageTaken;
    public event Action OnDeath;

    private void Start()
    {
        _skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        _ragdoll = GetComponent<Ragdoll>();
        SetupKinematics();
    }

    private void SetupKinematics()
    {
        var rigidbodies = GetComponentsInChildren<Rigidbody>();
        foreach (var rigidbody in rigidbodies)
        {
            rigidbody.isKinematic = true;
        }
    }

    public void OnAwake(int health)
    {
        Health = health;
    }

    public void TakeDamage(int damage)
    {
       if (!IsDead)
        {
            if (_blink != null) StopCoroutine(_blink);
            _blink = StartCoroutine(BlinkEntity());
            
            Health -= damage;

            if (Health <= 0)
            {
                Death();
                Health = 0;
            }
            
            OnHealthChanged?.Invoke(Health + damage, Health);
            OnDamageTaken?.Invoke();
        } 
    }

    private void Death()
    {
        IsDead = true;
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

    private IEnumerator BlinkEntity()
    {
        _skinnedMeshRenderer.material.color = Color.white;
        _blinkTimer = _blinkDuration;
    
        while (_blinkTimer > 0)
        {
            yield return null; 
            _blinkTimer -= Time.deltaTime;
            float lerp = Mathf.Clamp01(_blinkTimer / _blinkDuration);
            float intensity = (lerp * _blinkIntensity) + 1.0f;
            _skinnedMeshRenderer.material.color = Color.white * intensity;
        }   
    }
    
    private IEnumerator DestroyObject()
    {
        yield return new WaitForSeconds(_timeToDestroyLeft);
        Destroy(gameObject);
    }
}
