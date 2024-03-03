using UnityEngine;
using Zenject;
using Scripts.Player.Camera;

namespace Scripts.Player
{
    public class MovementService : MonoBehaviour
    {
        [SerializeField] private Rigidbody Rigidbody;
        [Header("Movement")]
        [SerializeField] private float MoveSpeed;
        [SerializeField] private float GroundDrag;
        [SerializeField] private float JumpForce;
        [Header("Ground Check")]
        [SerializeField] private float PlayerHeight;
        [SerializeField] private LayerMask IsGround;

        private bool _grounded;
        private Vector3 _moveDirection;

        private InputService _inputService;
        private ThirdPersonCam _thirdPersonCam;

        [Inject]
        private void Construct(InputService inputService, ThirdPersonCam thirdPersonCam)
        {
            _inputService = inputService;
            _inputService.OnDirectionChanged += Direction;

            _thirdPersonCam = thirdPersonCam;
        }
        private void OnDestroy()
        {
            _inputService.OnDirectionChanged -= Direction;
        }

        private void Update()
        {
            _grounded = Physics.Raycast(Rigidbody.transform.position, Vector3.down, PlayerHeight * 0.5f + 0.05f, IsGround);

            if (_grounded)
            {
                Rigidbody.drag = GroundDrag;
            }
            else
                Rigidbody.drag = 0;
        }

        private void FixedUpdate()
        {
            if (_grounded)
            {
                if (_moveDirection.y > 0)
                {
                    Jump();
                }
                Vector3 movingDirection = new Vector3(_thirdPersonCam.ForwardDirection.x * MoveSpeed * 10f, 0, _thirdPersonCam.ForwardDirection.z * MoveSpeed * 10f);
                Rigidbody.AddForce(movingDirection, ForceMode.Force);
            }    
        }
        private void Direction(Vector3 direction)
        {  
            _moveDirection = direction;
        }
        private void Jump()
        {
            Vector3 Jumpdir = new Vector3(0, 1, 0);
            Rigidbody.AddForce(Jumpdir * JumpForce, ForceMode.Impulse);
        }      
    }
}