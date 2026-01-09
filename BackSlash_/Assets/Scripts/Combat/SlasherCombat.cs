using System.Collections;
using UnityEngine;

namespace Scripts.Combat
{
    public class SlasherCombat : MonoBehaviour
    {
        [Header("Refs")]
        [SerializeField] private Animator _animator;              // Animator на Model
        [SerializeField] private CharacterController _cc;         // на Root
        [SerializeField] private Scripts.Player.SlasherInput _input;
        [SerializeField] private SlasherTargeting _targeting;
        [SerializeField] private WeaponHitbox _hitbox;

        [Header("Damage")]
        [SerializeField] private int _lightDamage = 12;
        [SerializeField] private int _heavyDamage = 22;

        [Header("Facing")]
        [SerializeField] private float _faceTurnSpeed = 18f;

        [Header("Magnet / Lunge (GoW-ish)")]
        [SerializeField] private float _magnetRange = 6.5f;     // работаем только если цель не далеко
        [SerializeField] private float _stopDistance = 2.3f;    // хотим закончить примерно на этой дистанции
        [SerializeField] private float _maxStepLight = 1.1f;
        [SerializeField] private float _maxStepHeavy = 1.6f;
        [SerializeField] private float _timeLight = 0.10f;
        [SerializeField] private float _timeHeavy = 0.14f;

        [Header("Gate")]
        [SerializeField] private float _attackLockTime = 0.18f;

        private float _lockedUntil;
        private int _attackId;
        private Coroutine _magnetRoutine;

        private static readonly int LightAttack = Animator.StringToHash("LightAttack");
        private static readonly int HeavyAttack = Animator.StringToHash("HeavyAttack");

        private void Awake()
        {
            if (_cc == null) _cc = GetComponent<CharacterController>();
            if (_input == null) _input = GetComponent<Scripts.Player.SlasherInput>();

            // Animator обычно на Model, можно найти автоматически
            if (_animator == null) _animator = GetComponentInChildren<Animator>();

            if (_hitbox != null) _hitbox.SetOwner(transform);
        }

        private void OnEnable()
        {
            if (_input == null) return;
            _input.LightPressed += OnLight;
            _input.HeavyPressed += OnHeavy;
        }

        private void OnDisable()
        {
            if (_input == null) return;
            _input.LightPressed -= OnLight;
            _input.HeavyPressed -= OnHeavy;
        }

        private void OnLight()
        {
            if (Time.time < _lockedUntil) return;
            _attackId++;
            _lockedUntil = Time.time + _attackLockTime;

            FaceTargetOrCamera();
            StartMagnet(_maxStepLight, _timeLight);

            _animator.SetTrigger(LightAttack);
        }

        private void OnHeavy()
        {
            if (Time.time < _lockedUntil) return;
            _attackId++;
            _lockedUntil = Time.time + _attackLockTime;

            FaceTargetOrCamera();
            StartMagnet(_maxStepHeavy, _timeHeavy);

            _animator.SetTrigger(HeavyAttack);
        }

        private void FaceTargetOrCamera()
        {
            Vector3 dir = Vector3.zero;

            if (_targeting != null && _targeting.CurrentTarget != null)
            {
                dir = _targeting.CurrentTarget.position - transform.position;
                dir.y = 0f;
            }
            else if (Camera.main != null)
            {
                dir = Camera.main.transform.forward;
                dir.y = 0f;
            }

            if (dir.sqrMagnitude < 0.0001f) return;

            Quaternion target = Quaternion.LookRotation(dir.normalized, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, target, _faceTurnSpeed * Time.deltaTime);
        }

        private void StartMagnet(float maxStep, float time)
        {
            if (_targeting == null || _targeting.CurrentTarget == null || _cc == null) return;

            Vector3 to = _targeting.CurrentTarget.position - transform.position;
            to.y = 0f;
            float dist = to.magnitude;

            if (dist < 0.001f) return;
            if (dist > _magnetRange) return;
            if (dist <= _stopDistance) return;

            float step = Mathf.Min(maxStep, dist - _stopDistance);
            Vector3 dir = to / dist;

            if (_magnetRoutine != null) StopCoroutine(_magnetRoutine);
            _magnetRoutine = StartCoroutine(MagnetRoutine(dir, step, time));
        }

        private IEnumerator MagnetRoutine(Vector3 dir, float distance, float time)
        {
            float speed = distance / Mathf.Max(0.01f, time);
            float t = 0f;

            while (t < time)
            {
                _cc.Move(dir * (speed * Time.deltaTime));
                t += Time.deltaTime;
                yield return null;
            }

            _magnetRoutine = null;
        }

        // Эти методы будут вызываться через relay с Model (Animator)
        public void AE_HitboxOnLight() => _hitbox?.Activate(_lightDamage);
        public void AE_HitboxOnHeavy() => _hitbox?.Activate(_heavyDamage);
        public void AE_HitboxOff() => _hitbox?.Deactivate();
    }
}
