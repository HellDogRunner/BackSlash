using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RedMoonGames.Window
{
    public class AudioWindow : GameBasicWindow
    {
        [Header("Handlers")]
        [SerializeField] private WindowHandler _audioHandler;
        [SerializeField] private WindowHandler _settingsHandler;

        [Header("Sliders")]
        [SerializeField] private Slider _masterVolume;
        [SerializeField] private Slider _musicVolume;
        [SerializeField] private Slider _embientVolume;
        [SerializeField] private Slider _sfxVolume;

        [Header("Slider Values")]
        [SerializeField] private TMP_Text _masterValue;
        [SerializeField] private TMP_Text _musicValue;
        [SerializeField] private TMP_Text _embientValue;
        [SerializeField] private TMP_Text _sfxValue;

        [Header("Navigation Keys")]
        [SerializeField] private Button _back;
        [SerializeField] private Button _close;

        private void Awake()
        {
            _masterVolume.Select();

            _pauseInputs.OnBackKeyPressed += BackButton;

            _masterVolume.onValueChanged.AddListener(delegate { ChangeSliderValue(_masterVolume, _masterValue, 5); });
            _musicVolume.onValueChanged.AddListener(delegate { ChangeSliderValue(_musicVolume, _musicValue, 5); });
            _embientVolume.onValueChanged.AddListener(delegate { ChangeSliderValue(_embientVolume, _embientValue, 5); });
            _sfxVolume.onValueChanged.AddListener(delegate { ChangeSliderValue(_sfxVolume, _sfxValue, 5); });

            _back.onClick.AddListener(BackButton);
            if (!_IsMainMenu) _close.onClick.AddListener(_windowService.Unpause);
        }

        private void BackButton() { SwitchWindows(_audioHandler, _settingsHandler); }

        private void OnDestroy()
        {
            _windowService.OnHideWindow -= DisablePause;

            _pauseInputs.OnBackKeyPressed -= BackButton;

            _masterVolume.onValueChanged.RemoveListener(delegate { ChangeSliderValue(_masterVolume, _masterValue, 5); });
            _musicVolume.onValueChanged.RemoveListener(delegate { ChangeSliderValue(_musicVolume, _musicValue, 5); });
            _embientVolume.onValueChanged.RemoveListener(delegate { ChangeSliderValue(_embientVolume, _embientValue, 5); });
            _sfxVolume.onValueChanged.RemoveListener(delegate { ChangeSliderValue(_sfxVolume, _sfxValue, 5); });

            _back.onClick.RemoveListener(BackButton);
            if (!_IsMainMenu) _close.onClick.RemoveListener(_windowService.Unpause);
        }
    }
}