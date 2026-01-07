using System;
using UnityEngine;

[CreateAssetMenu(menuName = "World/World Gen Settings", fileName = "WorldGenSettings")]
public class WorldGenSettings : ScriptableObject
{
    [Header("Seed")]
    public int seed = 12345;

    [Header("Chunk")]
    [Min(32f)] public float chunkSize = 256f;
    [Min(1f)] public float terrainHeight = 200f;

    [Tooltip("Must be 2^n + 1")]
    public int heightmapResolution = 257;

    public int alphamapResolution = 256;

    [Header("View")]
    [Range(1, 6)] public int viewRadius = 2;

    [Header("Terrain Visuals")]
    public float pixelError = 5f;
    public float basemapDistance = 1000f;

    [Header("URP Terrain Layer")]
    public TerrainLayer sandLayer;

    [Header("Desert Dunes")]
    public float baseFreq = 0.0025f;
    public float warpFreq = 0.0015f;
    public float warpAmp = 35f;
    public float duneAmp = 90f;
    public float detailAmp = 20f;
    [Range(0f, 0.95f)] public float flatness = 0.0f;

    public event Action Changed;

#if UNITY_EDITOR
    private void OnValidate()
    {
        Changed?.Invoke();
    }
#endif
}
