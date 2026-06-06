namespace SecretPlanCore.Core;

public class NoiseBasedRng
{
    private readonly ExNoise _noise;
    private int _position;

    public NoiseBasedRng(int seed) : this(new ExNoise(seed))
    {
    }

    public NoiseBasedRng(ExNoise noise)
    {
        _noise = noise;
    }

    public uint NextUInt()
    {
        return _noise.UIntAt(_position++);
    }

    public byte NextByte()
    {
        return (byte)NextUInt();
    }

    public int NextPositiveInt(int max = int.MaxValue)
    {
        if (max == 0)
        {
            return 0;
        }
        return Math.Abs((int)NextUInt()) % max;
    }

    public int NextInt()
    {
        return (int)NextUInt();
    }

    public double NextDouble()
    {
        const int max = int.MaxValue / 2;
        return Math.Abs(NextPositiveInt(max)) / (double)max;
    }

    public float NextFloat()
    {
        return (float)NextDouble();
    }

    public bool NextBool()
    {
        return NextUInt() % 2 == 0;
    }

    public float NextRadian()
    {
        return NextFloat() * MathF.PI * 2;
    }

    public ExNoise NextNoise()
    {
        return new ExNoise(NextPositiveInt());
    }

    public void Shuffle<T>(IList<T> list)
    {
        var n = list.Count;
        while (n > 1)
        {
            n--;
            var k = NextPositiveInt(n + 1);
            (list[k], list[n]) = (list[n], list[k]);
        }
    }

    public T GetRandomElement<T>(IList<T> list)
    {
        return list[NextPositiveInt(list.Count)];
    }

    public T GetRandomElement<T>(IEnumerable<T> enumerable)
    {
        return GetRandomElement(enumerable.ToList());
    }

    public T? GetRandomElementOrDefault<T>(IList<T> list)
    {
        if (list.Count == 0)
        {
            return default;
        }

        return list[NextPositiveInt(list.Count)];
    }

    public T? GetRandomElementOrDefault<T>(IEnumerable<T> enumerable)
    {
        var list = enumerable.ToList();
        if (list.Count == 0)
        {
            return default;
        }
        return GetRandomElement(list);
    }

    public int NextSign()
    {
        return NextBool() ? 1 : -1;
    }

    public int NextInt(int low, int high)
    {
        var relativeRange = high - low;
        var mod = relativeRange;
        if (mod == 0)
        {
            return low;
        }

        return NextPositiveInt() % mod + low;
    }

    public override string ToString()
    {
        return $"{_noise}, Position {_position}";
    }

    public float NextFloat(float min, float max)
    {
        var range = max - min;
        var normal = NextFloat();
        return min + normal * range;
    }

    public T NextEnum<T>() where T : struct, Enum
    {
        return GetRandomElement(Enum.GetValues<T>());
    }
}