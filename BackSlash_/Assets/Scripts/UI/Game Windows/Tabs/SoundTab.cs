using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RedMoonGames.Window
{
    public class SoundTab : BasicTab
    {
        [Header("Settings Groups")]
        [SerializeField] private Button _totalButton;
        [SerializeField] private Slider _totalSlider;
        [SerializeField] private TMP_Text _totalValue;
        [Space]
        [SerializeField] private Button _sFXButton;
        [SerializeField] private Slider _sFXSlider;
        [SerializeField] private TMP_Text _sFXValue;
        [Space]
        [SerializeField] private Button _musicButton;
        [SerializeField] private Slider _musicSlider;
        [SerializeField] private TMP_Text _musicValue;
        [Space]
        [SerializeField] private Button _embientButton;
        [SerializeField] private Slider _embientSlider;
        [SerializeField] private TMP_Text _embientValue;

        private void OnEnable()
        {
            _audioManager.PlayGenericEvent(FMODEvents.instance.UIButtonClickEvent);

            _selectedTabImage.enabled = true;

            _totalButton.Select();
            _totalButton.onClick.AddListener(() => { SelectUI(_totalSlider); });
            _sFXButton.onClick.AddListener(() => { SelectUI(_sFXSlider); });
            _musicButton.onClick.AddListener(() => { SelectUI(_musicSlider); });
            _embientButton.onClick.AddListener(() => { SelectUI(_embientSlider); });

            _totalSlider.onValueChanged.AddListener((x) => { OnUIChanged(_totalSlider, _totalValue); });
            _sFXSlider.onValueChanged.AddListener((x) => { OnUIChanged(_sFXSlider, _sFXValue); });
            _musicSlider.onValueChanged.AddListener((x) => { OnUIChanged(_musicSlider, _musicValue); });
            _embientSlider.onValueChanged.AddListener((x) => { OnUIChanged(_embientSlider, _embientValue); });
        }

        private void OnDisable()
        {
            _selectedTabImage.enabled = false;

            _totalButton.onClick.RemoveListener(() => { SelectUI(_totalSlider); });
            _sFXButton.onClick.RemoveListener(() => { SelectUI(_sFXSlider); });
            _musicButton.onClick.RemoveListener(() => { SelectUI(_musicSlider); });
            _embientButton.onClick.RemoveListener(() => { SelectUI(_embientSlider); });

            _totalSlider.onValueChanged.RemoveAllListeners();
            _sFXSlider.onValueChanged.RemoveAllListeners();
            _musicSlider.onValueChanged.RemoveAllListeners();
            _embientSlider.onValueChanged.RemoveAllListeners();
        }
    }
}