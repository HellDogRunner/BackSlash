using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastWeapon : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] muzzleFlash;
    [SerializeField] private Transform raycastOrigin;
    [SerializeField] private Transform raycastDestination;

    private bool isFiring = false;

    private Ray ray;
    private RaycastHit hitInfo;

    private void StartFiring() 
    {
        isFiring = true;
        foreach (var particle in muzzleFlash)
        {
            particle.Emit(1);
        }

        ray.origin = raycastOrigin.position;
        ray.direction = raycastDestination.position - raycastOrigin.position;

        if (Physics.Raycast(ray, out hitInfo))
        {
            Debug.DrawLine(ray.origin, hitInfo.point, Color.red, 1f);
        }
    }

    private void StopFiring() 
    {
        isFiring = false;
    }
}
