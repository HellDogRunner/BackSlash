using Scripts.Player.Camera;
using UnityEngine;
using Zenject;

namespace Scripts.Player
{
    public class MovementService : MonoBehaviour
    {
        [SerializeField] private Rigidbody Rigidbody;
        [SerializeField] private PlayerCollision PlayerCollision;
        [Header("Movement")]
        [SerializeField] private float WalkSpeed;
        [SerializeField] private float MoveSpeed;
        [SerializeField] private float SprintSpeed;
        [SerializeField] private float GroundDrag;
        [SerializeField] private float JumpForce;
        [Header("Ground Check")]

        private float _sprintSpeed = 1;

        private InputService _inputService;
        private ThirdPersonCam _thirdPersonCam;

        private Vector3 _moveDirection;

        [Inject]
        private void Construct(InputService inputService, ThirdPersonCam thirdPersonCam)
        {
            _inputService = inputService;
            _thirdPersonCam = thirdPersonCam;

            _inputService.OnDirectionChanged += Direction;
            _inputService.OnSprintKeyPressed += Sprint;
        }

        private void OnDestroy()
        {
            _inputService.OnDirectionChanged -= Direction;
            _inputService.OnSprintKeyPressed -= Sprint;
        }

        private void FixedUpdate()
        {
            if (PlayerCollision.IsGrounded)
            {
                if (_moveDirection.y > 0)
                {
                    Jump();
                }
                else if (_inputService.StateContainer.State != PlayerState.EPlayerState.Jumping)
                {
                    Vector3 movingDirection = new Vector3(_thirdPersonCam.ForwardDirection.x, 0, _thirdPersonCam.ForwardDirection.z).normalized;
                    Rigidbody.AddForce(movingDirection * MoveSpeed * _sprintSpeed * 10f, ForceMode.Force);
                }
            }
        }

        private void Sprint()
        {
            if (_inputService.StateContainer.State == PlayerState.EPlayerState.Sprint)
            {
                _sprintSpeed = SprintSpeed;
            }
            else _sprintSpeed = 1;
        }

        private void Direction(Vector3 direction)
        {
            _moveDirection = direction;
        }

        private void Jump()
        {
            Rigidbody.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
        }
    }
}