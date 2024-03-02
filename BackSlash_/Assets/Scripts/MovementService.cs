using UnityEngine;

public class MovementService : MonoBehaviour
{
    [SerializeField] private Rigidbody Rigidbody;
    [SerializeField] private Transform Orientation;
    [Header("Movement")]
    [SerializeField] private float MoveSpeed;
    [SerializeField] private float GroundDrag;
    [SerializeField] private float JumpForce;
    [SerializeField] private float AirMultiplier;
    [Header("Key Binds")]
    [SerializeField] private KeyCode JumpKey = KeyCode.Space;
    [Header("Ground Check")]
    [SerializeField] private float PlayerHeight;
    [SerializeField] private LayerMask IsGround;

    bool _grounded;
    private float _horizontal, _vertical;
    private Vector3 _moveDirection;

    private void Update()
    {
        PlayerInput();
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
        MovePlayer();
    }

    private void PlayerInput()
    {
        _horizontal = Input.GetAxisRaw("Horizontal");
        _vertical = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(JumpKey) && _grounded)
        {
            Jump();
        }
    }

    public Vector3 ReturnAxis()
    {
        return new Vector2(_horizontal, _vertical);
    }

    private void MovePlayer()
    {
        _moveDirection = Orientation.forward * _vertical + Orientation.right * _horizontal;

        if (_grounded)
        {
            Rigidbody.AddForce(_moveDirection * MoveSpeed * 10f, ForceMode.Force);
        }
    }

    private void Jump()
    {
        Rigidbody.AddForce(Rigidbody.transform.up * JumpForce, ForceMode.Impulse);
    }
}
