using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private bool _startWithMenuAmbience;
    [SerializeField] private bool _startWithGameplayAmbience;

    private List<EventInstance> _eventInstances = new List<EventInstance>();

    private EventInstance _ambientEventInstance;

    private void Start()
    {
        if (_startWithGameplayAmbience)
        {
            InitialazeAmbience(FMODEvents.instance.GameplayAmbience);
            _startWithMenuAmbience = false;
        }
        if (_startWithMenuAmbience)
        {
            InitialazeAmbience(FMODEvents.instance.StartMenuAmbience);
            _startWithGameplayAmbience = false;
        }
    }

    private void InitialazeAmbience(EventReference abienceEventReference) 
    {
        _ambientEventInstance = CreateEventInstance(abienceEventReference);
        _ambientEventInstance.start();
    }

    public void PlayGenericEvent(EventReference uiEvent) 
    { 
        if (!uiEvent.IsNull)
        {
            RuntimeManager.PlayOneShot(uiEvent);
        }  
    }

    public EventInstance CreateEventInstance(EventReference eventReference) 
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        _eventInstances.Add(eventInstance);
        return eventInstance;
    }

    private void CleanUp()
    {
        foreach (var eventInstance in _eventInstances)
        {
            eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            eventInstance.release();
        }
    }

    private void OnDestroy()
    {
        CleanUp();
    }
}
