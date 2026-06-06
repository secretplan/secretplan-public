using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SokoConsole2;

public static class SimpleAssert
{
    public static void ShouldBe<T>(T? actual, T? expected, [CallerArgumentExpression(nameof(actual))] string actualName = "", [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "")
    {
        var caller = $"{callerFilePath}:{callerLineNumber} {actualName}";
        
        if (actual == null)
        {
            if (expected == null)
            {
                return;
            }

            throw new AssertFailedException(caller, actual, expected);
        }

        if (!actual.Equals(expected))
        {
            throw new AssertFailedException(caller, actual, expected);
        }
    }

    public class AssertFailedException : Exception
    {
        public AssertFailedException(string caller, object? actual, object? expected) : base(
            $"{caller} got {actual ?? "null"} when expected {expected ?? "null"}")
        {
        }
    }
}