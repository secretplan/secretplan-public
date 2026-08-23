using System.Text;
using Godot;
using SecretPlanCore.Core;
using Environment = System.Environment;

namespace SecretPlanGodot.Core;

public static class LocalClient
{
    private static bool? _isModdingEnabled;
    private static bool? _isAllowedLocalHost;
    private static bool? _isDev;
    private static string? _cachedUniqueId;
    private static Color? _cachedColor;

    public static bool IsModdingEnabled => _isModdingEnabled ??= HasCommandLineFlag("--enablemods");

    private static readonly string _generatedUniqueId = Guid.NewGuid().ToString().Substring(0, 5);

    public static readonly string RichTextDescriptor = $"[color={UniqueColor.ToHtml()}]{UniqueId}[/color]";

    public static string UniqueId
    {
        get { return _cachedUniqueId ??= ComputeManualUniqueId() ?? _generatedUniqueId; }
    }

    public static Color UniqueColor
    {
        get { return _cachedColor ??= Color.FromHtml(Hashing.HashStringToColor(UniqueId)); }
    }

    private static string CurrentTimeString => DateTime.Now.ToString("h:mm:sstt");
    public static bool IsDev => _isDev ??= HasCommandLineFlag("--dev");
    public static bool IsAllowedLocalHost => IsDev || (_isAllowedLocalHost ??= HasCommandLineFlag("--allowlocalhost"));

    private static bool HasCommandLineFlag(string flag)
    {
        foreach (var arg in OS.GetCmdlineUserArgs())
        {
            if (arg == flag)
            {
                return true;
            }
        }

        return false;
    }

    private static string? ComputeManualUniqueId()
    {
        foreach (var arg in OS.GetCmdlineUserArgs())
        {
            if (arg.StartsWith("--id="))
            {
                return arg.Replace("--id=", "");
            }
        }

        return null;
    }

    public static void Print(params object?[] originalMessage)
    {
        PrintInternal("", originalMessage);
    }

    public static void Error(params object?[] originalMessage)
    {
        PrintInternal("[color=ffaaaa]ERROR[/color] ", originalMessage);
    }

    private static void PrintInternal(string prefix, params object?[] originalMessage)
    {
        var colorString = UniqueColor.ToHtml(false);
        var newMessage = new List<object>
        {
            "[color=",
            colorString,
            "]",
            UniqueId,
            "[/color] ",
            CurrentTimeString,
            " [color=",
            colorString,
            "]#[/color] ",
            prefix
        };

        newMessage.AddRange(originalMessage.Select(a =>
        {
            if (a == null)
            {
                return "null";
            }

            return a;
        }));

        GD.PrintRich(newMessage.ToArray());

        var localMessage = new StringBuilder();
        localMessage.Append(CurrentTimeString);
        localMessage.Append(" ");
        localMessage.Append($"[color={colorString}]#[/color]");
        localMessage.Append(" ");
        localMessage.Append(prefix);
        foreach (var item in originalMessage)
        {
            localMessage.Append(item);
        }

        OnMessage?.Invoke(localMessage.ToString());
    }

    public static event Action<string>? OnMessage;

    public static void LogInit()
    {
        Print("True Args: ", string.Join(" ", Environment.GetCommandLineArgs().Select(a => $"[{a}]")));
        Print("User Args: ", string.Join(" ", OS.GetCmdlineUserArgs().Select(a => $"[{a}]")));
        Print($"Version is: {VersionManager.GetVersion()}");

        Print($"Godot Version: {GetGodotVersion()}");
        Print($"Local Time: {DateTime.Now:g}");
        Print($"UTC Time: {DateTime.UtcNow:g}");

        PerfClamps.Print += Print;
    }


    private static string GetGodotVersion()
    {
        var godotVersionInfo = Engine.GetVersionInfo();
        if (godotVersionInfo.TryGetValue("string", out var version))
        {
            return version.AsString();
        }

        return "???";
    }
}