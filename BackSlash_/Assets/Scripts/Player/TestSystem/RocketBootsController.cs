using UnityEngine;

namespace Scripts.Player
{
    public class RocketBootsController : MonoBehaviour
    {
        [SerializeField] private SlasherInput _input;

        [Header("Motors")]
        [SerializeField] private SlasherMotor _groundMotor;
        [SerializeField] private RocketBootsMotor _bootsMotor;

        [Header("Animator")]
        [SerializeField] private Animator _animator;
        [SerializeField] private string _bootsParam = "BootsOn";

        [Header("VFX")]
        [SerializeField] private Scripts.VFX.RocketBootsVFX _vfx;

        public bool BootsOn { get; private set; }

        private int _bootsHash;

        private void Awake()
        {
            if (_input == null) _input = GetComponent<SlasherInput>();
            if (_groundMotor == null) _groundMotor = GetComponent<SlasherMotor>();
            if (_bootsMotor == null) _bootsMotor = GetComponent<RocketBootsMotor>();
            if (_animator == null) _animator = GetComponentInChildren<Animator>();

            _bootsHash = Animator.StringToHash(_bootsParam);

            ApplyState(false);
        }

        private void OnEnable()
        {
            if (_input == null) return;
            _input.BootsTogglePressed += Toggle;
        }

        private void OnDisable()
        {
            if (_input == null) return;
            _input.BootsTogglePressed -= Toggle;
        }

        private void Update()
        {
            if (BootsOn && _vfx != null && _bootsMotor != null)
                _vfx.SetThrottle(_bootsMotor.Throttle01);
        }

        private void Toggle()
        {
            ApplyState(!BootsOn);
        }

        private void ApplyState(bool on)
        {
            BootsOn = on;

            if (_groundMotor != null) _groundMotor.enabled = !on;
            if (_bootsMotor != null) _bootsMotor.SetActive(on);

            if (_animator != null) _animator.SetBool(_bootsHash, on);

            if (_vfx != null)
            {
                _vfx.SetActive(on);
                _vfx.SetThrottle(on ? 0.5f : 0f);
            }
        }
    }
}
