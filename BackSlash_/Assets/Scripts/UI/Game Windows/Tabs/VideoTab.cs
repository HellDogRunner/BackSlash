using TMPro;
using Unity.VisualScripting;
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
        _audioManager.PlayGenericEvent(FMODEvents.instance.UIButtonClickEvent);

        _selectedTabImage.enabled = true;

        _brightnessButton.Select();
        _brightnessButton.onClick.AddListener(() => { SelectUI(_brightnessSlider); });
        _videoPresetButton.onClick.AddListener(() => { SelectUI(_videoPresetDropdown); });

        _brightnessSlider.onValueChanged.AddListener((x) => { OnUIChanged(_brightnessSlider, _brightnessValue); });
        _videoPresetDropdown.onValueChanged.AddListener((x) => { OnUIChanged(_videoPresetDropdown); });
    }

    private void OnDisable()
    {
        _selectedTabImage.enabled = false;

        _brightnessButton.onClick.RemoveListener(() => { SelectUI(_brightnessSlider); });
        _videoPresetButton.onClick.RemoveListener(() => { SelectUI(_videoPresetDropdown); });

        _brightnessSlider.onValueChanged.RemoveAllListeners();
        _videoPresetDropdown.onValueChanged.RemoveAllListeners();
    }
}
