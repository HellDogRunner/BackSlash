using UnityEngine;

namespace Scripts.Player
{
    /// <summary>
    /// Animator driver for slasher locomotion with 2D strafe blend.
    /// Params required:
    /// Float: MoveX (-1..1)
    /// Float: MoveY (-1..1)
    /// Bool : Grounded
    /// Trigger: Jump
    /// Trigger: Dodge
    /// Trigger: Slide
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class SlasherAnimator : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private SlasherMotor _motor;

        [Header("Damping")]
        [SerializeField] private float _damp = 0.06f;

        private static readonly int MoveX = Animator.StringToHash("MoveX");
        private static readonly int MoveY = Animator.StringToHash("MoveY");
        private static readonly int Grounded = Animator.StringToHash("Grounded");
        private static readonly int Jump = Animator.StringToHash("Jump");
        private static readonly int Dodge = Animator.StringToHash("Dodge");
        private static readonly int Slide = Animator.StringToHash("Slide");
        private static readonly int InAir = Animator.StringToHash("InAir");
        private static readonly int VerticalSpeed = Animator.StringToHash("VerticalSpeed");
        private static readonly int Land = Animator.StringToHash("Land");

        private bool _wasGrounded;


        private void Reset()
        {
            _animator = GetComponent<Animator>();
            _motor = GetComponentInParent<SlasherMotor>();
        }

        private void Awake()
        {
            if (_animator == null) _animator = GetComponent<Animator>();
            if (_motor == null) _motor = GetComponentInParent<SlasherMotor>();
        }

        private void OnEnable()
        {
            if (_motor == null) return;

            _motor.Jumped += OnJumped;
            _motor.Dodged += OnDodged;
            _motor.Slid += OnSlid;
        }

        private void OnDisable()
        {
            if (_motor == null) return;

            _motor.Jumped -= OnJumped;
            _motor.Dodged -= OnDodged;
            _motor.Slid -= OnSlid;
        }

        private void Update()
        {
            if (_motor == null) return;

            Vector2 move = _motor.LocalMove; // x=strafe, y=forward/back

            _animator.SetFloat(MoveX, move.x, _damp, Time.deltaTime);
            _animator.SetFloat(MoveY, move.y, _damp, Time.deltaTime);

            _animator.SetBool(Grounded, _motor.Grounded);

            bool grounded = _motor.Grounded;

            _animator.SetBool(Grounded, grounded);
            _animator.SetBool(InAir, _motor.InAirState);
            _animator.SetFloat(VerticalSpeed, _motor.VerticalSpeed);

            if (_wasGrounded == false && grounded == true)
            {
                // только что приземлились
                _animator.SetTrigger(Land);
            }
            _wasGrounded = grounded;

        }

        private void OnJumped() => _animator.SetTrigger(Jump);
        private void OnDodged() => _animator.SetTrigger(Dodge);
        private void OnSlid() => _animator.SetTrigger(Slide);
    }
}
