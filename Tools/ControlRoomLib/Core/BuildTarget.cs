namespace ControlRoomLib.Core;

public readonly struct BuildTarget
{
    public string HumanReadableName { get; init; }
    public string BuildExeNameTemplate { get; init; }
    public string ShortHandName { get; init; }

    /// <summary>
    ///     The (default) Godot Build Template associated with this platform. Technically this is game-specific, but if we
    ///     stick to the defaults we should be OK
    /// </summary>
    public string GodotBuildTemplateName { get; init; }

    public string GetExeName(string gameName)
    {
        return string.Format(BuildExeNameTemplate, gameName);
    }

    public static BuildTarget MacOsUniversal =>
        new()
        {
            HumanReadableName = "macOS Universal",
            BuildExeNameTemplate = "{0}.app",
            ShortHandName = "macos-universal",
            GodotBuildTemplateName = "macOS"
        };

    public static BuildTarget Windows =>
        new()
        {
            HumanReadableName = "Windows",
            BuildExeNameTemplate = "{0}.exe",
            ShortHandName = "win",
            GodotBuildTemplateName = "Windows Desktop"
        };

    public static BuildTarget Linux =>
        new()
        {
            HumanReadableName = "Linux",
            BuildExeNameTemplate = "{0}",
            ShortHandName = "linux",
            GodotBuildTemplateName = "Linux"
        };

    public static IEnumerable<BuildTarget> AllAvailable()
    {
        yield return Windows;
        yield return MacOsUniversal;
        yield return Linux;
    }

    public override string ToString()
    {
        return HumanReadableName;
    }
}