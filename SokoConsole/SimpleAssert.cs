using System.Runtime.CompilerServices;

namespace SokoConsole2;

public static class SimpleAssert
{
    public static void ShouldBe<T>(T? actual, T? expected, [CallerArgumentExpression("actual")] string actualName = "")
    {
        if (actual == null)
        {
            if (expected == null)
            {
                return;
            }

            throw new AssertFailedException(actualName, actual, expected);
        }

        if (!actual.Equals(expected))
        {
            throw new AssertFailedException(actualName, actual, expected);
        }
    }

    public class AssertFailedException : Exception
    {
        public AssertFailedException(string actualName, object? actual, object? expected) : base(
            $"{actualName}: got {actual ?? "null"} when expected {expected ?? "null"}")
        {
        }
    }
}