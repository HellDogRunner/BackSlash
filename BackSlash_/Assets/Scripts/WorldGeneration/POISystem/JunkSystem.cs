using System.Collections.Generic;
using UnityEngine;

public class JunkSystem
{
    readonly WorldGenSettings _world;
    readonly JunkSettings _junk;
    readonly JunkPool _pool;

    readonly Dictionary<Vector2Int, List<(JunkDefinition def, GameObject go)>> _spawnedByChunk = new();
    readonly Dictionary<Vector2Int, Transform> _rootsByChunk = new();

    public JunkSystem(WorldGenSettings worldSettings, JunkSettings junkSettings, JunkPool pool)
    {
        _world = worldSettings;
        _junk = junkSettings;
        _pool = pool;
    }

    public void DespawnForChunk(Vector2Int chunkCoord)
    {
        if (_spawnedByChunk.TryGetValue(chunkCoord, out var list))
        {
            for (int i = 0; i < list.Count; i++)
            {
                var (def, go) = list[i];
                if (def != null && go != null)
                    _pool.Release(def.prefab, go);
            }
            list.Clear();
            _spawnedByChunk.Remove(chunkCoord);
        }

        if (_rootsByChunk.TryGetValue(chunkCoord, out var root) && root != null)
            root.gameObject.SetActive(false);
    }

    public void DespawnAll()
    {
        var keys = new List<Vector2Int>(_spawnedByChunk.Keys);
        foreach (var k in keys) DespawnForChunk(k);
    }

    public void SpawnForChunk(Vector2Int chunkCoord, Terrain terrain, Transform parentForRoot)
    {
        if (_junk == null || _junk.definitions == null || _junk.definitions.Length == 0) return;
        if (terrain == null || terrain.terrainData == null) return;
        if (_pool == null) return;

        if (_spawnedByChunk.ContainsKey(chunkCoord)) return;

        Transform root = GetOrCreateRoot(chunkCoord, parentForRoot);
        root.gameObject.SetActive(true);

        var list = new List<(JunkDefinition def, GameObject go)>();
        _spawnedByChunk[chunkCoord] = list;

        float chunkSize = _world.chunkSize;
        Vector3 chunkOrigin = terrain.transform.position;

        float minX = chunkOrigin.x;
        float minZ = chunkOrigin.z;
        float maxX = chunkOrigin.x + chunkSize;
        float maxZ = chunkOrigin.z + chunkSize;

        float cellSize = _junk.junkCellSize;

        int cellMinX = Mathf.FloorToInt(minX / cellSize);
        int cellMaxX = Mathf.FloorToInt((maxX - 0.0001f) / cellSize);
        int cellMinZ = Mathf.FloorToInt(minZ / cellSize);
        int cellMaxZ = Mathf.FloorToInt((maxZ - 0.0001f) / cellSize);

        for (int cz = cellMinZ; cz <= cellMaxZ; cz++)
            for (int cx = cellMinX; cx <= cellMaxX; cx++)
                TrySpawnFromCell(cx, cz, chunkCoord, terrain, root, list);

        if (list.Count == 0)
            root.gameObject.SetActive(false);
    }

    Transform GetOrCreateRoot(Vector2Int chunkCoord, Transform parent)
    {
        if (_rootsByChunk.TryGetValue(chunkCoord, out var root) && root != null)
        {
            root.SetParent(parent, false);
            root.gameObject.name = $"JUNK_{chunkCoord.x}_{chunkCoord.y}";
            return root;
        }

        var go = new GameObject($"JUNK_{chunkCoord.x}_{chunkCoord.y}");
        go.transform.SetParent(parent, false);
        root = go.transform;

        _rootsByChunk[chunkCoord] = root;
        return root;
    }

    void TrySpawnFromCell(int cellX, int cellZ, Vector2Int targetChunk, Terrain terrain, Transform parent, List<(JunkDefinition def, GameObject go)> outList)
    {
        uint seed = Hash(_world.seed, _junk.seedOffset, cellX, cellZ);
        var rng = new DeterministicRng(seed);

        if (rng.Next01() > _junk.chancePerCell)
            return;

        int count = rng.NextInt(_junk.countPerCell.x, _junk.countPerCell.y + 1);
        if (count <= 0) return;

        float cellSize = _junk.junkCellSize;
        float baseX = cellX * cellSize;
        float baseZ = cellZ * cellSize;

        for (int i = 0; i < count; i++)
        {
            var def = ChooseDefinition(rng);
            if (def == null || def.prefab == null || def.weight <= 0f) continue;

            for (int attempt = 0; attempt < _junk.attemptsPerObject; attempt++)
            {
                float px = baseX + rng.Next01() * cellSize;
                float pz = baseZ + rng.Next01() * cellSize;

                // spawn only in owning chunk
                var chunkOfPoint = WorldToChunk(new Vector3(px, 0f, pz));
                if (chunkOfPoint != targetChunk) continue;

                Vector3 worldPos = new Vector3(px, 0f, pz);
                if (!TryProjectToTerrain(terrain, worldPos, out worldPos, out float slopeDeg, out Vector3 normal))
                    continue;

                if (slopeDeg > def.maxSlopeDeg)
                    continue;

                float yaw = def.randomYaw ? (rng.Next01() * 360f) : 0f;
                float tiltX = (rng.Next01() * 2f - 1f) * def.maxTiltDeg;
                float tiltZ = (rng.Next01() * 2f - 1f) * def.maxTiltDeg;

                // Align a bit with normal, then add chaos tilt
                Quaternion align = Quaternion.FromToRotation(Vector3.up, normal);
                Quaternion rot = align * Quaternion.Euler(tiltX, yaw, tiltZ);

                float bury = def.buryMax > 0f ? rng.Next01() * def.buryMax : 0f;
                Vector3 pos = worldPos + Vector3.up * (def.yOffset - bury);

                var go = _pool.Acquire(def.prefab, parent);
                if (go == null) break;

                go.transform.SetPositionAndRotation(pos, rot);
                outList.Add((def, go));
                break;
            }
        }
    }

    JunkDefinition ChooseDefinition(DeterministicRng rng)
    {
        float total = 0f;
        for (int i = 0; i < _junk.definitions.Length; i++)
        {
            var d = _junk.definitions[i];
            if (d == null || d.prefab == null || d.weight <= 0f) continue;
            total += d.weight;
        }
        if (total <= 0f) return null;

        float pick = rng.Next01() * total;
        float acc = 0f;

        for (int i = 0; i < _junk.definitions.Length; i++)
        {
            var d = _junk.definitions[i];
            if (d == null || d.prefab == null || d.weight <= 0f) continue;

            acc += d.weight;
            if (pick <= acc) return d;
        }
        return null;
    }

    bool TryProjectToTerrain(Terrain terrain, Vector3 worldXZ, out Vector3 worldOnTerrain, out float slopeDeg, out Vector3 normal)
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
            normal = Vector3.up;
            return false;
        }

        float y = terrain.SampleHeight(worldXZ) + tPos.y;

        normal = td.GetInterpolatedNormal(tx, tz);
        slopeDeg = Vector3.Angle(normal, Vector3.up);

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
