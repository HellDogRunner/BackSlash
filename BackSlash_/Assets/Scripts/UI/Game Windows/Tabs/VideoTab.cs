using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VideoTab : BasicTab
{
    [Header("Settings Groups")]
    [SerializeField] private Button _brightnessButton;
    [SerializeField] private Slider _brightnessSlider;
    [SerializeField] private TMP_Text _brightnessValue;
    [Space]
    [SerializeField] private Button _videoPresetButton;
    [SerializeField] private TMP_Dropdown _videoPresetDropdown;

    private void OnEnable()
    {
        _selectedTabImage.enabled = true;

        _brightnessButton.Select();
        _brightnessButton.onClick.AddListener(() => { SelectUI(_brightnessSlider); });
        _videoPresetButton.onClick.AddListener(() => { SelectUI(_videoPresetDropdown); });

        _brightnessSlider.onValueChanged.AddListener((x) => { OnSliderChanged(_brightnessSlider, _brightnessValue); });
        _videoPresetDropdown.onValueChanged.AddListener((x) => { VideoPresetChange(_videoPresetDropdown); });
    }

    private void OnDisable()
    {
        _selectedTabImage.enabled = false;

        _brightnessButton.onClick.RemoveAllListeners();
        _videoPresetButton.onClick.RemoveAllListeners();

        _brightnessSlider.onValueChanged.RemoveAllListeners();
        _videoPresetDropdown.onValueChanged.RemoveAllListeners();
    }

    private void VideoPresetChange(TMP_Dropdown dropdown)
    {
        PlayHoverSound();
        QualitySettings.SetQualityLevel(dropdown.value);
    }
}
