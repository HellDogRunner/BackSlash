using UnityEngine;

public class HitBox : MonoBehaviour
{
    private HealthController _healthController;

    private Collider _collider;

    private void Awake()
    {
        _healthController = GetComponentInParent<HealthController>();
        _healthController.OnDeath += EnableCollision;
        _collider = GetComponent<Collider>();
    }

    private void OnDestroy()
    {
        _healthController.OnDeath -= EnableCollision;
    }

    private void EnableCollision()
    {
        _collider.isTrigger = false;
    }
}
