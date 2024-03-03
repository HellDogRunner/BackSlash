using UnityEngine;
using Zenject;

namespace Scripts.Player.Camera
{ 
    class ThirdPersonCam : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform Camera;
        [SerializeField] private Transform Orientation;
        [SerializeField] private Transform CombatLookAt;

        [SerializeField] private Transform Player;
        [SerializeField] private Transform PlayerObj;
        [SerializeField] private Rigidbody PlayerRigibody;

        [SerializeField] private float RotationTime;

        [SerializeField] private GameObject PlayerServices;
        [SerializeField] private ECameraStyle CurrentStyle;

        private InputService _inputService;

        public Vector3 ForwardDirection;

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

        private void Update()
        {
            RotatePlayer();
            RotateCameraAroundPlayer();
        }

        private void RotatePlayer()
        {
            var direction = _inputService.MoveDirection;
            Vector3 ViewDir = Player.position - new Vector3(Camera.transform.position.x, Player.position.y, Camera.transform.position.z);
            Orientation.forward = ViewDir.normalized;

            Vector3 inputDir = Orientation.forward * direction.z + Orientation.right * direction.x;
            if (inputDir != Vector3.zero)
            {
                PlayerObj.forward = Vector3.Slerp(PlayerObj.forward, inputDir, Time.deltaTime * RotationTime);
            }
        }

        public void RotateCameraAroundPlayer()
        {
            var direction = _inputService.MoveDirection;
            Vector3 forward = Camera.transform.forward;
            Vector3 right = Camera.transform.right;

            Vector3 forwardRealtiveVerticalInput = direction.z * forward;
            Vector3 rightRealtiveVerticalInput = direction.x * right;

            ForwardDirection = forwardRealtiveVerticalInput + rightRealtiveVerticalInput;
        }

        private void DisableCursor()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}