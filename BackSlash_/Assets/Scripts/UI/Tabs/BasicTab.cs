using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class BasicTab : MonoBehaviour
{
    private AudioController _audioManager;

    [Inject]
    private void Construct(AudioController audioManager)
    {
        _audioManager = audioManager;
    }

    protected void ChangeSliderValue(Slider slider, TMP_Text value, int multiplier)
    {
        PlayHoverSound();
        value.text = (slider.value * multiplier).ToString();
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