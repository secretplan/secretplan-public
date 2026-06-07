using Godot;
using SecretPlanCore.Core;
using SecretPlanGodot.Core;

namespace Sokomatic;

public static class GameConstants
{
    public static string ReadTextResourceFile(string path)
    {
        var fileAccess = FileAccess.Open(path, FileAccess.ModeFlags.Read);

        if (fileAccess == null)
        {
            LocalClient.Error($"Could not read file: {path}");
            return string.Empty;
        }

        return fileAccess.GetAsText();
    }
}