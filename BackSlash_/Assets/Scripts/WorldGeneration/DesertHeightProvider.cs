using UnityEngine;

public class DesertHeightProvider
{
    readonly WorldGenSettings s;
    readonly Vector2 o1, o2, o3;

    public DesertHeightProvider(WorldGenSettings settings)
    {
        s = settings;
        o1 = Hash(settings.seed * 17 + 3);
        o2 = Hash(settings.seed * 31 + 7);
        o3 = Hash(settings.seed * 47 + 11);
    }

    public float SampleHeight(float x, float z)
    {
        Vector2 warp = new Vector2(
            FBM((x + o1.x) * s.warpFreq, (z + o1.y) * s.warpFreq),
            FBM((x + o2.x) * s.warpFreq, (z + o2.y) * s.warpFreq)
        );

        x += (warp.x * 2f - 1f) * s.warpAmp;
        z += (warp.y * 2f - 1f) * s.warpAmp;

        float dunes = Ridged((x + o2.x) * s.baseFreq, (z + o2.y) * s.baseFreq);
        float detail = FBM((x + o3.x) * s.baseFreq * 6f, (z + o3.y) * s.baseFreq * 6f);

        float h = dunes * s.duneAmp + detail * s.detailAmp;
        h *= (1f - s.flatness);
        h += s.terrainHeight * 0.02f;

        return h;
    }

    float FBM(float x, float z)
    {
        float v = 0f, a = 1f;
        for (int i = 0; i < 4; i++)
        {
            v += Mathf.PerlinNoise(x, z) * a;
            x *= 2f; z *= 2f; a *= 0.5f;
        }
        return v;
    }

    float Ridged(float x, float z)
    {
        float n = Mathf.PerlinNoise(x, z);
        return 1f - Mathf.Abs(n * 2f - 1f);
    }

    Vector2 Hash(int v)
    {
        uint h = (uint)v * 747796405u;
        return new Vector2((h % 10000) * 0.01f, ((h >> 16) % 10000) * 0.01f);
    }
}
