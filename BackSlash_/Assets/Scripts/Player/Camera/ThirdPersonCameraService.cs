using UnityEngine;
using Zenject;

namespace Scripts.Player.camera
{
    class ThirdPersonCameraService : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform _camera;
        [SerializeField] private Transform _orientation;
        [SerializeField] private float _rotationTime;

        private InputService _inputService;
        private Vector3 _forwardDirection;

        public Vector3 ForwardDirection => _forwardDirection;

        [Inject]
        private void Construct(InputService inputService)
        {
            _inputService = inputService;
            //DisableCursor();
        }

        private void FixedUpdate()
        {
            RotatePlayer();
            RotateCameraAroundPlayer();
        }

        private void RotatePlayer()
        {
            var direction = _inputService.MoveDirection;
            Vector3 ViewDir = gameObject.transform.position - new Vector3(_camera.transform.position.x, gameObject.transform.position.y, _camera.transform.position.z);
            _orientation.forward = ViewDir.normalized;

            Vector3 inputDir = _orientation.forward * direction.z + _orientation.right * direction.x;
            if (inputDir != Vector3.zero)
            {
                gameObject.transform.forward = Vector3.Slerp(inputDir, gameObject.transform.forward, _rotationTime * Time.fixedDeltaTime);
            }
        }

        public void RotateCameraAroundPlayer()
        {
            var direction = _inputService.MoveDirection;
            Vector3 forward = _camera.transform.forward;
            Vector3 right = _camera.transform.right;

            Vector3 forwardRealtiveVerticalInput = direction.z * forward;
            Vector3 rightRealtiveVerticalInput = direction.x * right;

            _forwardDirection = forwardRealtiveVerticalInput + rightRealtiveVerticalInput;
        }

        private void DisableCursor()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}