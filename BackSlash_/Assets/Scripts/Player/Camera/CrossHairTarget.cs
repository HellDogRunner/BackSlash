using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CrossHairTarget : MonoBehaviour
{
    [SerializeField] LayerMask attackLayer;

    private Camera _mainCamera;

    Ray ray;
    RaycastHit hitInfo;

    public Vector3 CrossHairTargetPosition => transform.position;

    [Inject]
    private void Construct()
    {
        _mainCamera = Camera.main;
    }

    void Update()
    {
        ray.origin = _mainCamera.transform.position;
        ray.direction = _mainCamera.transform.forward;
        if (Physics.Raycast(ray, out hitInfo, attackLayer))
        {
            transform.position = hitInfo.point;
        }
        else
        {
            transform.position = ray.origin + ray.direction * 1000.0f;
        }
    }
}
