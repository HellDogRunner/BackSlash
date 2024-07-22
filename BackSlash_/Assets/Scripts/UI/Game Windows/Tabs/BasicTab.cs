using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class BasicTab : MonoBehaviour
{
    [SerializeField] protected Image _selectedTabImage;

    protected AudioManager _audioManager;

    [Inject]
    private void Construct(AudioManager audioManager)
    {
        _audioManager = audioManager;
    }

    protected void SelectUI(Slider slider)
    {
        slider.Select();
    }

    protected void SelectUI(TMP_Dropdown dropdown)
    {
        dropdown.Select();
    }

    protected void OnUIChanged(Slider slider, TMP_Text value)
    {
        _audioManager.PlayGenericEvent(FMODEvents.instance.UIHoverEvent);
        value.text = slider.value.ToString();
    }

    protected void OnUIChanged(TMP_Dropdown dropdown)
    {
        _audioManager.PlayGenericEvent(FMODEvents.instance.UIHoverEvent);
        QualitySettings.SetQualityLevel(dropdown.value);
    }
}