using Scripts.Animations;
using Scripts.Player.camera;
using System;
using System.Collections;
using UnityEngine;
using Zenject;

namespace Scripts.Player
{
    public class MovementController : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private CapsuleCollider _playerCollider;
        [Header("IsGround settings")]
        [SerializeField] private float _maxDistanse;
        [SerializeField] private float _sphereRadius;
        [Header("Monitoring")]
        [SerializeField] private float _currentSpeed;
        [Header("Movement")]
        [SerializeField] private float _runSpeed;
        [SerializeField] private float _sprintSpeed;
        [SerializeField] private float _groundDrag;
        [SerializeField] private float _jumpForce;
        [SerializeField] private float _dodgeForce;
        [SerializeField] private float _dodgeCooldown;
        [SerializeField] private float _jumpCooldown;
        [SerializeField] private float _airMultiplier = 0.7f;
        [Header("SlopeAngle")]
        [SerializeField] private float _maxSlopeAngle;

        private Vector3 _moveDirection;

        private RaycastHit _slopeHit;
        private LayerMask _hitboxLayer;
        private InputController _inputController;
        private ThirdPersonCameraController _thirdPersonCam;
        private PlayerAnimationController _playerAnimationController;

        private bool isJump = true;
        private bool isDodge = true;
        private bool _wasGrounded = true;

        public event Action<bool> InAir;
        public event Action<bool> IsMoving;
        public event Action OnLanded;
        public event Action OnDodge;
        public event Action OnJump;

        [Inject]
        private void Construct(InputController inputController, ThirdPersonCameraController thirdPersonCam)
        {
            _thirdPersonCam = thirdPersonCam;

            _inputController = inputController;
            _inputController.OnJumpKeyPressed += Jump;
            _inputController.OnDodgeKeyPressed += Dodge;
            _inputController.OnSprintKeyPressed += Sprint;
            _inputController.OnSprintKeyRealesed += Run;

            _currentSpeed = _runSpeed;

            _hitboxLayer = 1 << 7;
            _hitboxLayer = ~_hitboxLayer;
        }

        private void OnDestroy()
        {
            _inputController.OnJumpKeyPressed -= Jump;
            _inputController.OnDodgeKeyPressed -= Dodge;
            _inputController.OnSprintKeyPressed -= Sprint;
            _inputController.OnSprintKeyRealesed -= Run;
        }

        private void Jump()
        {
            if (IsGrounded() && isJump)
            {
                _rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
                StartCoroutine(JumpCooldown(_jumpCooldown));
                isJump = false;
                OnJump?.Invoke();
            }
        }

        private void Dodge()
        {
            if (IsGrounded() && (_moveDirection != Vector3.zero) && isDodge)
            {
                _rigidbody.AddForce(_moveDirection * _dodgeForce, ForceMode.VelocityChange);
                StartCoroutine(DodgeCooldown(_dodgeCooldown));
                isDodge = false;
                OnDodge?.Invoke();
            }
        }

        private void Sprint()
        {
            _currentSpeed = _sprintSpeed;
        }

        private void Run()
        {
            _currentSpeed = _runSpeed;
        }

        private void FixedUpdate()
        {
            MovePlayer();
            SpeedControl();
            IsGroundedAndMoving();
            IsPlayerLanded();
            _wasGrounded = IsGrounded();
        }

        private void MovePlayer()
        {
            if (IsGrounded() && !OnSlope())
            {
                InAir?.Invoke(false);
                _rigidbody.AddForce(MoveDiretion() * _currentSpeed * 10f, ForceMode.Force);
                _rigidbody.drag = _groundDrag;
            }
            else if (IsGrounded() && OnSlope())
            {
                InAir?.Invoke(false);
                _rigidbody.AddForce(SlopeMoveDirection() * _currentSpeed * 10f, ForceMode.Force);
                _rigidbody.velocity -= _slopeHit.normal * 0.5f;
            }
            else if (!IsGrounded())
            {
                InAir?.Invoke(true);
                _rigidbody.AddForce(MoveDiretion() * _currentSpeed * 10f * _airMultiplier, ForceMode.Force);
                _rigidbody.drag = 0;
            }
        }

        private void SpeedControl()
        {
            Vector3 playerSpeed = new Vector3(_rigidbody.velocity.x, 0f, _rigidbody.velocity.z);
            if (playerSpeed.magnitude > _currentSpeed)
            {
                Vector3 limitedSpeed = playerSpeed.normalized * _currentSpeed;
                _rigidbody.velocity = new Vector3(limitedSpeed.x, _rigidbody.velocity.y, limitedSpeed.z);
            }
        }

        private bool IsGrounded()
        {
            RaycastHit hitInfo;
            Vector3 directionDown = transform.TransformDirection(Vector3.down);

            if (Physics.SphereCast(gameObject.transform.position + Vector3.up, _sphereRadius, directionDown, out hitInfo, _maxDistanse, _hitboxLayer, QueryTriggerInteraction.Ignore))
            {
                return true;
            }
            return false;
        }

        private bool IsPlayerLanded() 
        {
            if (IsGrounded() && !_wasGrounded)
            {
                OnLanded?.Invoke();
                return true;
            }
            return false;
        }

        private bool OnSlope()
        {
            if (!IsGrounded()) return false;

            if (Physics.Raycast(transform.position, Vector3.down, out _slopeHit, _playerCollider.height * 0.5f + 1.5f))
            {
                float angle = Vector3.Angle(Vector3.up, _slopeHit.normal);            
                if (angle < _maxSlopeAngle)
                {
                    return true;
                }
            }
            return false;
        }

        private void IsGroundedAndMoving()
        {
            if (IsGrounded() && MoveDiretion() != Vector3.zero)
            {
                IsMoving?.Invoke(true);
            }
            else
            {
                IsMoving?.Invoke(false);
            }
        }

        private Vector3 MoveDiretion()
        {
            _moveDirection = new Vector3(_thirdPersonCam.ForwardDirection.x, 0, _thirdPersonCam.ForwardDirection.z).normalized;
            if (_moveDirection != Vector3.zero)
            {
                return _moveDirection;
            }
            return Vector3.zero;
        }

        private Vector3 SlopeMoveDirection()
        {
            return Vector3.ProjectOnPlane(MoveDiretion(), _slopeHit.normal).normalized;
        }

        private IEnumerator JumpCooldown(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            isJump = true;
        }

        private IEnumerator DodgeCooldown(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            isDodge = true;
        }
    }
}