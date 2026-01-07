public class DeterministicRng
{
    uint _state;

    public DeterministicRng(uint seed)
    {
        _state = seed == 0 ? 1u : seed;
    }

    uint NextU()
    {
        // xorshift32
        uint x = _state;
        x ^= x << 13;
        x ^= x >> 17;
        x ^= x << 5;
        _state = x;
        return x;
    }

    public float Next01()
    {
        // [0,1)
        return (NextU() & 0x00FFFFFF) / 16777216f;
    }

    public int NextInt(int minInclusive, int maxExclusive)
    {
        if (maxExclusive <= minInclusive) return minInclusive;
        uint r = NextU();
        uint range = (uint)(maxExclusive - minInclusive);
        return (int)(r % range) + minInclusive;
    }
}
