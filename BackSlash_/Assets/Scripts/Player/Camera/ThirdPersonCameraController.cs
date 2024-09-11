using System;
using System.Collections;
using UnityEngine;
using Zenject;

namespace Scripts.Player.camera
{
    class ThirdPersonCameraController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform _baseOrientation;
        [SerializeField] private float _rotationTime = 30;
        [SerializeField] private float _turnSpeed = 30;
        [SerializeField] private float _delayRotationTime = 5f;

        private float _timeToRotate;

        private InputController _inputService;
        private TargetLock _targetLock;
        private ComboSystem _comboSystem;

        private Vector3 _forwardDirection;
        private Transform _camera;
        private Coroutine _currentDelayRoutine;

        private bool _isAttacking;
        public Vector3 ForwardDirection => _forwardDirection;

        public event Action<bool> IsAttacking;

        [Inject]
        private void Construct(InputController inputService, TargetLock targetLock, ComboSystem comboSystem)
        {
            _inputService = inputService;
            _targetLock = targetLock;
            _comboSystem = comboSystem;
            _comboSystem.IsAttacking += OnAttack;

            _camera = Camera.main.transform;
        }

        private void FixedUpdate()
        {
            _timeToRotate += Time.deltaTime;
            if (_timeToRotate >= _delayRotationTime) 
            {
                _timeToRotate = 0;
                _isAttacking = false;
            }

            CalculateForwardDirection();

            if (_targetLock.CurrentTargetTransform != null)
            {
                RotatePlayerLocked();
            }
            else
            {
                if (_isAttacking)
                {
                    RotatePlayerForward();
                }
                else
                {
                    RotatePlayer();
                }
            }
            IsAttacking?.Invoke(_isAttacking);
        }

        private void RotatePlayer()
        {
            var direction = _inputService.MoveDirection;
            Vector3 ViewDir = gameObject.transform.position - new Vector3(_camera.transform.position.x, gameObject.transform.position.y, _camera.transform.position.z);
            _baseOrientation.forward = ViewDir.normalized;

            Vector3 inputDir = _baseOrientation.forward * direction.z + _baseOrientation.right * direction.x;
            if (inputDir != Vector3.zero)
            {
                gameObject.transform.forward = Vector3.Slerp(inputDir, gameObject.transform.forward, _rotationTime * Time.deltaTime);
            }
        }

        private void RotatePlayerLocked()
        {
            Vector3 rotationDirection = _targetLock.CurrentTargetTransform.position - gameObject.transform.position;
            rotationDirection.Normalize();
            rotationDirection.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(rotationDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationTime * Time.deltaTime);
        }

        private void RotatePlayerForward()
        {
            float cameraYaw = _camera.transform.eulerAngles.y;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, cameraYaw, 0), _turnSpeed * Time.deltaTime);
        }

        private void OnAttack(bool attack) 
        {
            if (attack)
            {
                _isAttacking = true;
                _timeToRotate = 0;
            }
        }

        public void CalculateForwardDirection()
        {
            var direction = _inputService.MoveDirection;
            Vector3 forward = _camera.transform.forward;
            Vector3 right = _camera.transform.right;

            Vector3 forwardRealtiveVerticalInput = direction.z * forward;
            Vector3 rightRealtiveVerticalInput = direction.x * right;

            _forwardDirection = forwardRealtiveVerticalInput + rightRealtiveVerticalInput;
        }

        private IEnumerator DelayRotation(float delaySecounds)
        {
            yield return new WaitForSeconds(delaySecounds);

            _isAttacking = false;
        }

        private void OnDestroy()
        {
            _comboSystem.IsAttacking -= OnAttack;
        }
    }
}