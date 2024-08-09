using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

// Класс для воспроизведения звука в выпадающем списке
// Пока хз как реализовать нормально

public class DropdownItemAudioController : MonoBehaviour
{
    private Toggle _toggle;
    private AudioController _audioManager;

    [Inject]
    private void Construct(AudioController audioManager)
    {
        _toggle = GetComponent<Toggle>();
        _audioManager = audioManager;
        //_toggle.OnValueChangedAsObservable

        _toggle.OnSelectAsObservable().Subscribe(_ =>
        {
            OnHover();
        }).AddTo(this);
    }

    private void OnHover()
    {
        Debug.Log("245137");
        _audioManager.PlayGenericEvent(FMODEvents.instance.UIHoverEvent);
    }
}
