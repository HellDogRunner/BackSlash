using System.Collections.Generic;
using UnityEngine;

public class POISystem
{
    readonly WorldGenSettings _world;
    readonly POISettings _poi;
    readonly POIPool _pool;
    readonly TerrainStamper _stamper;

    // For each chunkCoord -> list of spawned instances (so we can despawn to pool)
    readonly Dictionary<Vector2Int, List<(POIDefinition def, GameObject go)>> _spawnedByChunk = new();

    // One root per chunk (so we can keep hierarchy tidy and reuse root objects)
    readonly Dictionary<Vector2Int, Transform> _rootsByChunk = new();

    public POISystem(WorldGenSettings worldSettings, POISettings poiSettings, POIPool pool, TerrainStamper stamper)
    {
        _world = worldSettings;
        _poi = poiSettings;
        _pool = pool;
        _stamper = stamper;
    }

    public void DespawnForChunk(Vector2Int chunkCoord)
    {
        // Return instances to pool
        if (_spawnedByChunk.TryGetValue(chunkCoord, out var list))
        {
            for (int i = 0; i < list.Count; i++)
            {
                var (def, go) = list[i];
                if (go != null && def != null)
                    _pool.Release(def.prefab, go);
            }
            list.Clear();
            _spawnedByChunk.Remove(chunkCoord);
        }

        // Cleanup root (optional). We keep it but disable it; next spawn will reuse it.
        if (_rootsByChunk.TryGetValue(chunkCoord, out var root) && root != null)
        {
            // remove any leftover children (safety)
            for (int i = root.childCount - 1; i >= 0; i--)
            {
                var child = root.GetChild(i).gameObject;
                child.SetActive(false);
                child.transform.SetParent(null);
            }
            root.gameObject.SetActive(false);
        }
    }

    public void DespawnAll()
    {
        var keys = new List<Vector2Int>(_spawnedByChunk.Keys);
        foreach (var k in keys)
            DespawnForChunk(k);
    }

    /// <summary>
    /// Spawns POIs that "belong" to this chunk. Deterministic by world seed + cell coords.
    /// POIs are generated per POI-cell, but only instantiated in the chunk that contains the final point.
    /// </summary>
    public void SpawnForChunk(Vector2Int chunkCoord, Terrain terrain, Transform parentForPoiRoot)
    {
        if (_poi == null || _poi.definitions == null || _poi.definitions.Length == 0) return;
        if (terrain == null || terrain.terrainData == null) return;

        // Do not double-spawn
        if (_spawnedByChunk.ContainsKey(chunkCoord)) return;

        // Create/reuse root
        Transform root = GetOrCreateRoot(chunkCoord, parentForPoiRoot);

        var list = new List<(POIDefinition def, GameObject go)>();
        _spawnedByChunk[chunkCoord] = list;

        float chunkSize = _world.chunkSize;
        Vector3 chunkOrigin = terrain.transform.position;

        // Chunk AABB in world
        float minX = chunkOrigin.x;
        float minZ = chunkOrigin.z;
        float maxX = chunkOrigin.x + chunkSize;
        float maxZ = chunkOrigin.z + chunkSize;

        float cellSize = _poi.poiCellSize;

        // Which POI-cells overlap this chunk
        int cellMinX = Mathf.FloorToInt(minX / cellSize);
        int cellMaxX = Mathf.FloorToInt((maxX - 0.0001f) / cellSize);
        int cellMinZ = Mathf.FloorToInt(minZ / cellSize);
        int cellMaxZ = Mathf.FloorToInt((maxZ - 0.0001f) / cellSize);

        for (int cz = cellMinZ; cz <= cellMaxZ; cz++)
            for (int cx = cellMinX; cx <= cellMaxX; cx++)
            {
                TrySpawnFromCell(cx, cz, chunkCoord, terrain, root, list);
            }

        // Hide empty roots (optional)
        if (list.Count == 0)
            root.gameObject.SetActive(false);
    }

    Transform GetOrCreateRoot(Vector2Int chunkCoord, Transform parent)
    {
        if (_rootsByChunk.TryGetValue(chunkCoord, out var root) && root != null)
        {
            root.gameObject.SetActive(true);
            root.SetParent(parent, false);
            root.gameObject.name = $"POI_{chunkCoord.x}_{chunkCoord.y}";
            return root;
        }

        var go = new GameObject($"POI_{chunkCoord.x}_{chunkCoord.y}");
        go.transform.SetParent(parent, false);
        root = go.transform;

        _rootsByChunk[chunkCoord] = root;
        return root;
    }

