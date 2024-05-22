using Scripts.Player.camera;
using System;
using System.Collections;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;
using Zenject;

namespace Scripts.Player
{
    public class MovementService : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;
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

        private LayerMask _hitboxLayer;
        private InputService _inputService;
        private ThirdPersonCameraService _thirdPersonCam;

        private bool isJump = true;
        private bool IsDodge = true;

        public event Action OnJump;
        public event Action OnDogde;
        [Inject]
        private void Construct(InputService inputService, ThirdPersonCameraService thirdPersonCam)
        {
            _inputService = inputService;
            _thirdPersonCam = thirdPersonCam;

            _inputService.OnJumpKeyPressed += Jump;
            _inputService.OnDogdeKeyPressed += Dodge;
            _inputService.OnSprintKeyPressed += Sprint;
            _inputService.OnSprintKeyRealesed += Run;

            _currentSpeed = _runSpeed;

            _hitboxLayer = 1 << 7;
            _hitboxLayer = ~_hitboxLayer;
        }

        private void OnDestroy()
        {
            _inputService.OnJumpKeyPressed -= Jump;
            _inputService.OnDogdeKeyPressed -= Dodge;
            _inputService.OnSprintKeyPressed -= Sprint;
            _inputService.OnSprintKeyRealesed -= Run;
        }

        private void FixedUpdate()
        {
            if (IsGrounded())
            {
                Moving(10f);              
                _rigidbody.drag = _groundDrag;
            }
            else
            {
                Moving(0.7f);
                _rigidbody.drag = 0;
            }
        }

        private void Moving(float acceleration)
        {
            Vector3 movingDirection = new Vector3(_thirdPersonCam.ForwardDirection.x, 0, _thirdPersonCam.ForwardDirection.z).normalized;
            _rigidbody.AddForce(movingDirection * _currentSpeed * acceleration, ForceMode.Force);

            if (!IsGrounded())
            {
                SpeedControl();
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

        private void Dodge()
        {
            if (IsGrounded() && IsDodge)
            {
                Vector3 movingDirection = new Vector3(_thirdPersonCam.ForwardDirection.x, 0, _thirdPersonCam.ForwardDirection.z).normalized;
                _rigidbody.AddForce(movingDirection * _dodgeForce, ForceMode.VelocityChange);
                StartCoroutine(DodgeCooldown(_dodgeCooldown));
                IsDodge = false;
                OnDogde?.Invoke();
            }
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

        private void SpeedControl()
        {
            Vector3 playerSpeed = new Vector3(_rigidbody.velocity.x, 0f, _rigidbody.velocity.z);
            if (playerSpeed.magnitude > _currentSpeed / 2)
            {
                Vector3 limitedSpeed = playerSpeed.normalized * _currentSpeed / 2;
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

        private IEnumerator JumpCooldown(float secounds)
        {
            yield return new WaitForSeconds(secounds);
            isJump = true;
        }
        private IEnumerator DodgeCooldown(float secounds)
        {
            yield return new WaitForSeconds(secounds);
            IsDodge = true;
        }
    }
}