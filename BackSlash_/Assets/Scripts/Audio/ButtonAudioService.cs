using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ButtonAudioService : MonoBehaviour
{
    [SerializeField] private bool _soundOnHover;
    [SerializeField] private bool _soundOnClick;

    private Button _thisButton;
    private AudioManager _audioManager;

    [Inject]
    private void Construct(AudioManager audioManager)
    {
        _thisButton = GetComponent<Button>();
        _audioManager = audioManager;

        _thisButton.onClick.AsObservable().Subscribe(_ =>
        {
            OnClick();
        }).AddTo(this);

        _thisButton.OnSelectAsObservable().Subscribe(_ =>
        {
            OnHover();
        }).AddTo(this);


        _thisButton.OnPointerEnterAsObservable().Subscribe(_ =>
        {
            //OnHover();
        }).AddTo(this);
    }

    private void OnHover()
    {
        if (_soundOnHover)
        {
            _audioManager.PlayGenericEvent(FMODEvents.instance.UIHoverEvent);
        }
    }

    private void OnClick()
    {
        if (_soundOnClick)
        {
            _audioManager.PlayGenericEvent(FMODEvents.instance.UIButtonClickEvent);
        }
    }
}
