using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FMODEvents : MonoBehaviour
{
    [field: Header("Ambience SFX")]
    [field: SerializeField]
    public EventReference StartMenuAmbience { get; private set; }
    [field: SerializeField]
    public EventReference GameplayAmbience { get; private set; }

    [field: Header("UI SFX")]
    [field: SerializeField]
    public EventReference UIDropEvent { get; private set; }

    [field: SerializeField]
    public EventReference UIButtonClickEvent { get; private set; }

    [field: SerializeField]
    public EventReference UIDragEvent { get; private set; }

    [field: SerializeField]
    public EventReference UIHoverEvent { get; private set; }

    [field: SerializeField]
    public EventReference UIWindowOpenEvent { get; private set; }

    [field: Header("Player SFX")]

    [field: SerializeField]
    public EventReference PlayerFootSteps { get; private set; }

    [field: SerializeField]
    public EventReference PlayerLanded { get; private set; }

    [field: Header("Weapon SFX")]
    [field: SerializeField]
    public EventReference DrawSword { get; private set; }

    [field: SerializeField]
    public EventReference SneathSword { get; private set; }
    [field: SerializeField]
    public EventReference SlashSword { get; private set; }
    public static FMODEvents instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one FMOD Events instance in the scene.");
        }
        instance = this;
    }

}
