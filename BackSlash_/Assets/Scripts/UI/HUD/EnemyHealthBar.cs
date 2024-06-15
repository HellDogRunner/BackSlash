using Scripts.Player;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Scripts.UI {
    public class EnemyHealthBar : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private Transform _camera;
        [SerializeField] private GameObject _player;

        [Header("Settings")]
        [SerializeField] private float _fadeDuration;

        private TargetLock _targetLock;
        private HealthController _health;

        private void Start()
        {
            _targetLock = _player.GetComponent<TargetLock>();
            _health = GetComponent<HealthController>();

            _canvas.gameObject.SetActive(false);

            _targetLock.OnStartTargeting += ShowHealthBar;
            _targetLock.OnStopTarteting += HideHealthBar;
            _health.OnEnemyTakeDamage += QuicklyShowHealthBar;
        }

        private void LateUpdate()
        {
            _canvas.transform.LookAt(_camera);
        }

        public void ShowHealthBar(GameObject target)
        {
            if (this.name == target.name)
            {
                _canvas.gameObject.SetActive(true);
            }
        }

        private void HideHealthBar(GameObject target)
        {
            if (this.name == target.name)
            {
                _canvas.gameObject.SetActive(false);
            }
        }

        private void QuicklyShowHealthBar(GameObject target)
        {
            if (this.name == target.name)
            {
                _canvas.gameObject.SetActive(true);
                StartCoroutine(HealthBarFade(_fadeDuration));
            }
        }

        private IEnumerator HealthBarFade(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            _canvas.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            _targetLock.OnStartTargeting -= ShowHealthBar;

        }
    }
}