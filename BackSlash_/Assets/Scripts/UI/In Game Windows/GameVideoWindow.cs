using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RedMoonGames.Window
{
    public class GameVideoWindow : GameBasicWindow
    {
        [SerializeField] private WindowHandler _videoHandler;
        
        [Header("Configuration Elements")]
        [SerializeField] private TMP_Dropdown _videoDropdown;
        [SerializeField] private Slider _brightnessSlider;
        [SerializeField] private TMP_Text _brightnessValue;

        [Header("Definition Texts")]
        [SerializeField] private Button _videoPresetButton;
        [SerializeField] private Button _brightnessButton;

        private void OnEnable()
        {
            _brightnessButton.Select();

            _videoDropdown.onValueChanged.AddListener((x) => { ChangeVideoPreset(); });
            _brightnessSlider.onValueChanged.AddListener(delegate { ChangeBrightness(); });
        }

        private void OnDisable()
        {
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

        protected override void HideCurrentWindow()
        {
            _animationController.HideWindowAnimation(_canvasGroup, _videoHandler);
        }
    }
}