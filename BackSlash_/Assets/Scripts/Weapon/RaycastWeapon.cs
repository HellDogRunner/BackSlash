using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class RaycastWeapon : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] muzzleFlash;
    [SerializeField] private ParticleSystem hitEffect;
    [SerializeField] private Transform raycastOrigin;

    private Vector3 _destinationOrigin;

    private bool isFiring = false;

    private Ray ray;
    private Ray camRay;
    private RaycastHit hitInfo;
    private RaycastHit camhitInfo;

    Camera mainCamera;


    private void Awake()
    {
        mainCamera = Camera.main;
    }

    public void StartFiring() 
    {
        camRay.origin = mainCamera.transform.position;
        camRay.direction = mainCamera.transform.forward;
        if (Physics.Raycast(camRay, out camhitInfo))
        {
            _destinationOrigin = camhitInfo.point;
        }
        else _destinationOrigin = ray.origin * 1000f;

        isFiring = true;
        foreach (var particle in muzzleFlash)
        {
            particle.Emit(1);
        }
        ray.origin = raycastOrigin.position;
        ray.direction = _destinationOrigin - raycastOrigin.position;

        if (Physics.Raycast(ray,out hitInfo))
        {
            //Debug.DrawLine(raycastOrigin.position, hitInfo.point, Color.red, 1f);
            hitEffect.transform.position = hitInfo.point;
            hitEffect.transform.forward = hitInfo.normal;
            hitEffect.Emit(1);
        }
    }

    public void StopFiring() 
    {
        isFiring = false;
    }
}
