using System.Globalization;

namespace SecretPlanCore.Core;

public struct ClampedInt
{
    private int _value = 0;
    private readonly int _min = 0;
    private readonly int _max = 1;

    public ClampedInt(int value, int min, int max)
    {
        _value = value;
        _min = min;
        _max = max;
    }

    public ClampedInt(int value, int max)
    {
        _min = 0;
        _max = max;
        _value = value;
    }

    public static implicit operator int(ClampedInt clampedFloat)
    {
        return clampedFloat._value;
    }

    public override string ToString()
    {
        return Value.ToString(CultureInfo.InvariantCulture);
    }

    public static ClampedInt operator +(ClampedInt left, int right)
    {
        return left with { Value = left.Value + right };
    }

    public static ClampedInt operator -(ClampedInt left, int right)
    {
        return left with { Value = left.Value - right };
    }

    public int Value
    {
        get => _value;
        set => _value = Math.Clamp(value, _min, _max);
    }
}