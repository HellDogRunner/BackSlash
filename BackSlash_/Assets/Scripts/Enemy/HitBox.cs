using UnityEngine;

public class HitBox : MonoBehaviour
{
    private HealthController _healthController;

    private void Awake()
    {
        _healthController = this.GetComponentInParent<HealthController>();
        var collider = this.GetComponent<Collider>();
        collider.isTrigger = true;
        gameObject.layer = 7;
    }

    public void OnRangedHit(float damage)
    {
        _healthController.TakeDamage(damage);
    }

    public void OnMeleeHit(float damage)
    {
        _healthController.TakeDamage(damage);
    }
}
