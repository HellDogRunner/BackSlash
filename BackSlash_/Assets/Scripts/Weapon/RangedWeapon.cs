using Scripts.Entity;
using System.Collections.Generic;
using UnityEngine;

public class RangedWeapon : MonoBehaviour
{
    class Bullet
    {
        public float time;
        public Vector3 initialPosition;
        public Vector3 initialVelocity;
        public TrailRenderer tracer;
        public AttackModel attack;
    }

    [SerializeField] private ParticleSystem[] _muzzleFlash;
    [SerializeField] private ParticleSystem _hitEffect;
    [SerializeField] private Transform _raycastOrigin;
    [SerializeField] private TrailRenderer _tracerEffect;

    [Header("BulletSettings")]
    //TODO replace shooting setting in database
    [SerializeField] private float _bulletSpeed = 1000;
    [SerializeField] private float _bulletDrop = 0f;
    [SerializeField] private float _inaccuracyRadius = 0f;
    [SerializeField] private float _accuracyPercent = 100f;
    [SerializeField] private float _missShotRadius = 0f;
    [SerializeField] private LayerMask _hitboxLayer;

    private float _maxLifeTime = 3;

    private List<Bullet> _bullets = new List<Bullet>();

    private Ray ray;
    private RaycastHit _hitInfo;

    private Entity _entity;
    private AttackModel _attack;
    private Vector3 _target;

    private void Awake()
    {
        _entity = GetComponentInParent<Entity>();
    }

    private void OnEnable()
    {
        _entity.OnSetAttack += SetAttack;
        _entity.OnSetTarget += SetTarget;
        _entity.OnStartAttack += FireBullet;
    }

    private void OnDisable()
    {
        _entity.OnSetAttack -= SetAttack;
        _entity.OnSetTarget -= SetTarget;
        _entity.OnStartAttack -= FireBullet;
    }

    private void Update()
    {
        if (_bullets.Count > 0)
        {
            SimulateBullets(Time.deltaTime);
            DestroyBullets();
        }
    }

    private void SetAttack(AttackModel attack)
    {
        if (!attack.Ranged) return;

        _attack = attack;
    }

    private void SetTarget(Vector3 target) => _target = SetBulletAccuracy(target);

    private Vector3 GetPosition(Bullet bullet)
    {
        Vector3 gravity = Vector3.down * _bulletDrop;
        return bullet.initialPosition + (bullet.initialVelocity * bullet.time) + (0.5f * gravity * bullet.time * bullet.time);
    }

    private Bullet CreateBullet(Vector3 position, Vector3 velocity, AttackModel attack)
    {
        Bullet bullet = new Bullet();

        bullet.initialPosition = position;
        bullet.initialVelocity = velocity;
        bullet.time = 0f;
        bullet.tracer = CreateTracer(bullet.initialPosition);
        bullet.tracer.AddPosition(position);
        bullet.attack = attack;

        return bullet;
    }

    private Vector3 SetBulletAccuracy(Vector3 target)
    {
        target += new Vector3(0, 1, 0);

        var percent = Random.Range(1, 100);

        if (_accuracyPercent >= percent)
        {
            target += Random.insideUnitSphere * _inaccuracyRadius;
        }
        else
        {
            target += Random.insideUnitSphere * _missShotRadius;
        }

        return target;
    }

    private void DestroyBullets()
    {
        _bullets.ForEach(bullet =>
        {
            if (bullet.time >= _maxLifeTime)
            {
                Destroy(bullet.tracer.gameObject);
            }
        });

        _bullets.RemoveAll(bullet => bullet.time >= _maxLifeTime);
    }

    private void DestroyBullets(Bullet bullet)
    {
        Destroy(bullet.tracer.gameObject);
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

        if (bullet.tracer == null)
        {
            bullet.tracer = CreateTracer(bullet.initialPosition);
        }

        if (Physics.Raycast(ray, out _hitInfo, distance, _hitboxLayer, QueryTriggerInteraction.Collide))
        {
            _hitEffect.transform.position = _hitInfo.point;
            _hitEffect.transform.forward = _hitInfo.normal;
            _hitEffect.Emit(1);

            bullet.tracer.transform.position = _hitInfo.point;
            bullet.time = _maxLifeTime;

            if (_hitInfo.collider.TryGetComponent(out HitBox hitbox))
            {
                hitbox.AttackTaken(bullet.attack);
                DestroyBullets(bullet);
            }
        }
        else
        {
            bullet.tracer.transform.position = end;
        }
    }

    private void FireBullet()
    {
        if (_attack == null) return;

        foreach (var particle in _muzzleFlash)
        {
            particle.Emit(1);
        }

        Vector3 velocity = (_target - _raycastOrigin.position).normalized * _bulletSpeed;
        var bullet = CreateBullet(_raycastOrigin.position, velocity, _attack);
        _attack = null;
        _bullets.Add(bullet);
    }

    private TrailRenderer CreateTracer(Vector3 position)
    {
        return Instantiate(_tracerEffect, position, Quaternion.identity);
    }
}
