using UnityEngine;

[CreateAssetMenu(menuName = "World/POI Definition", fileName = "POI_")]
public class POIDefinition : ScriptableObject
{
    public GameObject prefab;
    [Min(0f)] public float weight = 1f;

    [Header("Placement")]
    [Range(0f, 60f)] public float maxSlopeDeg = 12f;
    public bool randomYaw = true;
    public float yOffset = 0f;

    [Header("Terrain Pad (Flatten)")]
    public bool makePad = true;

    [Tooltip("Radius in meters of the flat area.")]
    [Min(0f)] public float padRadius = 6f;

    [Tooltip("Extra blending ring around radius, in meters.")]
    [Min(0f)] public float padBlend = 4f;

    [Tooltip("0..1 how strongly we pull terrain to target height.")]
    [Range(0f, 1f)] public float padStrength = 1f;
}
