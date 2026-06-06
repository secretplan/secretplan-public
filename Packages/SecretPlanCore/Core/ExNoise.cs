namespace SecretPlanCore.Core;

public readonly struct ExNoise
{
    private readonly uint _seed;

    public ExNoise(int seed)
    {
        _seed = (uint)seed;
    }

    public uint UIntAt(int position)
    {
        return Squirrel5.Noise(position, _seed);
    }

    public byte ByteAt(int position)
    {
        return (byte)UIntAt(position);
    }

    public int PositiveIntAt(int position, int max = int.MaxValue)
    {
        return Math.Abs((int)UIntAt(position)) % max;
    }

    public int IntAt(int position, int max = int.MaxValue)
    {
        return Math.Abs((int)UIntAt(position)) % max;
    }

    public double DoubleAt(int position)
    {
        const int max = int.MaxValue / 2;
        return Math.Abs(PositiveIntAt(position, max)) / (double)max;
    }

    public float FloatAt(int position)
    {
        return (float)DoubleAt(position);
    }

    public bool BoolAt(int position)
    {
        return UIntAt(position) % 2 == 0;
    }

    public float RadianAt(int position)
    {
        return FloatAt(position) * MathF.PI * 2;
    }

    public ExNoise NoiseAt(int position)
    {
        return new ExNoise(PositiveIntAt(position));
    }

    public override string ToString()
    {
        return $"Seed = {_seed}";
    }

    public static int EpochTimeSeedMillis()
    {
        // wrap, then truncate
        return (int)(DateTimeOffset.Now.ToUnixTimeMilliseconds() % int.MaxValue);
    }

    /// <summary>
    /// Deterministic noise seed from a string
    /// </summary>
    public static int SeedFromString(string givenString)
    {
        int seed = 0;
        foreach (var character in givenString)
        {
            var noise = new ExNoise(seed);
            seed = noise.IntAt(character);
        }

        return seed;
    }
}