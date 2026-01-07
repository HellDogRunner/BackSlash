using UnityEngine;

public class TerrainStamper
{
    readonly WorldGenSettings _world;

    public TerrainStamper(WorldGenSettings world)
    {
        _world = world;
    }

    public void FlattenPad(Terrain terrain, Vector3 worldCenter, POIDefinition def)
    {
        if (terrain == null || def == null || !def.makePad) return;
        if (def.padRadius <= 0f || def.padStrength <= 0f) return;

        TerrainData td = terrain.terrainData;
        Vector3 tPos = terrain.transform.position;
        Vector3 size = td.size;

        float tx = (worldCenter.x - tPos.x) / size.x;
        float tz = (worldCenter.z - tPos.z) / size.z;

        if (tx <= 0f || tx >= 1f || tz <= 0f || tz >= 1f) return;

        int res = td.heightmapResolution;
        float metersPerSample = size.x / (res - 1);

        float outer = def.padRadius + def.padBlend;
        int outerSamples = Mathf.CeilToInt(outer / metersPerSample);
        int innerSamples = Mathf.CeilToInt(def.padRadius / metersPerSample);

        int cx = Mathf.RoundToInt(tx * (res - 1));
        int cz = Mathf.RoundToInt(tz * (res - 1));

        int x0 = Mathf.Clamp(cx - outerSamples, 0, res - 1);
        int z0 = Mathf.Clamp(cz - outerSamples, 0, res - 1);
        int x1 = Mathf.Clamp(cx + outerSamples, 0, res - 1);
        int z1 = Mathf.Clamp(cz + outerSamples, 0, res - 1);

        int w = x1 - x0 + 1;
        int h = z1 - z0 + 1;

        // Target height = height at center
        float[,] center = td.GetHeights(cx, cz, 1, 1);
        float target = center[0, 0];

        float[,] heights = td.GetHeights(x0, z0, w, h);

        for (int z = 0; z < h; z++)
        {
            int hz = z0 + z;
            int dz = hz - cz;

            for (int x = 0; x < w; x++)
            {
                int hx = x0 + x;
                int dx = hx - cx;

                float distSamples = Mathf.Sqrt(dx * dx + dz * dz);

                if (distSamples <= innerSamples)
                {
                    heights[z, x] = Mathf.Lerp(heights[z, x], target, def.padStrength);
                }
                else if (distSamples <= outerSamples && def.padBlend > 0f)
                {
                    float t = Mathf.InverseLerp(outerSamples, innerSamples, distSamples);
                    t = t * t * (3f - 2f * t); // smoothstep
                    heights[z, x] = Mathf.Lerp(heights[z, x], target, t * def.padStrength);
                }
            }
        }

        td.SetHeights(x0, z0, heights);
    }

    /// <summary>
    /// Чтобы не портить швы чанков, проверяем, что площадка целиком внутри чанка
    /// </summary>
    public bool HasEnoughMarginInsideChunk(Terrain terrain, Vector3 worldPos, POIDefinition def)
    {
        if (terrain == null || def == null || !def.makePad) return true;

        float margin = def.padRadius + def.padBlend + 1f;
        Vector3 tPos = terrain.transform.position;
        float size = terrain.terrainData.size.x;

        return (worldPos.x - tPos.x) >= margin
            && (worldPos.z - tPos.z) >= margin
            && (tPos.x + size - worldPos.x) >= margin
            && (tPos.z + size - worldPos.z) >= margin;
    }
}
