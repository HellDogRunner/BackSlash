using System;
using System.Collections;
using UnityEngine;

namespace Scripts.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class SlasherMotor : MonoBehaviour
    {
        [SerializeField] private CharacterController _cc;
        [SerializeField] private SlasherInput _input;

        [Header("Move")]
        [SerializeField] private float _moveSpeed = 6.5f;
        [SerializeField] private float _sprintSpeed = 9.0f;
        [SerializeField] private float _groundAccel = 85f;
        [SerializeField] private float _groundDecel = 110f;

        [Header("Air")]
        [SerializeField] private float _airSpeed = 6.0f;
        [SerializeField] private float _airAccel = 45f;
        [SerializeField, Range(0f, 1f)] private float _airControl = 0.9f;

        [Header("Rotation")]
        [SerializeField] private float _turnSpeed = 28f;

        [Header("Jump")]
        [SerializeField] private float _jumpHeight = 1.6f;
        [SerializeField] private float _jumpTime = 0.55f;
        [SerializeField] private float _coyoteTime = 0.12f;
        [SerializeField] private float _jumpBuffer = 0.12f;

        [Header("Gravity Feel")]
        [SerializeField] private float _fallMultiplier = 2.7f;
        [SerializeField] private float _jumpCutMultiplier = 0.55f;
        [SerializeField] private float _groundSnapY = -2.0f;

        [Header("Dodge")]
        [SerializeField] private float _dodgeSpeed = 12.5f;
        [SerializeField] private float _dodgeDuration = 0.22f;
        [SerializeField] private float _dodgeCooldown = 0.08f;

        [Header("Slide")]
        [SerializeField] private float _slideSpeed = 14.0f;
        [SerializeField] private float _slideDuration = 0.35f;
        [SerializeField] private float _slideFriction = 18f;

        [Header("Ground Check (IMPORTANT)")]
        [SerializeField] private float _sphereRadius = 0.24f;
        [SerializeField] private float _castDistance = 0.45f;
        [SerializeField] private LayerMask _groundMask;

        public event Action Jumped;
        public event Action Dodged;
        public event Action Slid;
        public float VerticalSpeed => _ySpeed;
        public bool InAirState => !_grounded;


        public bool Grounded => _grounded;

        /// <summary>Move in local space of character (x=strafe, y=forward). Good for 2D blend tree.</summary>
        public Vector2 LocalMove => _localMove;

        private Transform _cam;

        private Vector3 _hVel;
        private Vector3 _impulseVel;

        private float _gravityForce;
        private float _startJumpVelocity;
        private float _ySpeed;

        private bool _grounded;
        private Vector2 _localMove;

        private bool _inDodge;
        private bool _inSlide;

        private float _lastGroundedTime = -999f;
        private float _lastJumpPressedTime = -999f;
        private float _nextDodgeAllowedTime = 0f;

        private void Reset()
        {
            _cc = GetComponent<CharacterController>();
        }

        private void Awake()
        {
            if (_cc == null) _cc = GetComponent<CharacterController>();
            if (_input == null) _input = GetComponent<SlasherInput>();

            _cam = Camera.main != null ? Camera.main.transform : null;
            RecalcJump();
        }

        private void OnEnable()
        {
            if (_input == null) return;
            _input.JumpPressed += OnJumpPressed;
            _input.DodgePressed += OnDodgePressed;
            _input.SlidePressed += OnSlidePressed;
        }

        private void OnDisable()
        {
            if (_input == null) return;
            _input.JumpPressed -= OnJumpPressed;
            _input.DodgePressed -= OnDodgePressed;
            _input.SlidePressed -= OnSlidePressed;
        }

        private void Update()
        {
            UpdateGrounded();

            // jump buffer
            HandleJump();

            // normal move unless in actions
            if (!_inDodge && !_inSlide)
                ApplyMove();

            ApplyVertical();
            RotateToCamera();
            MoveCharacter();

            ComputeLocalMoveForAnimator();
        }

        private void UpdateGrounded()
        {
            // Most reliable combo:
            // - CC.isGrounded catches some cases
            // - SphereCast catches slopes/steps better when tuned
            bool ccGrounded = _cc.isGrounded;

            Vector3 origin = transform.position + _cc.center + Vector3.up * 0.1f;
            bool castGrounded = Physics.SphereCast(
                origin,
                _sphereRadius,
                Vector3.down,
                out _,
                _castDistance,
                _groundMask,
                QueryTriggerInteraction.Ignore
            );

            _grounded = ccGrounded || castGrounded;

            if (_grounded)
                _lastGroundedTime = Time.time;
            Debug.Log($"Grounded={_grounded} cc={_cc.isGrounded}");

        }

        private void OnJumpPressed()
        {
            _lastJumpPressedTime = Time.time;
        }

        private void HandleJump()
        {
            bool buffered = (Time.time - _lastJumpPressedTime) <= _jumpBuffer;
            bool coyote = (Time.time - _lastGroundedTime) <= _coyoteTime;

            if (buffered && (_grounded || coyote) && !_inDodge && !_inSlide)
            {
                _lastJumpPressedTime = -999f;
                DoJump();
            }
        }

        private void DoJump()
        {
            RecalcJump();

            if (_ySpeed < 0f) _ySpeed = 0f;
            _ySpeed = _startJumpVelocity;

            Jumped?.Invoke();
        }

        private void ApplyMove()
        {
            Vector3 wishDir = GetMoveDirection();
            float maxSpeed = _grounded
                ? (_input != null && _input.SprintHeld ? _sprintSpeed : _moveSpeed)
                : _airSpeed;

            if (_input == null || _input.Move == Vector2.zero) maxSpeed = 0f;

            Vector3 target = wishDir * maxSpeed;

            if (_grounded)
            {
                float accel = (target.sqrMagnitude > _hVel.sqrMagnitude) ? _groundAccel : _groundDecel;
                _hVel = Vector3.MoveTowards(_hVel, target, accel * Time.deltaTime);
            }
            else
            {
                Vector3 airTarget = target * _airControl;
                _hVel = Vector3.MoveTowards(_hVel, airTarget, _airAccel * Time.deltaTime);
            }
        }

        private void ApplyVertical()
        {
            if (_grounded && _ySpeed < 0f)
                _ySpeed = _groundSnapY;

            // jump cut
            if (_input != null && !_input.JumpHeld && _ySpeed > 0f)
                _ySpeed *= _jumpCutMultiplier;

            float gravity = _gravityForce;
            if (_ySpeed <= 0f) gravity *= _fallMultiplier;

            _ySpeed -= gravity * Time.deltaTime;
        }

        private void RotateToCamera()
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
            Vector3 horizontal = (_inDodge || _inSlide) ? _impulseVel : _hVel;
            Vector3 vel = new Vector3(horizontal.x, _ySpeed, horizontal.z);
            _cc.Move(vel * Time.deltaTime);
        }

        private void OnDodgePressed()
        {
            if (Time.time < _nextDodgeAllowedTime) return;
            if (_inDodge || _inSlide) return;

            Vector3 dir = GetActionDirection();
            if (dir.sqrMagnitude < 0.0001f) dir = GetFacingDirection();
            StartCoroutine(DodgeRoutine(dir));
        }

        private void OnSlidePressed()
        {
            if (Time.time < _nextDodgeAllowedTime) return;
            if (_inDodge || _inSlide) return;

            Vector3 dir = GetActionDirection();
            if (dir.sqrMagnitude < 0.0001f) dir = GetFacingDirection();
            StartCoroutine(SlideRoutine(dir));
        }

        private IEnumerator DodgeRoutine(Vector3 dir)
        {
            _inDodge = true;
            _nextDodgeAllowedTime = Time.time + _dodgeDuration + _dodgeCooldown;

            Dodged?.Invoke();

            float t = 0f;
            while (t < _dodgeDuration)
            {
                float k = 1f - (t / _dodgeDuration);
                float speed = Mathf.Lerp(_dodgeSpeed * 0.6f, _dodgeSpeed, k);

                _impulseVel = dir * speed;

                t += Time.deltaTime;
                yield return null;
            }

            _impulseVel = Vector3.zero;
            _inDodge = false;
        }

        private IEnumerator SlideRoutine(Vector3 dir)
        {
            _inSlide = true;
            _nextDodgeAllowedTime = Time.time + _slideDuration + _dodgeCooldown;

            Slid?.Invoke();

            float t = 0f;
            float speed = _slideSpeed;

            while (t < _slideDuration)
            {
                speed = Mathf.Max(0f, speed - _slideFriction * Time.deltaTime);
                _impulseVel = dir * speed;

                t += Time.deltaTime;
                yield return null;
            }

            _impulseVel = Vector3.zero;
            _inSlide = false;
        }

        private Vector3 GetMoveDirection()
        {
            if (_cam == null || _input == null) return Vector3.zero;

            Vector2 input = _input.Move;
            if (input == Vector2.zero) return Vector3.zero;

            Vector3 forward = _cam.forward; forward.y = 0f; forward.Normalize();
            Vector3 right = _cam.right; right.y = 0f; right.Normalize();

            Vector3 wish = input.y * forward + input.x * right;
            return wish.sqrMagnitude > 0f ? wish.normalized : Vector3.zero;
        }

        private Vector3 GetFacingDirection()
        {
            if (_cam == null) return transform.forward;
            Vector3 f = _cam.forward; f.y = 0f;
            return f.sqrMagnitude > 0f ? f.normalized : transform.forward;
        }

        private Vector3 GetActionDirection()
        {
            Vector3 d = GetMoveDirection();
            return d.sqrMagnitude > 0.0001f ? d : GetFacingDirection();
        }

        private void RecalcJump()
        {
            float heightTime = _jumpTime / 2f;
            _gravityForce = 2f * _jumpHeight / (heightTime * heightTime);
            _startJumpVelocity = 2f * _jumpHeight / heightTime;
        }

        private void ComputeLocalMoveForAnimator()
        {
            // Так как мы крутимся за камерой, local X/Y идеально подходит для strafe blend.
            // Берём ИМЕННО input, чтобы анимации сразу реагировали, даже если скорость ещё догоняет.
            if (_input == null)
            {
                _localMove = Vector2.zero;
                return;
            }

            _localMove = _input.Move; // x=left/right, y=forward/back
        }
#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (_cc == null) return;

            // Начальная точка SphereCast
            Vector3 origin = transform.position + _cc.center + Vector3.up * 0.1f;

            // Конечная точка (куда кастим)
            Vector3 end = origin + Vector3.down * _castDistance;

            // Цвет в зависимости от grounded
            Gizmos.color = Application.isPlaying && _grounded ? Color.green : Color.red;

            // Верхняя сфера
            Gizmos.DrawWireSphere(origin, _sphereRadius);

            // Нижняя сфера (конец каста)
            Gizmos.DrawWireSphere(end, _sphereRadius);

            // Линия между ними (визуализация кастинга)
            Gizmos.DrawLine(origin, end);
        }
#endif

    }
}
