using Godot;

namespace SecretPlanGodot.Core;

public static class SecretResourceLoader
{
    private static readonly Dictionary<string, Resource> _cache = new();
    private static int LoadCount { get; set; }

    public static Resource? LoadTypeless(string path)
    {
        if (_cache.TryGetValue(path, out var cachedTypeless))
        {
            return cachedTypeless;
        }

        LoadCount++;
        if (!ResourceLoader.Exists(path))
        {
            return null;
        }

        var loaded = ResourceLoader.Load(path);

        _cache[path] = loaded;

        return loaded;
    }

    public static T? LoadTyped<T>(string path) where T : Resource
    {
        return LoadTypeless(path) as T;
    }
    
    public static T LoadTypedConfident<T>(string path) where T : Resource
    {
        if (!ResourceLoader.Exists(path))
        {
            throw new Exception($"Failed to find resource {path}");
        }

        return LoadTyped<T>(path)!;
    }
}