using Scripts.Player.Camera;
using UnityEngine;
using Zenject;

namespace Scripts.Player
{
    public class MovementService : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;
        [Header("Monitoring")]
        [SerializeField] private float currentSpeed;
        [Header("Movement")]
        [SerializeField] private float walkSpeed;
        [SerializeField] private float runSpeed;
        [SerializeField] private float sprintSpeed;
        [SerializeField] private float groundDrag;
        [SerializeField] private float jumpForce;
        [SerializeField] private float fallForce;
        

        private InputService _inputService;
        private ThirdPersonCam _thirdPersonCam;

        public bool IsGrounded;

        private Vector3 _moveDirection;

        [Inject]
        private void Construct(InputService inputService, ThirdPersonCam thirdPersonCam)
        {
            _inputService = inputService;
            _thirdPersonCam = thirdPersonCam;

            _inputService.OnDirectionChanged += Direction;
            _inputService.OnSprintKeyPressed += Sprint;
            _inputService.OnPlayerWalking += Walking;

            currentSpeed = runSpeed;
        }

        private void OnDestroy()
        {
            _inputService.OnDirectionChanged -= Direction;
            _inputService.OnSprintKeyPressed -= Sprint;
            _inputService.OnPlayerWalking -= Walking;
        }

        private void OnTriggerEnter(Collider other)
        {
            IsGrounded = true;
        }

        private void FixedUpdate()
        {
            if (IsGrounded)
            {
                if (_moveDirection.y > 0)
                {
                    _rigidbody.drag = 0;
                    IsGrounded = false;
                    JumpMoving();
                }
                else _rigidbody.drag = groundDrag;

                Moving(10f);
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
            SpeedControl();
        }

        private void Sprint()
        {
            if (_inputService.StateContainer.State == PlayerState.EPlayerState.Sprint)
            {
                currentSpeed = sprintSpeed;
            }
            else if (_inputService.StateContainer.State == PlayerState.EPlayerState.Run)
            {
                currentSpeed = runSpeed;
            }
        }

        private void Direction(Vector3 direction)
        {
            _moveDirection = direction;
        }

        private void JumpMoving()
        {
            _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        private void Walking()
        {

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