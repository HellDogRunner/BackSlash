using UnityEngine;

public class AtmosphereController : MonoBehaviour
{
    public AtmosphereSettings settings;
    public Light directionalLight;

    public bool applyOnStart = true;
    public bool liveUpdateInPlayMode = true;

    void OnEnable()
    {
        if (settings != null) settings.Changed += OnSettingsChanged;
    }

    void OnDisable()
    {
        if (settings != null) settings.Changed -= OnSettingsChanged;
    }

    void Start()
    {
        if (applyOnStart) Apply();
    }

    void OnSettingsChanged()
    {
        if (!liveUpdateInPlayMode) return;
        if (!Application.isPlaying) return;
        Apply();
    }

    public void Apply()
    {
        if (settings == null) return;

        // Fog
        RenderSettings.fog = settings.fogEnabled;
        RenderSettings.fogMode = FogMode.Exponential;
        RenderSettings.fogColor = settings.fogColor;
        RenderSettings.fogDensity = settings.fogDensity;

        // Ambient
        RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
        RenderSettings.ambientLight = settings.ambientColor;

        // Directional
        if (directionalLight != null)
        {
            directionalLight.color = settings.sunColor;
            directionalLight.intensity = settings.sunIntensity;
        }
    }
}
