using UnityEngine;
using Zenject;

namespace Scripts.Player.camera
{ 
    class ThirdPersonCam : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform cam;

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
            DisableCursor();
        }

        private void Start()
        {
            DisableCursor();
        }

        private void LateUpdate()
        {
            RotateCameraAroundPlayer();
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