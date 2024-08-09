using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI
{
    public class EnemyHealthBar : MonoBehaviour
    {
        [Header("Objects")]
        [SerializeField] private CanvasGroup _enemyCG;

        [Header("Bars")]
        [SerializeField] private Image _healthBar;
        [SerializeField] private Image _damageBar;

        [Header("Animation Settings")]
        [SerializeField] private float _healthBarTime = 0.2f;
        [SerializeField] private float _damageBarTime = 1.3f;
        [Space]
        [SerializeField] private float _fadeDuration = 1f;
        [SerializeField] private float _timeToHide;
        [SerializeField] private float _timeToHideLeft;

        [Header("indicators")]
        [SerializeField] private float _maxHealth = 100;
        [SerializeField] private float _currentHealth = 0;

        private HealthController _health;
        private Camera _camera;

        private bool _isEnemyCanvasVisible;

        private void Awake()
        {
            _health = GetComponent<HealthController>();
            _camera = Camera.main;

            _enemyCG.alpha = 0f;
            _maxHealth = _health.Health;
            _currentHealth = _maxHealth;

            _health.OnEnemyTakeDamage += ShowHealthBar;
            _health.OnHealthChanged += FillHealthBar;
        }

        private void Update()
        {
            if (_isEnemyCanvasVisible)
            {
                _timeToHideLeft -= Time.deltaTime;
                if (_timeToHideLeft < 0)
                {
                    HideHealthBar();
                }
            }
        }

        private void LateUpdate()
        {
            _enemyCG.transform.LookAt(_camera.transform.position);
        }

        private void ShowHealthBar(GameObject target)
        {
            if (this.name == target.name)
            {
                _damageBar.enabled = true;
                _enemyCG.alpha = 1f;
                _isEnemyCanvasVisible = true;
                _timeToHideLeft = _timeToHide;
            }
        }

        private void HideHealthBar()
        {
            _damageBar.enabled = false;
            _isEnemyCanvasVisible = false;
            _enemyCG.DOFade(0f, _fadeDuration);
        }

        private void FillHealthBar(float health)
        {
            if (_health.Health <= 0)
            {
                _enemyCG.gameObject.SetActive(false);
            }

            _currentHealth = health;
            DOTween.To(() => _healthBar.fillAmount, x => _healthBar.fillAmount = x, health / _maxHealth, _healthBarTime).SetEase(Ease.OutExpo);
            DOTween.To(() => _damageBar.fillAmount, x => _damageBar.fillAmount = x, health / _maxHealth, _damageBarTime).SetEase(Ease.InExpo);
        }

        private void OnDestroy()
        {
            _health.OnEnemyTakeDamage -= ShowHealthBar;
            _health.OnHealthChanged -= FillHealthBar;
        }
    }
}