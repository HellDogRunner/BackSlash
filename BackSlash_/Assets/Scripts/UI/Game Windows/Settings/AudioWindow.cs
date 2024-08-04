using FMOD.Studio;
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

            _controller.OnBackKeyPressed += Back;

            _masterVolume.onValueChanged.AddListener((_) => ChangeSliderValue(_masterVolume, _masterValue, 5));
            _musicVolume.onValueChanged.AddListener((_) => ChangeSliderValue(_musicVolume, _musicValue, 5));
            _embientVolume.onValueChanged.AddListener((_) => ChangeSliderValue(_embientVolume, _embientValue, 5));
            _sfxVolume.onValueChanged.AddListener((_) => ChangeSliderValue(_sfxVolume, _sfxValue, 5));

            _back.onClick.AddListener(() => SwitchWindows(_audioHandler, _settingsHandler));
            _close.onClick.AddListener(_windowManager.SwitchPause);
        }

        private void Back()
        {
            SwitchWindows(_audioHandler, _settingsHandler);
        }

        private void OnDestroy()
        {
            _windowManager.OnUnpausing -= DisablePause;

            _controller.OnBackKeyPressed -= Back;

            _masterVolume.onValueChanged.RemoveAllListeners();
            _musicVolume.onValueChanged.RemoveAllListeners();
            _embientVolume.onValueChanged.RemoveAllListeners();
            _sfxVolume.onValueChanged.RemoveAllListeners();

            _back.onClick.RemoveAllListeners();
            _close.onClick.RemoveAllListeners();
        }
    }
}