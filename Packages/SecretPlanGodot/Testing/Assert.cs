using SecretPlanGodot.Testing;

namespace BirdGameTests;

public class Assert
{
    public static void IsTrue(bool value, string? message = null)
    {
        EqualViaOperator(true, value, message);
    }

    public static void IsFalse(bool value, string? message = null)
    {
        EqualViaOperator(false, value, message);
    }

    public static void EqualViaOperator(bool expected, bool actual, string? message = null)
    {
        if (expected != actual)
        {
            throw new AssertionFailedException(message ?? $"{expected} != {actual}");
        }
    }
}