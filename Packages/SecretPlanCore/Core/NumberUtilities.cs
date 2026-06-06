using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace SecretPlanCore.Core;

public static class NumberUtilities
{
    public static bool TryParseNumeric(string input, Type type, [NotNullWhen(true)] out object? value)
    {
        value = null;

        var targetType = Nullable.GetUnderlyingType(type) ?? type;

        if (!decimal.TryParse(
                input,
                NumberStyles.Number,
                CultureInfo.InvariantCulture,
                out var parsed))
        {
            return false;
        }

        try
        {
            value = ConvertToTarget(parsed, targetType);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public static bool TypeIsNumber(Type targetType)
    {
        return targetType == typeof(int) ||
               targetType == typeof(long) ||
               targetType == typeof(short) ||
               targetType == typeof(byte) ||
               targetType == typeof(uint) ||
               targetType == typeof(ulong) ||
               targetType == typeof(float) || 
               targetType == typeof(double) ||
               targetType == typeof(decimal);
    }

    private static object ConvertToTarget(decimal value, Type targetType)
    {
        if (targetType == typeof(int))
        {
            return (int)value;
        }

        if (targetType == typeof(long))
        {
            return (long)value;
        }

        if (targetType == typeof(short))
        {
            return (short)value;
        }

        if (targetType == typeof(byte))
        {
            return (byte)value;
        }

        if (targetType == typeof(uint))
        {
            return (uint)value;
        }

        if (targetType == typeof(ulong))
        {
            return (ulong)value;
        }

        if (targetType == typeof(float))
        {
            return (float)value;
        }

        if (targetType == typeof(double))
        {
            return (double)value;
        }

        if (targetType == typeof(decimal))
        {
            return value;
        }

        throw new NotSupportedException($"Unsupported numeric type: {targetType}");
    }
}