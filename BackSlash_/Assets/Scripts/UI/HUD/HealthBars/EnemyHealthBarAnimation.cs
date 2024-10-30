using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI
{
	public class EnemyHealthBarAnimation : MonoBehaviour
	{
		[Header("Objects")]
		[SerializeField] private CanvasGroup _enemyCG;

		[Header("Bars")]
		[SerializeField] private Image _healthBar;
		[SerializeField] private Image _damageBar;

		[Header("Animation Settings")]
		[SerializeField] private float _fadeDuration = 1f;
		[SerializeField] private float _healthBarTime = 0.2f;
		[SerializeField] private float _damageBarTime = 1.3f;
		[SerializeField] private float _timeToHide;

		private float _maxHealth = 100;
		private float _timeToHideLeft;
		
		private HealthController _health;
		private Camera _camera;

		private bool _isEnemyCanvasVisible;

		private void Awake()
		{
			_health = GetComponent<HealthController>();
			_camera = Camera.main;

			_enemyCG.alpha = 0f;
			_maxHealth = _health.Health;

			_health.OnHealthChanged += FillHealthBar;
			_health.OnDamageTaken += ShowHealthBar;
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

		private void ShowHealthBar()
		{
			_damageBar.enabled = true;
			_enemyCG.alpha = 1f;
			_isEnemyCanvasVisible = true;
			_timeToHideLeft = _timeToHide;
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

			DOTween.To(() => _healthBar.fillAmount, x => _healthBar.fillAmount = x, health / _maxHealth, _healthBarTime).SetEase(Ease.OutExpo);
			DOTween.To(() => _damageBar.fillAmount, x => _damageBar.fillAmount = x, health / _maxHealth, _damageBarTime).SetEase(Ease.InExpo);
		}

		private void OnDestroy()
		{
			_health.OnDamageTaken -= ShowHealthBar;
			_health.OnHealthChanged -= FillHealthBar;
		}
	}
}