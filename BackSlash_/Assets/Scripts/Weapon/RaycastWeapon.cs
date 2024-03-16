using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class RaycastWeapon : MonoBehaviour
{
    class Bullet 
    {
        public float time;
        public Vector3 initialPosition;
        public Vector3 initialVelocity;
        public TrailRenderer tracer;
    }

    [SerializeField] private ParticleSystem[] muzzleFlash;
    [SerializeField] private ParticleSystem hitEffect;
    [SerializeField] private Transform raycastOrigin;
    [SerializeField] private TrailRenderer tracerEffect;
    [Header("BulletSettings")]

    [SerializeField] private int fireRate = 25;
    [SerializeField] private float bulletSpeed = 1000;
    [SerializeField] private float bulletDrop = 0f;

    private float _accumulatedTime;
    private float _maxLifeTime = 3f;

    private List<Bullet> _bullets = new List<Bullet>();

    private Ray ray;
    private RaycastHit hitInfo;

    private Camera mainCamera;

    private bool _isFiring = false;
    public bool IsFiring => _isFiring;

    private Transform _raycastDestination;
    public Transform RaycastDestination 
    {
        get => _raycastDestination;
        set => _raycastDestination = value;
    }

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private Vector3 GetPosition(Bullet bullet) 
    {
        Vector3 gravity = Vector3.down * bulletDrop;
        return (bullet.initialPosition) + (bullet.initialVelocity * bullet.time) + (0.5f * gravity * bullet.time * bullet.time);
    }

    private Bullet CreateBullet(Vector3 position, Vector3 velocity)
    {
        Bullet bullet = new Bullet();

        bullet.initialPosition = position;
        bullet.initialVelocity = velocity;
        bullet.time = 0f;
        bullet.tracer = Instantiate(tracerEffect, position, Quaternion.identity);
        bullet.tracer.AddPosition(position);
        return bullet;
    }

    public void StartFiring()
    {
        _isFiring = true;
        _accumulatedTime = 0f;
        FireBullet();
    }

    public void UpdateFiring(float deltaTime) 
    {
        _accumulatedTime += deltaTime;
        float fireInterval = 1f / fireRate;
        while (_accumulatedTime >= 0f)
        {
            FireBullet();
            _accumulatedTime -= fireInterval;
        }
    }

    public void UpdateBullets(float deltaTime)
    {
        SimulateBullets(deltaTime);
        DestroyBullets();
    }

    private void DestroyBullets()
    {
        _bullets.RemoveAll(bullet => bullet.time >= _maxLifeTime);
    }

    public void SimulateBullets(float deltaTime)
    {
        _bullets.ForEach(bullet =>
        {
            Vector3 p0 = GetPosition(bullet);
            bullet.time += deltaTime;
            Vector3 p1 = GetPosition(bullet);
            RayCastSegment(p0, p1, bullet);
        });
    }

    private void RayCastSegment(Vector3 start, Vector3 end, Bullet bullet)
    {
        Vector3 direction = end - start;
        float distance = direction.magnitude;
        ray.origin = start;
        ray.direction = direction;

        if (Physics.Raycast(ray, out hitInfo, distance))
        {
            hitEffect.transform.position = hitInfo.point;
            hitEffect.transform.forward = hitInfo.normal;
            hitEffect.Emit(1);

            bullet.tracer.transform.position = hitInfo.point;
            bullet.time = _maxLifeTime;
        }
        else {
            bullet.time = _maxLifeTime;
            bullet.tracer.transform.position = end; 
        }
    }

    private void FireBullet()
    {
        foreach (var particle in muzzleFlash)
        {
            particle.Emit(1);
        }
        Vector3 velocity = (_raycastDestination.position - raycastOrigin.position).normalized * bulletSpeed;
        var bullet = CreateBullet(raycastOrigin.position, velocity);
        _bullets.Add(bullet);    
    }

    public void StopFiring() 
    {
        _isFiring = false;
    }
}
