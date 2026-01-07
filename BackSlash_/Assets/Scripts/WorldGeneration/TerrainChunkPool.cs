using System.Collections.Generic;
using UnityEngine;

public class TerrainChunkPool
{
    readonly Stack<Terrain> pool = new();
    readonly Transform parent;

    public TerrainChunkPool(Transform parent)
    {
        this.parent = parent;
    }

    public Terrain Acquire(WorldGenSettings s)
    {
        if (pool.Count > 0)
        {
            var t = pool.Pop();
            t.gameObject.SetActive(true);
            return t;
        }

        var td = new TerrainData
        {
            heightmapResolution = s.heightmapResolution,
            alphamapResolution = s.alphamapResolution,
            size = new Vector3(s.chunkSize, s.terrainHeight, s.chunkSize),
            terrainLayers = s.sandLayer ? new[] { s.sandLayer } : null
        };

        var go = Terrain.CreateTerrainGameObject(td);
        go.transform.SetParent(parent, false);

        return go.GetComponent<Terrain>();
    }

    public void Release(Terrain t)
    {
        t.SetNeighbors(null, null, null, null);
        t.gameObject.SetActive(false);
        pool.Push(t);
    }

    public void Clear()
    {
        while (pool.Count > 0)
            Object.Destroy(pool.Pop().gameObject);
    }
}
