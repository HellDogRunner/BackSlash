using UnityEngine;

public class TerrainTexturePainter
{
    readonly WorldGenSettings s;

    public TerrainTexturePainter(WorldGenSettings settings)
    {
        s = settings;
    }

    public void Apply(TerrainData td)
    {
        if (s.sandLayer != null)
            td.terrainLayers = new[] { s.sandLayer };
    }
}
