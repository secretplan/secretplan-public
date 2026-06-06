using System.Diagnostics.Contracts;
using Newtonsoft.Json;

namespace SecretPlanCore.Core;

public static class JsonHelpers
{
    [Pure]
    public static T? DeserializeSafe<T>(string json)
    {
        try
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
        catch (Exception)
        {
            return default;
        }
    }
    
    [Pure]
    public static object? DeserializeSafe(string json, Type type)
    {
        try
        {
            return JsonConvert.DeserializeObject(json, type);
        }
        catch (Exception)
        {
            return null;
        }
    }
}