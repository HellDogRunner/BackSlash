using UnityEngine;

[CreateAssetMenu(menuName = "World/POI Settings", fileName = "POISettings")]
public class POISettings : ScriptableObject
{
    [Header("Grid")]
    [Min(64f)] public float poiCellSize = 512f;

    [Header("Spawn Rate")]
    [Range(0f, 1f)] public float chancePerCell = 0.15f; // шанс POI в €чейке

    [Header("Placement Attempts")]
    [Range(1, 10)] public int attemptsPerCell = 3; // пробуем несколько точек в €чейке

    [Header("Seed")]
    public int seedOffset = 9001;

    [Header("POI Types")]
    public POIDefinition[] definitions;
}
