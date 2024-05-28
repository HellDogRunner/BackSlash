using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthView : MonoBehaviour
{
    [SerializeField] private Slider _healthBar;
    private HealthController _health;

    void Start()
    {
        _health = GetComponent<HealthController>();

        _health.OnHealthChanged += FillHealthBar;
    }

    private void FillHealthBar(float health)
    {
        _healthBar.value = health / 100f;
        if (_health.Health <= 0)
        {
            _healthBar.gameObject.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        _health.OnHealthChanged -= FillHealthBar;
    }
}
