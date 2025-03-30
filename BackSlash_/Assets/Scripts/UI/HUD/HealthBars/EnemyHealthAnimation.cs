using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI
{
	public class EnemyHealthAnimation : MonoBehaviour
	{
		[SerializeField] private HealthController _healthController;
		
		[Header("Bars")]
		[SerializeField] private Image _healthBar;
		[SerializeField] private Image _damageBar;

		[Header("Settings")]
		[SerializeField] private float _fadeDuration;
		[SerializeField] private float _healthTime;
		[SerializeField] private float _damageTime;
		[SerializeField] private float _timeToHide;
		
		private CanvasGroup _canvasGroup;
		private Camera _camera;

		private Coroutine _coroutine;
		
		private Tween _hideTween;
		
		private float _maxHealth;

		private void Awake()
		{
			_camera = Camera.main;
			_canvasGroup = GetComponent<CanvasGroup>();
		}

		private void OnEnable()
		{
			_healthController.OnHealthChanged += FillHealthBar;
			_healthController.OnDamageTaken += ShowHealthBar;
			_healthController.OnDeath += DisableCanvas;
		}

        private void OnDisable()
        {
            _healthController.OnDamageTaken -= ShowHealthBar;
			_healthController.OnHealthChanged -= FillHealthBar;
			_healthController.OnDeath -= DisableCanvas;
        }

		private void Start()
		{
			_canvasGroup.alpha = 0f;
			_maxHealth = _healthController.Health;
		}

		private void LateUpdate()
		{
			transform.LookAt(_camera.transform.position);
		}

		private void DisableCanvas()
		{
		    _canvasGroup.gameObject.SetActive(false);
		}

		private void ShowHealthBar()
		{
			if (_coroutine != null)
			{
				_hideTween.Complete();
				StopCoroutine(_coroutine);
			}
			_coroutine = StartCoroutine(HideDelay());
		}

		private void FillHealthBar(int before, int after)
		{
			_healthBar.DOFillAmount(after / _maxHealth, _healthTime).From(before / _maxHealth).SetEase(Ease.OutExpo);
			if (after < before)
				_damageBar.DOFillAmount(after / _maxHealth, _damageTime).From(before / _maxHealth).SetEase(Ease.InExpo);
			
			// DOTween.To(() => _healthBar.fillAmount, x => _healthBar.fillAmount = x, health / _maxHealth, _healthTime).SetEase(Ease.OutExpo);
			// DOTween.To(() => _damageBar.fillAmount, x => _damageBar.fillAmount = x, health / _maxHealth, _damageTime).SetEase(Ease.InExpo);
		}
		
		private IEnumerator HideDelay()
		{
			_damageBar.enabled = true;
			_canvasGroup.alpha = 1f;
		
			yield return new WaitForSeconds(_timeToHide);
			
			_damageBar.enabled = false;
			_hideTween = _canvasGroup.DOFade(0f, _fadeDuration);
		}
	}
}
