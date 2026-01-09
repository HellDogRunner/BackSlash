using System;
using UnityEngine;

namespace Scripts.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class RocketBootsMotor : MonoBehaviour
    {
        [SerializeField] private CharacterController _cc;
        [SerializeField] private SlasherInput _input;

        [Header("Ground Probe")]
        [SerializeField] private LayerMask _groundMask;
        [SerializeField] private float _probeDistance = 120f; // сделай с запасом, если прыгаешь высоко

        [Header("Hover (spring)")]
        [SerializeField] private float _hoverHeight = 2.0f;   // высота ОТ НИЗА капсулы
        [SerializeField] private float _hoverSpring = 60f;
        [SerializeField] private float _hoverDamping = 12f;

        [Header("Jump (shape)")]
        [SerializeField] private float _jumpHeightUnits = 8f; // на сколько выше hover
        [SerializeField] private float _timeToApex = 0.55f;   // больше = мягче
        [SerializeField] private float _fallMultiplier = 1.15f;

        [Header("Speed limits (feel)")]
        [SerializeField] private float _maxRiseSpeed = 16f;
        [SerializeField] private float _maxFallSpeed = 10f;

        [Header("Landing / Re-jump gating (THE IMPORTANT PART)")]
        [SerializeField] private float _landHeightEpsilon = 0.75f; // ✅ в пределах ±0.75 от hover считаем "вернулся"
        [SerializeField] private float _landSpeedEpsilon = 4.0f;   // ✅ не ждём идеального нуля
        [SerializeField] private float _landStableTime = 0.06f;    // ✅ коротко, чтобы не залипало

        [SerializeField] private float _jumpCooldown = 0.05f;

        [Header("Horizontal (Inertia / Drift)")]
        [SerializeField] private float _cruiseSpeed = 8.5f;
        [SerializeField] private float _sprintSpeed = 11f;
        [SerializeField] private float _accel = 28f;
        [SerializeField] private float _decel = 12f;
        [SerializeField, Range(0f, 1f)] private float _strafeDrift = 0.25f;

        [Header("Rotation")]
        [SerializeField] private float _turnSpeed = 18f;

        [Header("Debug")]
        [SerializeField] private bool _debugDraw;

        public event Action Jumped;
        public event Action Landed;

        public bool BootsActive { get; private set; }

        // Animator API
        public bool Grounded => !_inAir;
        public float VerticalSpeed => _yVel;
        public float Throttle01 { get; private set; }

        private Transform _cam;

        private Vector3 _hVel;
        private float _yVel;

        private bool _inAir;
        private float _stableTimer;
        private float _nextJumpTime;

        // jump math
        private float _jumpGravity; // positive
        private float _jumpV0;

        private void Reset() => _cc = GetComponent<CharacterController>();

        private void Awake()
        {
            if (_cc == null) _cc = GetComponent<CharacterController>();
            if (_input == null) _input = GetComponent<SlasherInput>();
            _cam = Camera.main != null ? Camera.main.transform : null;

            RecalcJumpMath();
            enabled = false;
        }

        private void OnEnable()
        {
            if (_input != null)
                _input.JumpPressed += OnJumpPressed;
        }

        private void OnDisable()
        {
            if (_input != null)
                _input.JumpPressed -= OnJumpPressed;
        }

        public void SetActive(bool active)
        {
            BootsActive = active;
            enabled = active;

            if (active)
            {
                _hVel = Vector3.zero;
                _yVel = 0f;
                _inAir = false;
                _stableTimer = 0f;
                _nextJumpTime = 0f;
            }
        }

        private void Update()
        {
            RecalcJumpMath();

            RotateToCameraForward();
            ApplyHorizontalMoveWithInertia();

            float dist = GetGroundDistanceFromFeet();

            if (!_inAir)
            {
                // hover держит высоту
                ApplyHoverSpring(dist);
            }
            else
            {
                // в прыжке hover не тянет вниз
                ApplyJumpGravity();

                // лимиты
                if (_yVel > _maxRiseSpeed) _yVel = _maxRiseSpeed;
                if (_yVel < -_maxFallSpeed) _yVel = -_maxFallSpeed;

                // ✅ приземление только по band-логике (без wantsAir)
                UpdateLanding(dist);
            }

            MoveCharacter();
            UpdateThrottle();

            if (_debugDraw) DebugStuff(dist);
        }

        private void OnJumpPressed()
        {
            if (!BootsActive) return;
            if (Time.time < _nextJumpTime) return;
            if (_inAir) return; // нельзя повторно пока не приземлился на hover

            _nextJumpTime = Time.time + _jumpCooldown;

            _inAir = true;
            _stableTimer = 0f;

            _yVel = Mathf.Max(_yVel, _jumpV0);
            Jumped?.Invoke();
        }

        // ---------- Hover ----------
        private void ApplyHoverSpring(float feetToGround)
        {
            float error = feetToGround - _hoverHeight;
            float accel = (-error * _hoverSpring) - (_yVel * _hoverDamping);
            _yVel += accel * Time.deltaTime;

            // мягкие лимиты
            if (_yVel > _maxRiseSpeed) _yVel = _maxRiseSpeed;
            if (_yVel < -_maxFallSpeed) _yVel = -_maxFallSpeed;
        }

        // ---------- Jump math ----------
        private void RecalcJumpMath()
        {
            float H = Mathf.Max(0.01f, _jumpHeightUnits);
            float T = Mathf.Max(0.05f, _timeToApex);

            _jumpGravity = 2f * H / (T * T);
            _jumpV0 = 2f * H / T;
        }

        private void ApplyJumpGravity()
        {
            float g = _jumpGravity;
            if (_yVel < 0f) g *= Mathf.Max(0.05f, _fallMultiplier);
            _yVel -= g * Time.deltaTime;
        }

        // ---------- Landing (FIX) ----------
        private void UpdateLanding(float feetToGround)
        {
            float heightErr = Mathf.Abs(feetToGround - _hoverHeight);
            bool nearHover = heightErr <= _landHeightEpsilon;

            // Быстрый "anti-stuck": если уже близко к hover и мы падаем/почти не движемся вверх — считаем что приземлились
            if (nearHover && _yVel <= 0.5f)
            {
                // ждём небольшую стабильность по скорости, но не требуем идеального 0
                if (Mathf.Abs(_yVel) <= _landSpeedEpsilon) _stableTimer += Time.deltaTime;
                else _stableTimer = 0f;

                if (_stableTimer >= _landStableTime)
                {
                    _inAir = false;
                    _stableTimer = 0f;

                    // сброс вертикальной скорости — чтобы hover сразу подхватил и не лип к земле
                    _yVel = 0f;

                    Landed?.Invoke();
                }

                return;
            }

            // если мы далеко от hover — сбрасываем таймер
            _stableTimer = 0f;
        }

        // ---------- Ground dist from FEET ----------
        private float GetGroundDistanceFromFeet()
        {
            // start slightly above controller center
            Vector3 origin = _cc.bounds.center + Vector3.up * 0.05f;
            float extY = _cc.bounds.extents.y;

            if (Physics.Raycast(origin, Vector3.down, out var hit, _probeDistance, _groundMask, QueryTriggerInteraction.Ignore))
            {
                float feetDist = hit.distance - extY;
                return Mathf.Max(0f, feetDist);
            }

            // если не достали — считаем что далеко
            return _probeDistance;
        }

        // ---------- Horizontal ----------
        private void ApplyHorizontalMoveWithInertia()
        {
            if (_cam == null || _input == null)
            {
                _hVel = Vector3.MoveTowards(_hVel, Vector3.zero, _decel * Time.deltaTime);
                return;
            }

            Vector2 input = _input.Move;

            Vector3 forward = _cam.forward; forward.y = 0f; forward.Normalize();
            Vector3 right = _cam.right; right.y = 0f; right.Normalize();

            Vector3 wishDir = (input.y * forward + input.x * right);
            if (wishDir.sqrMagnitude > 0.0001f) wishDir.Normalize();

            float maxSpeed = (_input.SprintHeld ? _sprintSpeed : _cruiseSpeed);
            Vector3 targetVel = wishDir * (input == Vector2.zero ? 0f : maxSpeed);

            float rate = (targetVel.sqrMagnitude > _hVel.sqrMagnitude) ? _accel : _decel;

            if (input != Vector2.zero && _hVel.sqrMagnitude > 0.01f && _strafeDrift > 0f)
            {
                float dot = Vector3.Dot(_hVel.normalized, wishDir);
                if (dot < 0.3f) rate *= (1f - _strafeDrift);
            }

            _hVel = Vector3.MoveTowards(_hVel, targetVel, rate * Time.deltaTime);
        }

        private void RotateToCameraForward()
        {
            if (_cam == null) return;

            Vector3 face = _cam.forward;
            face.y = 0f;
            if (face.sqrMagnitude < 0.0001f) return;

            Quaternion target = Quaternion.LookRotation(face.normalized, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, target, _turnSpeed * Time.deltaTime);
        }

        private void MoveCharacter()
        {
            Vector3 vel = new Vector3(_hVel.x, _yVel, _hVel.z);
            _cc.Move(vel * Time.deltaTime);
        }

        private void UpdateThrottle()
        {
            float h01 = Mathf.InverseLerp(0f, _sprintSpeed, new Vector3(_hVel.x, 0f, _hVel.z).magnitude);
            float v01 = Mathf.InverseLerp(0f, Mathf.Max(0.01f, _maxRiseSpeed), Mathf.Max(0f, _yVel));
            Throttle01 = Mathf.Clamp01(Mathf.Max(h01, v01));
        }

        private void DebugStuff(float feetToGround)
        {
            Debug.DrawRay(transform.position + Vector3.up * 1.2f, new Vector3(_hVel.x, 0f, _hVel.z), Color.cyan);
            Debug.DrawRay(transform.position + Vector3.up * 1.2f, Vector3.up * _yVel, Color.magenta);
            Debug.DrawRay(transform.position, Vector3.up * (feetToGround - _hoverHeight), Color.yellow);
        }
        private void OnDrawGizmos()
        {
            if (_cc == null) return;

            float feetToGround = Application.isPlaying
                ? GetGroundDistanceFromFeet()
                : 0f;

            Vector3 feetPos = transform.position;
            Vector3 hoverPos = feetPos + Vector3.up * _hoverHeight;

            // ===== RAY TO GROUND =====
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(_cc.bounds.center + Vector3.up * 0.05f,
                            _cc.bounds.center + Vector3.up * 0.05f + Vector3.down * _probeDistance);

            // ===== FEET POSITION =====
            Gizmos.color = Color.white;
            Gizmos.DrawSphere(feetPos, 0.05f);

            // ===== CURRENT HEIGHT =====
            Gizmos.color = Color.red;
            Gizmos.DrawLine(feetPos, feetPos + Vector3.up * feetToGround);
            Gizmos.DrawSphere(feetPos + Vector3.up * feetToGround, 0.06f);

            // ===== HOVER TARGET =====
            Gizmos.color = Color.green;
            Gizmos.DrawLine(feetPos, hoverPos);
            Gizmos.DrawSphere(hoverPos, 0.08f);

            // ===== LAND / REJUMP BAND =====
            Gizmos.color = new Color(1f, 0.8f, 0f, 0.25f); // 🟡 полупрозрачный
            Gizmos.DrawCube(
                feetPos + Vector3.up * _hoverHeight,
                new Vector3(0.4f, _landHeightEpsilon * 2f, 0.4f)
            );

            // ===== IN AIR STATUS =====
            if (_inAir)
            {
                Gizmos.color = Color.magenta;
                Gizmos.DrawWireSphere(feetPos + Vector3.up * feetToGround, 0.15f);
            }
        }

    }

}
