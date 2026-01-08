using UnityEngine;

[CreateAssetMenu(menuName = "World/Junk Definition", fileName = "Junk_")]
public class JunkDefinition : ScriptableObject
{
    public GameObject prefab;
    [Min(0f)] public float weight = 1f;

    [Header("Placement")]
    [Range(0f, 60f)] public float maxSlopeDeg = 30f;

    public bool randomYaw = true;

    [Tooltip("Random tilt around X/Z in degrees (visual chaos).")]
    [Range(0f, 30f)] public float maxTiltDeg = 8f;

    [Tooltip("Push into ground (meters).")]
    [Min(0f)] public float buryMax = 0.25f;

    public float yOffset = 0f;
}
