using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CrossHairTarget : MonoBehaviour
{
    [SerializeField] LayerMask _attackLayer;

    private Camera _mainCamera;

    private Ray _ray;
    private RaycastHit _hitInfo;

    public Vector3 CrossHairTargetPosition => transform.position;

    [Inject]
    private void Construct()
    {
        _mainCamera = Camera.main;
    }

    void Update()
    {
        _ray.origin = _mainCamera.transform.position;
        _ray.direction = _mainCamera.transform.forward;
        if (Physics.Raycast(_ray, out _hitInfo, _attackLayer))
        {
            transform.position = _hitInfo.point;
        }
        else
        {
            transform.position = _ray.origin + _ray.direction * 1000.0f;
        }
    }
}
