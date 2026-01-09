using UnityEngine;

namespace Scripts.Player
{
    [RequireComponent(typeof(Animator))]
    public class SlasherAnimator : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        [Header("Sources")]
        [SerializeField] private SlasherInput _input;
        [SerializeField] private SlasherMotor _groundMotor;
        [SerializeField] private RocketBootsMotor _bootsMotor;
        [SerializeField] private RocketBootsController _boots;

        [Header("Damping")]
        [SerializeField] private float _damp = 0.06f;

        private static readonly int MoveX = Animator.StringToHash("MoveX");
        private static readonly int MoveY = Animator.StringToHash("MoveY");
        private static readonly int Grounded = Animator.StringToHash("Grounded");
        private static readonly int InAir = Animator.StringToHash("InAir");
        private static readonly int VerticalSpeed = Animator.StringToHash("VerticalSpeed");
        private static readonly int BootsOn = Animator.StringToHash("BootsOn");

        private static readonly int Jump = Animator.StringToHash("Jump");
        private static readonly int Land = Animator.StringToHash("Land");
        private static readonly int Dodge = Animator.StringToHash("Dodge");
        private static readonly int Slide = Animator.StringToHash("Slide");

        private void Reset()
        {
            _animator = GetComponent<Animator>();
            _input = GetComponentInParent<SlasherInput>();
            _groundMotor = GetComponentInParent<SlasherMotor>();
            _bootsMotor = GetComponentInParent<RocketBootsMotor>();
            _boots = GetComponentInParent<RocketBootsController>();
        }

        private void Awake()
        {
            if (_animator == null) _animator = GetComponent<Animator>();
            if (_input == null) _input = GetComponentInParent<SlasherInput>();
            if (_groundMotor == null) _groundMotor = GetComponentInParent<SlasherMotor>();
            if (_bootsMotor == null) _bootsMotor = GetComponentInParent<RocketBootsMotor>();
            if (_boots == null) _boots = GetComponentInParent<RocketBootsController>();
        }

        private void OnEnable()
        {
            if (_groundMotor != null)
            {
                _groundMotor.Jumped += OnGroundJumped;
                _groundMotor.Landed += OnGroundLanded;
                _groundMotor.Dodged += OnDodged;
                _groundMotor.Slid += OnSlid;
            }

            if (_bootsMotor != null)
            {
                _bootsMotor.Jumped += OnBootsJumped;
                _bootsMotor.Landed += OnBootsLanded;
            }
        }

        private void OnDisable()
        {
            if (_groundMotor != null)
            {
                _groundMotor.Jumped -= OnGroundJumped;
                _groundMotor.Landed -= OnGroundLanded;
                _groundMotor.Dodged -= OnDodged;
                _groundMotor.Slid -= OnSlid;
            }

            if (_bootsMotor != null)
            {
                _bootsMotor.Jumped -= OnBootsJumped;
                _bootsMotor.Landed -= OnBootsLanded;
            }
        }

        private void Update()
        {
            // Move всегда из input
            Vector2 move = _input != null ? _input.Move : Vector2.zero;
            _animator.SetFloat(MoveX, move.x, _damp, Time.deltaTime);
            _animator.SetFloat(MoveY, move.y, _damp, Time.deltaTime);

            bool bootsOn = _boots != null && _boots.BootsOn;
            _animator.SetBool(BootsOn, bootsOn);

            bool grounded;
            float vSpeed;

            if (bootsOn && _bootsMotor != null && _bootsMotor.isActiveAndEnabled)
            {
                grounded = _bootsMotor.Grounded;
                vSpeed = _bootsMotor.VerticalSpeed;
            }
            else if (_groundMotor != null)
            {
                grounded = _groundMotor.Grounded;
                vSpeed = _groundMotor.VerticalSpeed;
            }
            else
            {
                grounded = true;
                vSpeed = 0f;
            }

            _animator.SetBool(Grounded, grounded);
            _animator.SetBool(InAir, !grounded);
            _animator.SetFloat(VerticalSpeed, vSpeed);
        }

        // --- Ground motor events ---
        private void OnGroundJumped()
        {
            if (_boots != null && _boots.BootsOn) return;
            _animator.SetTrigger(Jump);
        }

        private void OnGroundLanded()
        {
            if (_boots != null && _boots.BootsOn) return;
            _animator.SetTrigger(Land);
        }

        private void OnDodged() => _animator.SetTrigger(Dodge);
        private void OnSlid() => _animator.SetTrigger(Slide);

        // --- Boots motor events ---
        private void OnBootsJumped()
        {
            if (_boots == null || !_boots.BootsOn) return;
            _animator.SetTrigger(Jump);
        }

        private void OnBootsLanded()
        {
            if (_boots == null || !_boots.BootsOn) return;
            _animator.SetTrigger(Land);
        }
    }
}
