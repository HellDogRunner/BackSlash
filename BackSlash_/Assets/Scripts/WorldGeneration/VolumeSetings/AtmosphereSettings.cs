using System;
using UnityEngine;

[CreateAssetMenu(menuName = "World/Atmosphere Settings", fileName = "AtmosphereSettings")]
public class AtmosphereSettings : ScriptableObject
{
    [Header("Fog (RenderSettings)")]
    public bool fogEnabled = true;
    public Color fogColor = new Color(0.08f, 0.09f, 0.13f, 1f);
    [Range(0f, 0.05f)] public float fogDensity = 0.008f;

    [Header("Directional Light")]
    public Color sunColor = new Color(0.73f, 0.78f, 1.0f, 1f);
    [Range(0f, 2f)] public float sunIntensity = 0.85f;

    [Header("Ambient")]
    public Color ambientColor = new Color(0.04f, 0.06f, 0.08f, 1f);

    public event Action Changed;

#if UNITY_EDITOR
    void OnValidate() => Changed?.Invoke();
#endif
}