    void TrySpawnFromCell(
        int cellX,
        int cellZ,
        Vector2Int targetChunk,
        Terrain terrain,
        Transform parent,
        List<(POIDefinition def, GameObject go)> outList)
    {
        uint cellSeed = Hash(_world.seed, _poi.seedOffset, cellX, cellZ);
        var rng = new DeterministicRng(cellSeed);

        // Chance that this cell has any POI
        if (rng.Next01() > _poi.chancePerCell)
            return;

        var def = ChooseDefinition(rng);
        if (def == null || def.prefab == null)
            return;

        float cellSize = _poi.poiCellSize;
        float baseX = cellX * cellSize;
        float baseZ = cellZ * cellSize;

        for (int attempt = 0; attempt < _poi.attemptsPerCell; attempt++)
        {
            float px = baseX + rng.Next01() * cellSize;
            float pz = baseZ + rng.Next01() * cellSize;

            // Only spawn in chunk that owns this point
            var chunkOfPoint = WorldToChunk(new Vector3(px, 0f, pz));
            if (chunkOfPoint != targetChunk)
                continue;

            // Project to terrain and compute slope
            Vector3 worldPos = new Vector3(px, 0f, pz);
            if (!TryProjectToTerrain(terrain, worldPos, out worldPos, out float slopeDeg))
                continue;

            if (slopeDeg > def.maxSlopeDeg)
                continue;

            // Prevent seam artifacts: require that pad fits inside the chunk (MVP-safe)
            if (_stamper != null && !_stamper.HasEnoughMarginInsideChunk(terrain, worldPos, def))
                continue;

            // Create/modify pad BEFORE placing prefab (so prefab doesn't "sink" after)
            _stamper?.FlattenPad(terrain, worldPos, def);

            float yaw = def.randomYaw ? (rng.Next01() * 360f) : 0f;
            Quaternion rot = Quaternion.Euler(0f, yaw, 0f);

            var go = _pool.Acquire(def.prefab, parent);
            if (go == null) return;

            go.transform.SetPositionAndRotation(worldPos + Vector3.up * def.yOffset, rot);

            outList.Add((def, go));
            return;
        }
    }

    POIDefinition ChooseDefinition(DeterministicRng rng)
    {
        float total = 0f;
        for (int i = 0; i < _poi.definitions.Length; i++)
        {
            var d = _poi.definitions[i];
            if (d == null || d.prefab == null || d.weight <= 0f) continue;
            total += d.weight;
        }

        if (total <= 0f) return null;

        float pick = rng.Next01() * total;
        float acc = 0f;

        for (int i = 0; i < _poi.definitions.Length; i++)
        {
            var d = _poi.definitions[i];
            if (d == null || d.prefab == null || d.weight <= 0f) continue;

            acc += d.weight;
            if (pick <= acc) return d;
        }

        return null;
    }

    bool TryProjectToTerrain(Terrain terrain, Vector3 worldXZ, out Vector3 worldOnTerrain, out float slopeDeg)
    {
        var td = terrain.terrainData;
        Vector3 tPos = terrain.transform.position;
        Vector3 size = td.size;

        float tx = (worldXZ.x - tPos.x) / size.x;
        float tz = (worldXZ.z - tPos.z) / size.z;

        if (tx < 0f || tx > 1f || tz < 0f || tz > 1f)
        {
            worldOnTerrain = default;
            slopeDeg = 999f;
            return false;
        }

        float y = terrain.SampleHeight(worldXZ) + tPos.y;

        Vector3 n = td.GetInterpolatedNormal(tx, tz);
        slopeDeg = Vector3.Angle(n, Vector3.up);

        worldOnTerrain = new Vector3(worldXZ.x, y, worldXZ.z);
        return true;
    }

    Vector2Int WorldToChunk(Vector3 p)
    {
        float s = _world.chunkSize;
        return new Vector2Int(Mathf.FloorToInt(p.x / s), Mathf.FloorToInt(p.z / s));
    }

    static uint Hash(int worldSeed, int seedOffset, int a, int b)
    {
        unchecked
        {
            uint x = (uint)(worldSeed * 73856093) ^ (uint)(seedOffset * 19349663);
            x ^= (uint)(a * 83492791);
            x ^= (uint)(b * 2654435761u);

            x ^= x >> 16;
            x *= 2246822519u;
            x ^= x >> 13;
            x *= 3266489917u;
            x ^= x >> 16;

            return x == 0 ? 1u : x;
        }
    }
}
