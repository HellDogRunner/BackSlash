using Scripts.Player.Camera;
using UnityEngine;
using Zenject;

namespace Scripts.Player
{
    public class MovementService : MonoBehaviour
    {
        [SerializeField] private Rigidbody Rigidbody;
        [Header("Movement")]
        [SerializeField] private float WalkSpeed;
        [SerializeField] private float MoveSpeed;
        [SerializeField] private float SprintSpeed;
        [SerializeField] private float GroundDrag;
        [SerializeField] private float JumpForce;
        [Header("Ground Check")]
        [SerializeField] private LayerMask IsGround;

        private float _sprintSpeed = 1;

        private InputService _inputService;
        private ThirdPersonCam _thirdPersonCam;

        [Inject]
        private void Construct(InputService inputService, ThirdPersonCam thirdPersonCam)
        {
            _inputService = inputService;
            _thirdPersonCam = thirdPersonCam;

            _inputService.OnSprintKeyPressed += Sprint;
        }

        private void OnDestroy()
        {
            _inputService.OnSprintKeyPressed -= Sprint;
        }

        public bool IsGrounded()
        {
            bool _isGround = Physics.Raycast(Rigidbody.transform.position, Vector3.down, 0.05f, IsGround);
            if (_isGround)
            {
                Rigidbody.drag = GroundDrag;
            }
            else Rigidbody.drag = 0;

            return _isGround;
        }

        private void FixedUpdate()
        {
            if (_inputService.StateContainer.State == PlayerState.EPlayerState.Jump)
            {
                Jump();
                _inputService.StateContainer.State = PlayerState.EPlayerState.InAir;
            } 
            else if (IsGrounded())
            {
                if (_inputService.StateContainer.State == PlayerState.EPlayerState.InAir)
                {
                    _inputService.StateContainer.State = PlayerState.EPlayerState.Run;
                }

                Vector3 movingDirection = new Vector3(_thirdPersonCam.ForwardDirection.x, 0, _thirdPersonCam.ForwardDirection.z).normalized;
                Rigidbody.AddForce(movingDirection * MoveSpeed * _sprintSpeed * 10f, ForceMode.Force);
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

        private void Jump()
        {
            Rigidbody.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
        }
    }
}