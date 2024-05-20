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

    [SerializeField] private ParticleSystem[] _muzzleFlash;
    [SerializeField] private ParticleSystem _hitEffect;
    [SerializeField] private Transform _raycastOrigin;
    [SerializeField] private TrailRenderer _tracerEffect;
    [SerializeField] private AnimationClip _weaponAnimation;
    [Header("BulletSettings")]

    [SerializeField] private float _fireRate = 25;
    [SerializeField] private float _bulletSpeed = 1000;
    [SerializeField] private float _bulletDrop = 0f;
    [SerializeField] private float _damage = 0f;
    [SerializeField] private float _inaccuracyRadius = 0f;
    [SerializeField] private float _accuracyPercent = 0f;
    [SerializeField] private float _missShotRadius = 0f;

    private float _accumulatedTime;
    private float _maxLifeTime = 3;

    private List<Bullet> _bullets = new List<Bullet>();

    private Ray ray;
    private RaycastHit hitInfo;
    private LayerMask _hitboxLayer;

    private bool _isFiring = false;

    public bool IsFiring => _isFiring;

    public AnimationClip WeaponAnimation => _weaponAnimation;


    private void Awake()
    {
        _hitboxLayer = LayerMask.GetMask("Player");
        _hitboxLayer = ~_hitboxLayer;
    }

    private Vector3 GetPosition(Bullet bullet) 
    {
        Vector3 gravity = Vector3.down * _bulletDrop;
        return (bullet.initialPosition) + (bullet.initialVelocity * bullet.time) + (0.5f * gravity * bullet.time * bullet.time);
    }

    private Bullet CreateBullet(Vector3 position, Vector3 velocity)
    {
        Bullet bullet = new Bullet();

        bullet.initialPosition = position;
        bullet.initialVelocity = velocity;
        bullet.time = 0f;
        bullet.tracer = Instantiate(_tracerEffect, position, Quaternion.identity);
        bullet.tracer.AddPosition(position);
        return bullet;
    }

    public void StartFiring()
    {
        _isFiring = true;
        if (_accumulatedTime > 0f)
        {
            _accumulatedTime = 0f;
        }
    }

    public void UpdateFiring(float deltaTime, Vector3 target) 
    {
        if (_isFiring)
        {
            _accumulatedTime += deltaTime;
            float fireInterval = 1f / _fireRate;
            while (_accumulatedTime >= 0f)
            {
                var percent = UnityEngine.Random.Range(1, 100);
                if (_accuracyPercent >= percent)
                {
                    target += UnityEngine.Random.insideUnitSphere * _inaccuracyRadius;    
                }
                else
                {
                    target += UnityEngine.Random.insideUnitSphere * _missShotRadius;
                    
                }
                FireBullet(target);
                _accumulatedTime -= fireInterval;
            }
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

        if (Physics.Raycast(ray, out hitInfo, distance, _hitboxLayer))
        {
            _hitEffect.transform.position = hitInfo.point;
            _hitEffect.transform.forward = hitInfo.normal;
            _hitEffect.Emit(1);

            bullet.tracer.transform.position = hitInfo.point;
            bullet.time = _maxLifeTime;
            Debug.Log(hitInfo.collider.name);

            if (hitInfo.transform.TryGetComponent<HealthService>(out HealthService T))
            {
                T.TakeDamage(_damage);
            }

        }
        else {
            bullet.tracer.transform.position = end; 
        }
    }

    private void FireBullet(Vector3 target)
    {
        foreach (var particle in _muzzleFlash)
        {
            particle.Emit(1);
        }
        Vector3 velocity = (target - _raycastOrigin.position).normalized * _bulletSpeed;
        var bullet = CreateBullet(_raycastOrigin.position, velocity);
        _bullets.Add(bullet);    
    }

    public void StopFiring() 
    {
        _isFiring = false;
    }

}
