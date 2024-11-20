using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Scripts.UI
{
	public class PlayerHealthBarAnimation : MonoBehaviour
	{
		[Header("Bars")]
		[SerializeField] private Image _healthBar;

		[Header("Decrease Time")]
		[SerializeField] private float _healthBarTime = 0.2f;

		[Header("indicators")]
		[SerializeField] private float _maxHealth;
		[SerializeField] private float _currentHealth;

		private HealthController _playerHealth;

		[Inject]
		private void Construct(HealthController playerHealth)
		{
			_playerHealth = playerHealth;
		}

		private void Awake()
		{
			_maxHealth = _playerHealth.Health;
			_currentHealth = _maxHealth;
		}

		private void OnEnable()
		{
			_playerHealth.OnHealthChanged += HealthChanged;
		}

		private void OnDisable()
		{
			_playerHealth.OnHealthChanged -= HealthChanged;
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
			DOTween.To(() => _healthBar.fillAmount, x => _healthBar.fillAmount = x, health / _maxHealth, _healthBarTime).SetEase(Ease.InOutCubic);
			_currentHealth = health;
		}
	}
}