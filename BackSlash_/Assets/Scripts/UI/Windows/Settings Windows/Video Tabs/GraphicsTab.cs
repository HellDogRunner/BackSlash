using TMPro;
using UnityEngine;

public class GraphicsTab : BasicTab
{
    [Header("Preset")]
    [SerializeField] private TMP_Dropdown _videoPresetDropdown;

    private void OnEnable()
    {
        _videoPresetDropdown.Select();

        _videoPresetDropdown.onValueChanged.AddListener(delegate { VideoPresetChange(_videoPresetDropdown); });

        _videoPresetDropdown.value = QualitySettings.GetQualityLevel();
    }

    private void OnDisable()
    {
        _videoPresetDropdown.onValueChanged.RemoveListener(delegate { VideoPresetChange(_videoPresetDropdown); });
    }

    private void VideoPresetChange(TMP_Dropdown dropdown)
    {
        PlayHoverSound();
        QualitySettings.SetQualityLevel(dropdown.value);
    }
}
