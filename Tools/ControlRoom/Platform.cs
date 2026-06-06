using System.Text;
using ControlRoom.Core;
using ControlRoom.Missions;
using Newtonsoft.Json;

namespace ControlRoom;

public static class Platform
{
    public static SerializedState SerializedState { get; private set; } = new();

    public static string Shortcut(string shortcut, string? helpOnFail = null)
    {
        if (SerializedState.Shortcuts.TryGetValue(shortcut, out var value))
        {
            return value;
        }

        var stringBuilder = new StringBuilder();

        stringBuilder.AppendLine($"Could not find shortcut: `{shortcut}`");

        if (helpOnFail != null)
        {
            stringBuilder.AppendLine(helpOnFail);
        }
        else
        {
            stringBuilder.AppendLine($"hint: {Constants.GenerateMissionCommand<Shortcut>($"add {shortcut} <value>")}");
        }

        throw new MissionFailedException(stringBuilder.ToString());
    }

    public static void Startup()
    {
        if (File.Exists(Constants.StatePath))
        {
            var json = File.ReadAllText(Constants.StatePath);
            try
            {
                var state = JsonConvert.DeserializeObject<SerializedState>(json);
                if (state != null)
                {
                    SerializedState = state;
                }
            }
            catch (Exception exception)
            {
                throw new MissionFailedException("Failed to read state", exception);
            }
        }

        Console.OutputEncoding = Encoding.UTF8;
        Directory.CreateDirectory(Constants.LogsFolder);
        OutPipe.Open($"{Constants.LogsFolder}/{Constants.CurrentLogFile}");
    }

    public static void Shutdown()
    {
        File.WriteAllText(Constants.StatePath, JsonConvert.SerializeObject(SerializedState, Formatting.Indented));
        OutPipe.Close();
    }

    public static bool HasShortcut(string key)
    {
        if (SerializedState.Shortcuts.ContainsKey(key))
        {
            return true;
        }

        return false;
    }

    public static HostOperatingSystem GetHostOperatingSystem()
    {
        if (OperatingSystem.IsWindows())
        {
            return HostOperatingSystem.Windows;
        }

        if (OperatingSystem.IsMacOS())
        {
            return HostOperatingSystem.MacOs;
        }

        if (OperatingSystem.IsLinux())
        {
            return HostOperatingSystem.Linux;
        }

        return HostOperatingSystem.Unknown;
    }
}

public enum HostOperatingSystem
{
    Unknown = 0,
    Windows,
    MacOs,
    Linux
}