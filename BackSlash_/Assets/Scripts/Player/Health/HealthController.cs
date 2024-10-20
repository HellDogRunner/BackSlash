using System;
using System.Collections;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    [SerializeField] private float _health;
    [SerializeField] private float _timeToDestroyLeft = 5f;
    [SerializeField] private float _blinkIntensity;
    [SerializeField] private float _blinkDuration;

    private float _blinkTimer;
    private SkinnedMeshRenderer _skinnedMeshRenderer;

    private Ragdoll _ragdoll;
    private bool _isDead;

    public float Health => _health;
    public bool IsDead => _isDead;

    public event Action<float> OnHealthChanged;
    public event Action OnDamageTaken;
    public event Action OnDeath;

    private void Start()
    {
        _skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        _ragdoll = GetComponent<Ragdoll>();
        SetupKinematics();
    }

    private void Update()
    {
        _blinkTimer -= Time.deltaTime;
        float lerp = Mathf.Clamp01(_blinkTimer / _blinkDuration);
        float intensity = (lerp * _blinkIntensity) + 1.0f;
        _skinnedMeshRenderer.material.color = Color.white * intensity;
    }

    private void SetupKinematics()
    {
        var rigidbodies = GetComponentsInChildren<Rigidbody>();
        foreach (var rigidbody in rigidbodies)
        {
            rigidbody.isKinematic = true;
        }
    }

    public void TakeDamage(float damage)
    {
        if (!_isDead)
        {
            _blinkTimer = _blinkDuration;

            _health -= damage;

            if (_health <= 0)
            {
                Death();
                _health = 0;
            }
            OnHealthChanged?.Invoke(_health);
            OnDamageTaken?.Invoke();
        }
    }

    private void Death()
    {
        _isDead = true;
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
