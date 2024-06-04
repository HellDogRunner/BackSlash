using Scripts.Weapon;
using Scripts.Weapon.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class HitBox : MonoBehaviour
{
    public HealthController Health;

    private void Awake()
    {
        Health = this.GetComponent<HealthController>();
    }

    public void OnRaycastHit(RaycastWeapon weapon)
    {
        Health.TakeDamage(weapon.Damage);
    }

    public void OnSwordHit(float damage)
    {
        Health.TakeDamage(damage);
    }

}
