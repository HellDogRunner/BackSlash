using UnityEngine;

public class HitBox : MonoBehaviour
{
    private HealthController _healthController;

    private Collider _collider;

    private void Awake()
    {
        _healthController = this.GetComponentInParent<HealthController>();
        _healthController.OnDeath += EnableCollision;

        _collider = this.GetComponent<Collider>();
    }

    private void OnDestroy()
    {
        _healthController.OnDeath -= EnableCollision;
    }

    public void OnRangedHit(float damage)
    {
        _healthController.TakeDamage(damage);
    }

    public void OnMeleeHit(float damage)
    {
        _healthController.TakeDamage(damage);
    }

    private void EnableCollision()
    {
        _collider.isTrigger = false;
    }
}
