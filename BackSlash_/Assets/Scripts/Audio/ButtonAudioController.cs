using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;
using Zenject;

public class ButtonAudioController : MonoBehaviour
{
    [SerializeField] private bool _soundOnHover;

    private Button _thisButton;
    private UIAudioManager _audioManager;

    [Inject]
    private void Construct(UIAudioManager uiAudioManager)
    {
        _thisButton = GetComponent<Button>();
        _audioManager = uiAudioManager;

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
            OnHover();
        }).AddTo(this);
    }

    private void OnHover()
    {
        if (_soundOnHover)
        {         
            _audioManager.PlayGenericEvent(_audioManager._uiHoverEvent);
        }
    }

    private void OnClick()
    {
        _audioManager.PlayGenericEvent(_audioManager._uiButtonClickEvent);          
    }
}
