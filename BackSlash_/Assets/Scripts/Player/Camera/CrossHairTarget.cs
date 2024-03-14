using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CrossHairTarget : MonoBehaviour
{
    Camera mainCamera;
    Ray ray;
    RaycastHit hitInfo;

    public Vector3 CrossHairTargetPosition => transform.position;

    [Inject]
    private void Construct()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        ray.origin = mainCamera.transform.position;
        ray.direction = mainCamera.transform.forward;
        if (Physics.Raycast(ray, out hitInfo))
        {
            transform.position = hitInfo.point;
        }
        else
        {
            transform.position = ray.origin + ray.direction * 1000.0f;
        }
    }
}
