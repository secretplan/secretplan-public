using Godot;
using SecretPlanCore.Configuration;
using SecretPlanCore.Core;
using SecretPlanGodot.Core;
using FileAccess = Godot.FileAccess;

namespace SecretPlanGodot.Serialization;

public static class CommonSerializationConstants
{
    public static readonly string SavePath = ProjectSettings.GlobalizePath("user://save.json");
    public static readonly string SettingsPath = ProjectSettings.GlobalizePath("user://settings.json");
    public static readonly string UserRoot = ProjectSettings.GlobalizePath("user://");
    private static Texture2D? _cachedMissingTexture;
    public static RealFileSystem AppDataFiles { get; } = new(UserRoot);

    public static Texture2D MissingTexture
    {
        get
        {
            if (_cachedMissingTexture == null)
            {
                var image = new Image();
                image.SetData(2, 2, false, Image.Format.Rgb8, new byte[2 * 2 * 3]);
                image.Resize(64, 64, Image.Interpolation.Nearest);
                image.Fill(Colors.Black);
                image.FillRect(new Rect2I(0, 0, 32, 32), Colors.Fuchsia);
                image.FillRect(new Rect2I(32, 32, 32, 32), Colors.Fuchsia);
                _cachedMissingTexture = ImageTexture.CreateFromImage(image);
            }

            return _cachedMissingTexture;
        }
    }
    
    public static void PopulateConfigServer(bool shouldLog)
    {
        ConfigServer.Clear();

        var lines = GodotUtilities.ReadTextResourceFileLines("res://AllConfigs.txt");

        foreach (var configFileName in lines)
        {
            if (string.IsNullOrWhiteSpace(configFileName))
            {
                continue;
            }

            using var configFileAccess = FileAccess.Open("res://" + configFileName, FileAccess.ModeFlags.Read);

            if (configFileAccess == null)
            {
                LocalClient.Error($"Could not read config {configFileName}");
                continue;
            }

            var json = configFileAccess.GetAsText();
            var untypedInstance = ConfigServer.Instance.LoadFromJsonUntyped(configFileName, json, true);
            var id = untypedInstance?.InstanceInfo.InstanceId ?? 0;
            var loadedInstance = ConfigServer.Instance.GetInstanceUntyped(id);

            if (loadedInstance == null)
            {
                LocalClient.Error($"Failed to load config at {configFileName}");
                continue;
            }

            if (shouldLog)
            {
                // LocalClient.Print($"Loaded config {id}");
            }
        }
    }
}