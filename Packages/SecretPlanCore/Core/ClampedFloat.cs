using System.Globalization;

namespace SecretPlanCore.Core;

public struct ClampedFloat
{
    private float _value = 0f;
    private readonly float _min = 0f;
    private readonly float _max = 1f;

    public ClampedFloat(float value, float min, float max)
    {
        _value = value;
    }

    public ClampedFloat(float value, float max)
    {
        _min = 0;
        _max = max;
        _value = value;
    }

    public ClampedFloat()
    {
        _min = 0f;
        _value = 0f;
        _max = 1f;
    }

    public static implicit operator float(ClampedFloat clampedFloat)
    {
        return clampedFloat._value;
    }

    public override string ToString()
    {
        return Value.ToString(CultureInfo.InvariantCulture);
    }

    public static ClampedFloat operator +(ClampedFloat left, float right)
    {
        return left with { Value = left.Value + right };
    }

    public static ClampedFloat operator -(ClampedFloat left, float right)
    {
        return left with { Value = left.Value - right };
    }

    public float Value
    {
        get => _value;
        set => _value = Math.Clamp(value, _min, _max);
    }
}