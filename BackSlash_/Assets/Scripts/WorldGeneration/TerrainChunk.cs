using UnityEngine;

public class TerrainChunk
{
    public Vector2Int Coord { get; private set; }
    public Terrain Terrain { get; }
    public TerrainData Data => Terrain.terrainData;

    public TerrainChunk(Terrain terrain)
    {
        Terrain = terrain;
    }

    public void SetCoord(Vector2Int coord, float size)
    {
        Coord = coord;
        Terrain.transform.position = new Vector3(coord.x * size, 0f, coord.y * size);
        Terrain.gameObject.name = $"Terrain_{coord.x}_{coord.y}";
    }
}
