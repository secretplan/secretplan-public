using Newtonsoft.Json;
using SecretPlanCore.Core;
using FileAccess = Godot.FileAccess;

namespace SecretPlanGodot.Core;

public static class VersionManager
{
    private static SecretPlanVersion? _version;

    public static SecretPlanVersion GetVersion()
    {
        if (_version == null)
        {
            try
            {
                using var file = FileAccess.Open("res://VERSION.json", FileAccess.ModeFlags.Read);
                _version = JsonConvert.DeserializeObject<SecretPlanVersion>(file.GetAsText());
            }
            catch (Exception)
            {
                // ignored
            }
        }

        return _version ?? new SecretPlanVersion();
    }
}