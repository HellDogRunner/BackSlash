using UnityEngine;
using Zenject;
using Scripts.Player.Camera;
using static Scripts.Player.PlayerState;

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

        public bool Grounded;

        private float _sprintSpeed = 1;

        private Vector3 _moveDirection;

        private InputService _inputService;
        private ThirdPersonCam _thirdPersonCam;
        private PlayerState _playerState;

        [Inject]
        private void Construct(InputService inputService, ThirdPersonCam thirdPersonCam, PlayerState playerState)
        {
            _inputService = inputService;
            _playerState = playerState;

            _inputService.OnDirectionChanged += Direction;
            _inputService.OnSprintKeyPressed += Sprint;

            _thirdPersonCam = thirdPersonCam;
        }

        private void OnDestroy()
        {
            _inputService.OnDirectionChanged -= Direction;
            _inputService.OnSprintKeyPressed += Sprint;
        }

        private void Update()
        {
            Grounded = Physics.Raycast(Rigidbody.transform.position, Vector3.down, 0.05f, IsGround);

            if (Grounded)
            {
                Rigidbody.drag = GroundDrag;
            }
            else
                Rigidbody.drag = 0;
        }

        private void FixedUpdate()
        {
            if (Grounded)
            {
                if (_moveDirection.y > 0)
                {
                    Jump();
                }
                Vector3 movingDirection = new Vector3(_thirdPersonCam.ForwardDirection.x, 0, _thirdPersonCam.ForwardDirection.z).normalized;
                Rigidbody.AddForce(movingDirection * MoveSpeed* _sprintSpeed * 10f, ForceMode.Force);
            }    
        }

        private void Direction(Vector3 direction)
        {  
            _moveDirection = direction;
        }

        private void Sprint()
        {
            if (_playerState._state == PlayerState.EPlayerState.Sprint)
            {
                _sprintSpeed = SprintSpeed;
            }
            else _sprintSpeed = 1;
        }

        private void Jump()
        {
            Vector3 Jumpdir = new Vector3(0, 1, 0);
            Rigidbody.AddForce(Jumpdir * JumpForce, ForceMode.Impulse);
        }  
    }
}