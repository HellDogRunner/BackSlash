using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisplayTab : BasicTab
{
    [Header("Brightness")]
    [SerializeField] private Slider _brightnessSlider;
    [SerializeField] private TMP_Text _brightnessValue;

    private void OnEnable()
    {
        _brightnessSlider.Select();

        _brightnessSlider.onValueChanged.AddListener(delegate { ChangeSliderValue(_brightnessSlider, _brightnessValue, 5); });
    }

    private void OnDisable()
    {
        _brightnessSlider.onValueChanged.RemoveListener(delegate { ChangeSliderValue(_brightnessSlider, _brightnessValue, 5); });
    }
}
