using UnityEngine;

namespace Scripts.UI
{
    public class EnemyHealthBar : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private Transform _camera;
        [SerializeField] private GameObject _player;

        [Header("Settings")]
        [SerializeField] private float _fadeDuration;
        [SerializeField] private float _timeToHide;

        private HealthController _health;

        private bool _isActive;

        private void Awake()
        {
            _health = GetComponent<HealthController>();

            _health.OnEnemyTakeDamage += QuicklyShowHealthBar;
        }

        private void Start()
        {     
            _canvas.gameObject.SetActive(false);

            _isActive = false;
        }

        private void Update()
        {
            if (_isActive)
            {
                _timeToHide -= Time.deltaTime;
                if (_timeToHide < 0)
                {
                    _canvas.gameObject.SetActive(false);
                    _isActive = false;
                }
            }
        }

        private void LateUpdate()
        {
            _canvas.transform.LookAt(_camera);
        }

        private void QuicklyShowHealthBar(GameObject target)
        {
            if (this.name == target.name)
            {
                _canvas.gameObject.SetActive(true);
                _isActive = true;
                _timeToHide = _fadeDuration;
            }
        }

        private void OnDestroy()
        {
            _health.OnEnemyTakeDamage -= QuicklyShowHealthBar;
        }
    }
}