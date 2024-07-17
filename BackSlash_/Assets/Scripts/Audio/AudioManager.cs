using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioManager : MonoBehaviour
{
    [Header("Volume")]

    [Range(0f, 1f)]
    public float masterVolume = 1;

    [Range(0f, 1f)]
    public float ambientVolume = 1;

    [Range(0f, 1f)]
    public float sfxVolume = 1;

    [SerializeField] private bool _startWithMenuAmbience;
    [SerializeField] private bool _startWithGameplayAmbience;

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
        if (_startWithGameplayAmbience)
        {
            InitialazeAmbience(FMODEvents.instance.GameplayAmbience);
            return;
        }
        if (_startWithMenuAmbience)
        {
            InitialazeAmbience(FMODEvents.instance.StartMenuAmbience);
        }
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
