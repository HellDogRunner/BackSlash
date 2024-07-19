using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VideoTab : BasicTab
{
    [Header("Configuration Elements")]
    [SerializeField] private TMP_Dropdown _videoDropdown;
    [SerializeField] private Slider _brightnessSlider;
    [SerializeField] private TMP_Text _brightnessValue;

    [Header("Definition Texts")]
    [SerializeField] private Button _videoPresetButton;
    [SerializeField] private Button _brightnessButton;

    protected override void OnEnable()
    {
        _selectedImage.enabled = true;
        _brightnessButton.Select();

        _videoDropdown.onValueChanged.AddListener((x) => { ChangeVideoPreset(); });
        _brightnessSlider.onValueChanged.AddListener(delegate { ChangeBrightness(); });
    }

    protected override void OnDisable()
    {
        _selectedImage.enabled = false;
        _videoDropdown.onValueChanged.RemoveListener((x) => { ChangeVideoPreset(); });
        _brightnessSlider.onValueChanged.RemoveListener(delegate { ChangeBrightness(); });
    }

    private void ChangeVideoPreset()
    {
        QualitySettings.SetQualityLevel(_videoDropdown.value);
    }

    private void ChangeBrightness()
    {
        _brightnessValue.text = _brightnessSlider.value.ToString();
    }
}
