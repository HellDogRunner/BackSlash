using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioController : MonoBehaviour
{
    [Header("Volume")]
    [Range(0f, 1f)] public float masterVolume = 1;
    [Range(0f, 1f)] public float ambientVolume = 1;
    [Range(0f, 1f)] public float sfxVolume = 1;

    private List<EventInstance> _eventInstances = new List<EventInstance>();

    private EventInstance _ambientEventInstance;

    private Bus masterBus;
    private Bus ambientBus;
    private Bus sfxBus;

    private void Awake()
    {
        masterBus = RuntimeManager.GetBus("bus:/");
        ambientBus = RuntimeManager.GetBus("bus:/Ambience");
        sfxBus = RuntimeManager.GetBus("bus:/SFX");
    }

    private void Start()
    {
        InitialazeAmbience(FMODEvents.instance.LocationMusic);
    }

    private void InitialazeAmbience(EventReference abienceEventReference) 
    {
        _ambientEventInstance = CreateEventInstance(abienceEventReference);
        _ambientEventInstance.start();
    }

    private void Update()
    {
        masterBus.setVolume(masterVolume);
        ambientBus.setVolume(ambientVolume);
        sfxBus.setVolume(sfxVolume);
    }

    public void PlayGenericEvent(EventReference eventRef) 
    { 
        if (!eventRef.IsNull)
        {
            RuntimeManager.PlayOneShot(eventRef);
        }  
    }

    public void PlayGenericEvent(EventReference eventRef, Vector3 point) 
    { 
        if (!eventRef.IsNull)
        {
            RuntimeManager.PlayOneShot(eventRef, point);
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
