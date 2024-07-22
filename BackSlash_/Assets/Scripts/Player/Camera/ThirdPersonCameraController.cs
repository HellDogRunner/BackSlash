using UnityEngine;
using Zenject;

namespace Scripts.Player.camera
{
    class ThirdPersonCameraController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform _baseOrientation;
        [SerializeField] private float _rotationTime;

        private InputController _inputService;
        private TargetLock _targetLock;

        private Vector3 _forwardDirection;
        private Transform _camera;
        public Vector3 ForwardDirection => _forwardDirection;

        [Inject]
        private void Construct(InputController inputService, TargetLock targetLock)
        {
            _inputService = inputService;
            _targetLock = targetLock;
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
                RotatePlayer();
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

        public void CalculateForwardDirection()
        {
            var direction = _inputService.MoveDirection;
            Vector3 forward = _camera.transform.forward;
            Vector3 right = _camera.transform.right;

            Vector3 forwardRealtiveVerticalInput = direction.z * forward;
            Vector3 rightRealtiveVerticalInput = direction.x * right;

            _forwardDirection = forwardRealtiveVerticalInput + rightRealtiveVerticalInput;
        }
    }
}