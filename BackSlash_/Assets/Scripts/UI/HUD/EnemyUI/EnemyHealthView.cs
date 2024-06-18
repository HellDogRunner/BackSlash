using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthView : MonoBehaviour
{
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
        _healthController = GetComponent<HealthController>();

        _maxHealth = _healthController.Health;
        _currentHealth = _maxHealth;
    }

    private void Start()
    {
        _healthController.OnHealthChanged += FillHealthBar;
    }

    private void FillHealthBar(float health)
    {
        _currentHealth = health;
        DOTween.To(() => _healthBar.fillAmount, x => _healthBar.fillAmount = x, health / _maxHealth, _healthBarTime);
        DOTween.To(() => _damageBar.fillAmount, x => _damageBar.fillAmount = x, health / _maxHealth, _damageBarTime);
        

        if (_healthController.Health <= 0)
        {
            _healthBar.gameObject.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        _healthController.OnHealthChanged -= FillHealthBar;
    }
}
