using UnityEngine;
using Zenject;

namespace Scripts.Player.camera
{
    class ThirdPersonCameraService : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform Camera;
        [SerializeField] private Transform Orientation;
        [SerializeField] private float RotationTime;

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
            Vector3 ViewDir = gameObject.transform.position - new Vector3(Camera.transform.position.x, gameObject.transform.position.y, Camera.transform.position.z);
            Orientation.forward = ViewDir.normalized;

            Vector3 inputDir = Orientation.forward * direction.z + Orientation.right * direction.x;
            if (inputDir != Vector3.zero)
            {
                gameObject.transform.forward = Vector3.Slerp(inputDir, gameObject.transform.forward, RotationTime * Time.fixedDeltaTime);
            }
        }

        public void RotateCameraAroundPlayer()
        {
            var direction = _inputService.MoveDirection;
            Vector3 forward = Camera.transform.forward;
            Vector3 right = Camera.transform.right;

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