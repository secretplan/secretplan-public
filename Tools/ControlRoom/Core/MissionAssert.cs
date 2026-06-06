using System.Reflection;
using System.Text;
using ControlRoom.Missions;
using SecretPlanCore.ArgumentParsing;

namespace ControlRoom.Core;

public static class MissionAssert
{
    public static void AreEqual(object actual, object expected, string? message = null)
    {
        if (!expected.Equals(actual))
        {
            var finalMessage = new StringBuilder();
            finalMessage.AppendLine("Assertion failed!");
            finalMessage.AppendLine(
                $"Expected: `{expected}`\n" +
                $"Actual:   `{actual}`"
            );

            if (!string.IsNullOrWhiteSpace(message))
            {
                finalMessage.AppendLine(message);
            }

            throw new MissionFailedException(finalMessage.ToString());
        }
    }

    public static void IsNotNull([System.Diagnostics.CodeAnalysis.NotNull] object? shouldNotBeNull, string message)
    {
        if (shouldNotBeNull != null)
        {
            return;
        }

        var finalMessage = new StringBuilder();
        finalMessage.AppendLine("Assertion failed!");
        finalMessage.AppendLine(message);
        throw new MissionFailedException(finalMessage.ToString());
    }

    public static void IsTrue(bool shouldBeTrue, string message)
    {
        if (shouldBeTrue)
        {
            return;
        }

        var finalMessage = new StringBuilder();
        finalMessage.AppendLine("Assertion failed!");
        finalMessage.AppendLine(message);

        throw new MissionFailedException(finalMessage.ToString());
    }

    public static void MissionVariableNotNull(MissionVariables missionVariables, string propertyName)
    {
        var propertyInfo = missionVariables.GetType().GetProperty(propertyName);
        var value = propertyInfo?.GetValue(missionVariables);
        
        IsNotNull(value, $"Missing mission variable --{propertyInfo?.GetCustomAttribute<ArgumentAttribute>()?.Key}");
    }

    public static void MissionVariableNot<T>(MissionVariables missionVariables, string propertyName, T undesiredValue)
    {
        var propertyInfo = missionVariables.GetType().GetProperty(propertyName);
        var value = propertyInfo?.GetValue(missionVariables);
        
        if (value is T valueAsT)
        {
            if (valueAsT.Equals(undesiredValue))
            {
                throw new MissionFailedException($"Invalid value for --{propertyInfo?.GetCustomAttribute<ArgumentAttribute>()?.Key}");
            }
        }
    }

    public static void HostOperatingSystemIs(HostOperatingSystem os)
    {
        if (Platform.GetHostOperatingSystem() != os)
        {
            throw new MissionFailedException($"Only available on {os}");
        }
    }

    public static void HostOperatingSystemIsUnix()
    {
        if (Platform.GetHostOperatingSystem() is not (HostOperatingSystem.Linux or HostOperatingSystem.MacOs))
        {
            throw new MissionFailedException("Only available on Unix-like operating systems");
        }
    }
}