using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Scripts.UI
{
	public class PlayerHealthAnimation : MonoBehaviour
	{
		[SerializeField] private Image _healthBar;
		[Space]
		[SerializeField] private float _changeTime;

		private float _maxHealth;

		private HealthController _playerHealth;

		[Inject]
		private void Construct(HealthController playerHealth)
		{
			_playerHealth = playerHealth;
		}

		private void Start()
		{
			_maxHealth = _playerHealth.Health;
		}

		private void OnEnable()
		{
			_playerHealth.OnHealthChanged += HealthChanged;
		}

		private void OnDisable()
		{
			_playerHealth.OnHealthChanged -= HealthChanged;
		}

		private void HealthChanged(int before, int after)
		{
			_healthBar.DOFillAmount(after / _maxHealth, _changeTime).From(before / _maxHealth).SetEase(Ease.InOutCubic);
		}
	}
}
