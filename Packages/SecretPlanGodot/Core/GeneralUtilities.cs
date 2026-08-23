using System.Diagnostics.Contracts;
using Godot;
using Newtonsoft.Json;
using SecretPlanGodot.Configuration;

namespace SecretPlanGodot.Core;

public static class GeneralUtilities
{
    public static T? JsonDeserializeSafe<T>(string json)
    {
        try
        {
            var result = JsonConvert.DeserializeObject<T>(json);
            return result;
        }
        catch (Exception e)
        {
            LocalClient.Error($"Failed to parse json {json} as {typeof(T).Name} {e}, returning default!");
            return default;
        }
    }
    
    [Pure]
    public static T LoadJsonOrDefault<T>(string path, Action<T> setupDefault) where T : new()
    {
        var thingName = typeof(T).Name;
        if (File.Exists(path))
        {
            var json = File.ReadAllText(path);
            var readResult = JsonDeserializeSafe<T>(json);
            if (readResult != null)
            {
                return readResult;
            }

            File.WriteAllText(path + "_failed_parse_" + DateTime.Now.ToFileTimeUtc(), json);
        }

        LocalClient.Print($"{thingName} was not loaded, loading default.");
        var instance = new T();
        setupDefault(instance);
        return instance;
    }
}