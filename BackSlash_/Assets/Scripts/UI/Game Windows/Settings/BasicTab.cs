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
        PlayClickSound();
        slider.Select();
    }

    protected void SelectUI(TMP_Dropdown dropdown)
    {
        PlayClickSound();
        dropdown.Select();
    }

    protected void OnSliderChanged(Slider slider, TMP_Text value)
    {
        PlayHoverSound();
        value.text = slider.value.ToString();
    }

    protected int OnDropdownChanged(TMP_Dropdown dropdown)
    {
        PlayHoverSound();
        return dropdown.value;
    }

    protected void PlayHoverSound()
    {
        _audioManager.PlayGenericEvent(FMODEvents.instance.UIHoverEvent);
    }

    protected void  PlayClickSound()
    {
        _audioManager.PlayGenericEvent(FMODEvents.instance.UIButtonClickEvent);
    }
}