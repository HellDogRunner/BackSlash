using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainChunkStreamer : MonoBehaviour
{
    [Header("Async Startup")]
    public bool asyncStartup = true;
    public int chunksPerFrameStartup = 1;
    public int rowsPerFrameHeights = 8;

    [Tooltip("Optional: disable these behaviours while initial world is generating")]
    public Behaviour[] disableWhileLoading;

    [Header("References")]
    public Transform player;

    [Header("Settings")]
    public WorldGenSettings settings;

    [Header("POI")]
    public POISettings poiSettings;

    [Header("Junk")]
    public JunkSettings junkSettings;

    JunkPool junkPool;
    JunkSystem junkSystem;
    Transform junkPoolRoot;


    readonly Dictionary<Vector2Int, TerrainChunk> active = new();

    TerrainChunkPool pool;
    TerrainHeightGenerator heightGen;
    TerrainTexturePainter painter;
    DesertHeightProvider provider;
    TerrainStamper stamper;

    // POI services
    POIPool poiPool;
    POISystem poiSystem;
    Transform poiPoolRoot;

    Vector2Int current;

    bool _isInitializing;

    void OnEnable()
    {
        if (settings != null) settings.Changed += OnSettingsChanged;
    }

    void OnDisable()
    {
        if (settings != null) settings.Changed -= OnSettingsChanged;
    }

    void Start()
    {
        if (player == null || settings == null)
        {
            Debug.LogError("TerrainChunkStreamer: Assign player and settings.");
            enabled = false;
            return;
        }

        pool = new TerrainChunkPool(transform);

        poiPoolRoot = new GameObject("POI_PoolRoot").transform;
        poiPoolRoot.SetParent(transform, false);
        poiPool = new POIPool(poiPoolRoot);

        junkPoolRoot = new GameObject("JUNK_PoolRoot").transform;
        junkPoolRoot.SetParent(transform, false);
        junkPool = new JunkPool(junkPoolRoot);


        RebuildServices();

        if (asyncStartup)
            StartCoroutine(InitializeWorldCoroutine());
        else
            UpdateVisible(true);
    }

    void Update()
    {
        if (_isInitializing) return;
        UpdateVisible(false);
    }

    IEnumerator InitializeWorldCoroutine()
    {
        _isInitializing = true;
        SetLoadingState(true);

        var center = WorldToChunk(player.position);
        current = center;

        int r = settings.viewRadius;

        // Order chunks by rings: center -> ring1 -> ring2 ...
        var ordered = new List<Vector2Int> { center };
        for (int ring = 1; ring <= r; ring++)
        {
            for (int dz = -ring; dz <= ring; dz++)
                for (int dx = -ring; dx <= ring; dx++)
                {
                    if (Mathf.Abs(dx) != ring && Mathf.Abs(dz) != ring) continue;
                    ordered.Add(new Vector2Int(center.x + dx, center.y + dz));
                }
        }

        int perFrame = Mathf.Max(1, chunksPerFrameStartup);
        int countThisFrame = 0;

        foreach (var cc in ordered)
        {
            if (!active.ContainsKey(cc))
                yield return CreateChunkAsync(cc);

            countThisFrame++;
            if (countThisFrame >= perFrame)
            {
                countThisFrame = 0;
                yield return null;
            }
        }

        UpdateNeighbors();

        SetLoadingState(false);
        _isInitializing = false;
    }

    IEnumerator CreateChunkAsync(Vector2Int cc)
    {
        var t = pool.Acquire(settings);
        var chunk = new TerrainChunk(t);
        chunk.SetCoord(cc, settings.chunkSize);

        painter.Apply(chunk.Data);

        // async heights
        yield return heightGen.ApplyAsync(chunk.Data, cc, rowsPerFrameHeights);

        poiSystem?.SpawnForChunk(cc, chunk.Terrain, chunk.Terrain.transform);
        junkSystem?.SpawnForChunk(cc, chunk.Terrain, chunk.Terrain.transform);

        active[cc] = chunk;
    }

    void SetLoadingState(bool isLoading)
    {
        if (disableWhileLoading == null) return;
        for (int i = 0; i < disableWhileLoading.Length; i++)
            if (disableWhileLoading[i] != null)
                disableWhileLoading[i].enabled = !isLoading;
    }

    void RebuildServices()
    {
        provider = new DesertHeightProvider(settings);
        heightGen = new TerrainHeightGenerator(settings, provider);
        painter = new TerrainTexturePainter(settings);
        stamper = new TerrainStamper(settings);

        if (poiSettings != null)
            poiSystem = new POISystem(settings, poiSettings, poiPool, stamper);
        else
            poiSystem = null;

        if (junkSettings != null)
            junkSystem = new JunkSystem(settings, junkSettings, junkPool);
        else
            junkSystem = null;

    }

    void UpdateVisible(bool force)
    {
        var c = WorldToChunk(player.position);
        if (!force && c == current) return;
        current = c;

        int r = settings.viewRadius;

        // Spawn missing chunks (sync path for runtime streaming)
        for (int dz = -r; dz <= r; dz++)
            for (int dx = -r; dx <= r; dx++)
            {
                var cc = new Vector2Int(c.x + dx, c.y + dz);
                if (active.ContainsKey(cc)) continue;

                var t = pool.Acquire(settings);
                var chunk = new TerrainChunk(t);
                chunk.SetCoord(cc, settings.chunkSize);

                painter.Apply(chunk.Data);
                heightGen.Apply(chunk.Data, cc);

                poiSystem?.SpawnForChunk(cc, chunk.Terrain, chunk.Terrain.transform);
                junkSystem?.SpawnForChunk(cc, chunk.Terrain, chunk.Terrain.transform);


                active[cc] = chunk;
            }

        // Release far chunks
        var remove = new List<Vector2Int>();
        foreach (var kv in active)
        {
            var cc = kv.Key;
            if (Mathf.Abs(cc.x - c.x) > r || Mathf.Abs(cc.y - c.y) > r)
                remove.Add(cc);
        }

        foreach (var cc in remove)
        {
            poiSystem?.DespawnForChunk(cc);
            junkSystem?.DespawnForChunk(cc);
            pool.Release(active[cc].Terrain);
            active.Remove(cc);
        }

        UpdateNeighbors();
    }

    void UpdateNeighbors()
    {
        foreach (var kv in active)
        {
            var c = kv.Key;
            var t = kv.Value.Terrain;

            active.TryGetValue(new Vector2Int(c.x - 1, c.y), out var l);
            active.TryGetValue(new Vector2Int(c.x + 1, c.y), out var r);
            active.TryGetValue(new Vector2Int(c.x, c.y + 1), out var top);
            active.TryGetValue(new Vector2Int(c.x, c.y - 1), out var b);

            t.SetNeighbors(l?.Terrain, top?.Terrain, r?.Terrain, b?.Terrain);
        }
    }

    void OnSettingsChanged()
    {
        if (!Application.isPlaying) return;

        RebuildServices();

        // Rebuild POIs because they depend on terrain shape
        poiSystem?.DespawnAll();
        junkSystem?.DespawnAll();
        foreach (var kv in active)
        {
            painter.Apply(kv.Value.Data);
            heightGen.Apply(kv.Value.Data, kv.Key);
            kv.Value.Terrain.Flush();

            poiSystem?.SpawnForChunk(kv.Key, kv.Value.Terrain, kv.Value.Terrain.transform);
            junkSystem?.SpawnForChunk(kv.Key, kv.Value.Terrain, kv.Value.Terrain.transform);
        }
    }

    Vector2Int WorldToChunk(Vector3 p)
    {
        float s = settings.chunkSize;
        return new Vector2Int(Mathf.FloorToInt(p.x / s), Mathf.FloorToInt(p.z / s));
    }
}
