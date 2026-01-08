using UnityEngine;

[CreateAssetMenu(menuName = "World/Junk Settings", fileName = "JunkSettings")]
public class JunkSettings : ScriptableObject
{
    [Header("Grid")]
    [Min(16f)] public float junkCellSize = 96f;

    [Header("Density")]
    [Range(0f, 1f)] public float chancePerCell = 0.75f;

    [Tooltip("How many junk objects per cell (if cell is active).")]
    public Vector2Int countPerCell = new Vector2Int(1, 4);

    [Header("Placement Attempts")]
    [Range(1, 10)] public int attemptsPerObject = 3;

    [Header("Seed")]
    public int seedOffset = 1337;

    [Header("Junk Types")]
    public JunkDefinition[] definitions;
}
