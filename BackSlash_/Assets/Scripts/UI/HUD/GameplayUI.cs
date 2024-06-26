using Scripts.Player;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Scripts.UI
{
    public class GameplayUI : MonoBehaviour
    {
        [Header("Game Objects")]
        [SerializeField] private GameObject _player;
        [SerializeField] private Image _dodgeTransition;

        [Header("Sliders")]
        [SerializeField] private Slider _healthBar;

        [Header("Fill")]
        [SerializeField] private float _currentHealth;
        [SerializeField] private float _maxHealth;

        private HealthController _health;
        private MovementController _movement;
        private ImageTransition _imageTransition;

        [Inject]
        private void Construct(MovementController movementController)
        {
            _movement = movementController;
        }

        private void Awake()
        {
            _health = _player.GetComponent<HealthController>();

            _imageTransition = _dodgeTransition.GetComponent<ImageTransition>();
        }

        private void Start()
        {
            _healthBar.value = _maxHealth;

            _health.OnHealthChanged += ChangeHealthView;
            _movement.OnDogde += DodgeCooldown;
        }

        public void ChangeHealthView(float _health)
        {
            _currentHealth = _health;
            _healthBar.value = _currentHealth;
        }

        private void DodgeCooldown()
        {
            _imageTransition.StartCooldown();
        }

        private void OnDestroy()
        {
            _health.OnHealthChanged -= ChangeHealthView;
            _movement.OnDogde -= DodgeCooldown;
        }
    }
}