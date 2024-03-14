using Scripts.Player.camera;
using UnityEngine;
using Zenject;

namespace Scripts.Player
{
    public class MovementService : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;
        [Header("Monitoring")]
        [SerializeField] private float currentSpeed;
        [SerializeField] private float turnSpeed;
        [Header("Movement")]
        [SerializeField] private float walkSpeed;
        [SerializeField] private float runSpeed;
        [SerializeField] private float sprintSpeed;
        [SerializeField] private float groundDrag;
        [SerializeField] private float jumpForce;
        [SerializeField] private float dodgeForce;

        private InputService _inputService;
        private ThirdPersonCam _thirdPersonCam;

        private bool isGrounded;

        private Camera mainCamera;

        [Inject]
        private void Construct(InputService inputService, ThirdPersonCam thirdPersonCam)
        {
            _inputService = inputService;
            _thirdPersonCam = thirdPersonCam;
            mainCamera = Camera.main;

            _inputService.OnJumpKeyPressed += Jump;
            _inputService.OnDogdeKeyPressed += Dodge;

            currentSpeed = runSpeed;
        }

        private void OnDestroy()
        {
            _inputService.OnJumpKeyPressed -= Jump;
            _inputService.OnDogdeKeyPressed -= Dodge;
        }

        private void Update()
        {
          
        }

        private void FixedUpdate()
        {
            float yawCamera = mainCamera.transform.rotation.eulerAngles.y;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, yawCamera, 0), turnSpeed * Time.deltaTime);
            if (isGrounded)
            {
                Moving(10f);              
                Sprint();
                _rigidbody.drag = groundDrag;
            }
            else
            {
                Moving(1.5f);
            }
        }

        private void Moving(float acceleration)
        {
            Vector3 movingDirection = new Vector3(_thirdPersonCam.ForwardDirection.x, 0, _thirdPersonCam.ForwardDirection.z).normalized;
            _rigidbody.AddForce(movingDirection * currentSpeed * acceleration, ForceMode.Force);

            if (!isGrounded)
            {
                SpeedControl();
            }
        }

        private void Sprint()
        {
            if (_inputService.PlayerStateContainer.State == PlayerState.EPlayerState.Sprint)
            {
                currentSpeed = sprintSpeed;
            }
            else if (_inputService.PlayerStateContainer.State == PlayerState.EPlayerState.Run)
            {
                currentSpeed = runSpeed;
            }
        }

        private void Dodge()
        {
            if (isGrounded)
            {
                Vector3 movingDirection = new Vector3(_thirdPersonCam.ForwardDirection.x, 0, _thirdPersonCam.ForwardDirection.z).normalized;
                _rigidbody.AddForce(movingDirection * dodgeForce, ForceMode.VelocityChange);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            isGrounded = true;
        }

        private void Jump()
        {
            if (isGrounded)
            {
                if (_inputService.PlayerStateContainer.State == PlayerState.EPlayerState.Jumping)
                {
                    _rigidbody.drag = 0;
                    _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

                    isGrounded = false;
                }
            }
        }

        private void SpeedControl()
        {
            Vector3 playerSpeed = new Vector3(_rigidbody.velocity.x, 0f, _rigidbody.velocity.z);
            if (playerSpeed.magnitude > currentSpeed)
            {
                Vector3 limitedSpeed = playerSpeed.normalized * currentSpeed;
                _rigidbody.velocity = new Vector3(limitedSpeed.x, _rigidbody.velocity.y, limitedSpeed.z);
            }
        }

       
    }
}