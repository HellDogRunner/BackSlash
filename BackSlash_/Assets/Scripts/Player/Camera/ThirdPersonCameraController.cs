using Scripts.Weapon;
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

        private InputController _inputService;
        private TargetLock _targetLock;
        private ComboSystem _comboSystem;

        private Vector3 _forwardDirection;
        private Transform _camera;
        private Coroutine _currentAttackRoutine;
        public Vector3 ForwardDirection => _forwardDirection;


        private bool _isAttaking;
        private bool _isBlocking;

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
            CalculateForwardDirection();
            if (_targetLock.CurrentTargetTransform != null)
            {
                RotatePlayerLocked();
            }
            else
            {
                if (_isAttaking || _isBlocking)
                {
                    RotatePlayerForward();
                }
                else
                {
                    RotatePlayer();
                }
            }
        }

        private void RotatePlayer()
        {
            var direction = _inputService.MoveDirection;
            Vector3 ViewDir = gameObject.transform.position - new Vector3(_camera.transform.position.x, gameObject.transform.position.y, _camera.transform.position.z);
            _baseOrientation.forward = ViewDir.normalized;

            Vector3 inputDir = _baseOrientation.forward * direction.z + _baseOrientation.right * direction.x;
            if (inputDir != Vector3.zero)
            {
                gameObject.transform.forward = Vector3.Slerp(inputDir, gameObject.transform.forward, _rotationTime * Time.fixedDeltaTime);
            }
        }

        private void RotatePlayerLocked()
        {
            Vector3 rotationDirection = _targetLock.CurrentTargetTransform.position - gameObject.transform.position;
            rotationDirection.Normalize();
            rotationDirection.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(rotationDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationTime * Time.fixedDeltaTime);
        }

        private void RotatePlayerForward()
        {
            float cameraYaw = _camera.transform.eulerAngles.y;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, cameraYaw, 0), _turnSpeed * Time.fixedDeltaTime);
        }

        private void OnAttack(bool attack) 
        {
            _isAttaking = attack;

        }

        private void OnBlock(bool block)
        {
            _isBlocking = block;
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

        private void OnDestroy()
        {
            _comboSystem.IsAttacking -= OnAttack;
        }
    }
}