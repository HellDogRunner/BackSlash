using UnityEngine;
using Zenject;

namespace Scripts.Player.Camera
{ 
    class ThirdPersonCam : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform cam;
        [SerializeField] private Transform orientation;

        [SerializeField] private Transform player;
        [SerializeField] private Transform playerModel;

        [SerializeField] private float RotationTime;

        [SerializeField] private ECameraStyle CurrentStyle;

        private InputService _inputService;

        private Vector3 _forwardDirection;
        public Vector3 ForwardDirection => _forwardDirection;

        private enum ECameraStyle
        {
            Basic,
            Combat
        }

        [Inject]
        private void Construct(InputService inputService)
        {
            _inputService = inputService;
        }

        private void Start()
        {
            DisableCursor();
        }

        private void FixedUpdate()
        {
            RotatePlayer();
            RotateCameraAroundPlayer();
        }

        private void RotatePlayer()
        {
            var direction = _inputService.MoveDirection;
            Vector3 ViewDir = player.position - new Vector3(cam.transform.position.x, player.position.y, cam.transform.position.z);
            orientation.forward = ViewDir.normalized;

            Vector3 inputDir = orientation.forward * direction.z + orientation.right * direction.x;
            if (inputDir != Vector3.zero)
            {
                playerModel.forward = Vector3.Slerp(inputDir, playerModel.forward, RotationTime * Time.fixedDeltaTime);
            }
        }

        public void RotateCameraAroundPlayer()
        {
            var direction = _inputService.MoveDirection;
            Vector3 forward = cam.transform.forward;
            Vector3 right = cam.transform.right;

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