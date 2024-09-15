using Scripts.Player.camera;
using System;
using System.Collections;
using UnityEngine;
using Zenject;

namespace Scripts.Player
{
    public class MovementController : MonoBehaviour
    {
        [SerializeField] private CharacterController _characterController;
        [Header("Monitoring")]
        [SerializeField] private float _currentSpeed;
        [Header("Movement settings")]
        [SerializeField] private float _runSpeed;
        [SerializeField] private float _sprintSpeed;
        [SerializeField] private float _jumpForce;
        [SerializeField] private float _jumpCooldown;
        [SerializeField] private float _dodgeForce;
        [SerializeField] private float _dodgeCooldown;
        [Header("SlopeAngle")]
        [SerializeField] private float _maxSlopeAngle;
        [Header("IsGround settings")]
        [SerializeField] private float _maxCastDistance;
        [SerializeField] private float _sphereCastRadius;

        private bool _isJump;
        private bool _isDodge;
        private bool _isAttackGoing;
        private float _ySpeed;
        private float _airTime;

        private Vector3 _moveDirection;
        private Vector3 _forwardDirection;
        private RaycastHit _slopeHit;
        private LayerMask _hitboxLayer;

        private InputController _inputController;
        private ComboSystem _comboSystem;
        private Transform _camera;

        public event Action<bool> InAir;
        public event Action<bool> PlaySteps;
        public event Action OnLanded;
        public event Action OnDodge;
        public event Action OnJump;

        //debug gizmo parameter delete later
        private float currenthitdisance;

        [Inject]
        private void Construct(InputController inputController, ComboSystem comboSystem)
        {
            _inputController = inputController;
            _inputController.OnJumpKeyPressed += Jump;
            _inputController.OnDodgeKeyPressed += Dodge;
            _inputController.OnSprintKeyPressed += Sprint;
            _inputController.OnSprintKeyRealesed += Run;

            _currentSpeed = _runSpeed;
            _hitboxLayer = 1 << 7;
            _hitboxLayer = ~_hitboxLayer;

            _comboSystem = comboSystem;
            _comboSystem.IsAttacking += IsAttacking;
        }

        private void Awake()
        {
            _camera = Camera.main.transform;
        }

        private void OnDestroy()
        {
            _inputController.OnJumpKeyPressed -= Jump;
            _inputController.OnDodgeKeyPressed -= Dodge;
            _inputController.OnSprintKeyPressed -= Sprint;
            _inputController.OnSprintKeyRealesed -= Run;
            _comboSystem.IsAttacking -= IsAttacking;
        }

        private void Update()
        {
            CalculateForwardDirection();
            CheckLand();
            CheckAirTime();
            MovePlayer();
            InvokeSteps();            
        }

        public void CalculateForwardDirection()
        {
            var direction = _inputController.MoveDirection;

            Vector3 forwardRealtiveVerticalInput = direction.z * _camera.forward;
            Vector3 rightRealtiveVerticalInput = direction.x * _camera.right;

            _forwardDirection = forwardRealtiveVerticalInput + rightRealtiveVerticalInput;
        }

        private void MovePlayer()
        {
            _ySpeed += Physics.gravity.y * Time.deltaTime;

            if (_isAttackGoing)
            {
                _moveDirection = Vector3.zero;
            }
            else 
            {
                _moveDirection = new Vector3(_forwardDirection.x, 0, _forwardDirection.z).normalized;
                if (IsGrounded())
                {
                    _ySpeed = -0.5f;

                    if (_isJump)
                    {
                        _ySpeed = _jumpForce;
                    }

                    if (OnSlope() && !_isJump)
                    {
                        _ySpeed = -10f;
                    }
                }
                _moveDirection = _moveDirection * _currentSpeed;
            }
            _moveDirection.y = _ySpeed;
            _characterController.Move(_moveDirection * Time.deltaTime);
        }

        private bool OnSlope()
        {
            if (Physics.Raycast(transform.position, Vector3.down, out _slopeHit, _characterController.height * 0.5f + 1.5f))
            {
                float angle = Vector3.Angle(Vector3.up, _slopeHit.normal);
                if (angle > _maxSlopeAngle)
                {
                    return true;
                }
            }
            return false;
        }

        private void Jump()
        {
            if (!_isJump)
            {
                _isJump = true;
                StartCoroutine(JumpCooldown(_jumpCooldown));
                OnJump?.Invoke();
            }
        }

        private void Dodge()
        {
            if (IsGrounded() && !_isDodge)
            {
                _currentSpeed = _currentSpeed + _dodgeForce;
                StartCoroutine(DodgeCooldown(_dodgeCooldown));
                _isDodge = true;
                OnDodge?.Invoke();
            }
        }

        private IEnumerator JumpCooldown(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            _isJump = false;
        }

        private IEnumerator DodgeCooldown(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            _currentSpeed = _runSpeed;
            _isDodge = false;
        }

        private void Sprint()
        {
            _currentSpeed = _sprintSpeed;
        }

        private void Run()
        {
            _currentSpeed = _runSpeed;
        }

        private void CheckAirTime()
        {
            if (IsGrounded())
            {
                _airTime = 0f;
            }
            else
            {
                _airTime += Time.deltaTime;
                if (_airTime > 0f)
                {
                    InAir?.Invoke(true);
                }
            }
        }

        private void CheckLand()
        {
            if (_airTime > 0f)
            {
                if (IsGrounded())
                {
                    OnLanded?.Invoke();
                    InAir?.Invoke(false);
                }
            }
        }

        private void InvokeSteps()
        {
            if ((_forwardDirection.x != 0 || _forwardDirection.z != 0) && IsGrounded() && !_isDodge)
            {
                PlaySteps?.Invoke(true);
            }
            else
            {
                PlaySteps?.Invoke(false);
            }
        }

        private bool IsGrounded()
        {
            RaycastHit hitInfo;
            Vector3 directionDown = transform.TransformDirection(Vector3.down);

            if (Physics.SphereCast(gameObject.transform.position + _characterController.center + (Vector3.up * 0.1f),
                _sphereCastRadius,
                Vector3.down, out hitInfo,
                _maxCastDistance,
                _hitboxLayer,
                QueryTriggerInteraction.UseGlobal))
            {
                currenthitdisance = hitInfo.distance;
                return true;
            }
            else
            {
                currenthitdisance = _maxCastDistance;
                return false;
            }
        }

        private void IsAttacking(bool isAttacking)
        {
            _isAttackGoing = isAttacking;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Debug.DrawLine(gameObject.transform.position + _characterController.center + (Vector3.up * 0.1f), gameObject.transform.position + (_characterController.center + (Vector3.up * 0.1f)) + Vector3.down * currenthitdisance,Color.yellow);
            Gizmos.DrawWireSphere(gameObject.transform.position + (_characterController.center + (Vector3.up * 0.1f)) + Vector3.down * currenthitdisance, _sphereCastRadius);
        }
    }
}