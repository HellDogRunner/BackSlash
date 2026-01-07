using System.Collections;
using UnityEngine;

public class TerrainHeightGenerator
{
    readonly WorldGenSettings s;
    readonly DesertHeightProvider hp;

    public TerrainHeightGenerator(WorldGenSettings settings, DesertHeightProvider provider)
    {
        s = settings;
        hp = provider;
    }

    public void Apply(TerrainData td, Vector2Int c)
    {
        int r = td.heightmapResolution;
        var h = new float[r, r];

        float size = s.chunkSize;
        float ox = c.x * size;
        float oz = c.y * size;

        td.size = new Vector3(td.size.x, s.terrainHeight, td.size.z);

        for (int z = 0; z < r; z++)
        {
            float wz = oz + (float)z / (r - 1) * size;
            for (int x = 0; x < r; x++)
            {
                float wx = ox + (float)x / (r - 1) * size;
                h[z, x] = Mathf.Clamp01(hp.SampleHeight(wx, wz) / s.terrainHeight);
            }
        }

        td.SetHeights(0, 0, h);
    }

    public IEnumerator ApplyAsync(TerrainData td, Vector2Int c, int rowsPerFrame)
    {
        rowsPerFrame = Mathf.Max(1, rowsPerFrame);

        int r = td.heightmapResolution;
        var h = new float[r, r];

        float size = s.chunkSize;
        float ox = c.x * size;
        float oz = c.y * size;

        td.size = new Vector3(td.size.x, s.terrainHeight, td.size.z);

        for (int z = 0; z < r; z++)
        {
            float wz = oz + (float)z / (r - 1) * size;
            for (int x = 0; x < r; x++)
            {
                float wx = ox + (float)x / (r - 1) * size;
                h[z, x] = Mathf.Clamp01(hp.SampleHeight(wx, wz) / s.terrainHeight);
            }

            if ((z % rowsPerFrame) == 0)
                yield return null;
        }

        td.SetHeights(0, 0, h);
    }
}
