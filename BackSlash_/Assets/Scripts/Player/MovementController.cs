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

        private bool _isJump;
        private bool _isDodge;
        private float _ySpeed;
        private float _airTime;

        private Vector3 _moveDirection;
        private RaycastHit _slopeHit;

        private InputController _inputController;
        private ThirdPersonCameraController _thirdPersonCam;

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
        }

        private void OnDestroy()
        {
            _inputController.OnJumpKeyPressed -= Jump;
            _inputController.OnDodgeKeyPressed -= Dodge;
            _inputController.OnSprintKeyPressed -= Sprint;
            _inputController.OnSprintKeyRealesed -= Run;
        }

        private void Update()
        {
            CheckLand();
            CheckAirTime();
            MovePlayer();
            IsMovingOnGround();
        }

        private void MovePlayer()
        {
            _moveDirection = new Vector3(_thirdPersonCam.ForwardDirection.x, 0, _thirdPersonCam.ForwardDirection.z).normalized;

            _ySpeed += Physics.gravity.y * Time.deltaTime;

            if (_characterController.isGrounded)
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
            if (_characterController.isGrounded && !_isDodge)
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
            if (_characterController.isGrounded)
            {
                _airTime = 0f;
            }
            else
            {
                _airTime += Time.deltaTime;
                if (_airTime > 0.2f)
                {
                    InAir?.Invoke(true);
                }
            }
        }

        private void CheckLand()
        {
            if (_airTime > 0.2f)
            {
                if (_characterController.isGrounded)
                {
                    OnLanded?.Invoke();
                    InAir?.Invoke(false);
                }
            }
        }

        private void IsMovingOnGround()
        {
            if ((_moveDirection.x != 0 || _moveDirection.z != 0) && _characterController.isGrounded && !_isDodge)
            {
                IsMoving?.Invoke(true);
            }
            else
            {
                IsMoving?.Invoke(false);
            }
        }
    }
}