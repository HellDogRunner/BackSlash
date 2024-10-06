using UnityEngine;

public class HitBox : MonoBehaviour
{
    public HealthController Health;

    private Collider _collider;

    private void Awake()
    {
        Health = this.GetComponent<HealthController>();
        _collider = GetComponent<Collider>();
        _collider.isTrigger = true;
    }

    public void OnRaycastHit(RaycastWeapon weapon)
    {
        Health.TakeDamage(weapon.Damage);
    }

    public void OnMeleeHit(float damage)
    {
        Health.TakeDamage(damage);
    }

}
