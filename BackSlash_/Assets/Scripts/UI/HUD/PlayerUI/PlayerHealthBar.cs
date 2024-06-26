using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI
{
    public class PlayerHealthBar : MonoBehaviour
    {
        [Header("Objects")]
        [SerializeField] private GameObject _player;

        [Header("Bars")]
        [SerializeField] private Image _healthBar;
        [SerializeField] private Image _damageBar;

        [Header("Decrease Time")]
        [SerializeField] private float _healthBarTime = 0.2f;
        [SerializeField] private float _damageBarTime = 1.3f;

        [Header("indicators")]
        [SerializeField] private float _maxHealth = 100;
        [SerializeField] private float _currentHealth = 0;

        private HealthController _healthController;

        private void Awake()
        {
            _healthController = _player.GetComponent<HealthController>();

            _maxHealth = _healthController.Health;
            _currentHealth = _maxHealth;
        }

        private void Start()
        {
            _healthController.OnHealthChanged += HealthChanged;
        }

        private void HealthChanged(float health)
        {
            if (health < _currentHealth)
            {
                DecreaseHealth(health);
            }
        }

        private void DecreaseHealth(float health)
        {
            DOTween.To(() => _healthBar.fillAmount, x => _healthBar.fillAmount = x, health / _maxHealth, _healthBarTime);
            DOTween.To(() => _damageBar.fillAmount, x => _damageBar.fillAmount = x, health / _maxHealth, _damageBarTime);
            _currentHealth = health;
        }

        private void OnDestroy()
        {
            _healthController.OnHealthChanged -= HealthChanged;
        }
    }
}
