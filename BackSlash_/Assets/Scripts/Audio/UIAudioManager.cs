using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class UIAudioManager : MonoBehaviour
{
    //[Header("FMODEvents")]

    [field: SerializeField]
    public EventReference _uiDropEvent { get; private set;}

    [field: SerializeField]
    public EventReference _uiButtonClickEvent {get; private set;}

    [field: SerializeField]
    public EventReference _uiDragEvent {get; private set;}

    [field: SerializeField]
    public EventReference _uiHoverEvent {get; private set;}

    [field: SerializeField]
    public EventReference _uiWindowOpenEvent {get; private set;}


    public void PlayGenericEvent(EventReference uiEvent) 
    { 
        if (!uiEvent.IsNull)
        {
            RuntimeManager.PlayOneShot(uiEvent);
        }  
    }
}
