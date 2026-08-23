using System.Text;
using Godot;
using SecretPlanGodot.Configuration;
using SecretPlanGodot.Serialization;

namespace SecretPlanGodot.Core;

public static class SecretResourceLoader
{
    private static readonly Dictionary<string, Resource> _cache = new();
    private static readonly Dictionary<string, Resource> _modCache = new();

    public static void InvalidateModCache()
    {
        _modCache.Clear();
    }

    public static Resource? LoadTypeless(string path)
    {
        if (_cache.TryGetValue(path, out var cachedTypeless))
        {
            return cachedTypeless;
        }

        var result = LoadTypelessInternal(path);

        if (result != null)
        {
            _cache[path] = result;
        }

        return result;
    }

    private static Resource? LoadTypelessInternal(string path)
    {
        if (LocalClient.IsModdingEnabled && path.StartsWith("mods://"))
        {
            var userPath = path.Replace("mods://", "user://Mods/");

            var loadedFromUserPath = LoadTypelessSimple(userPath);

            if (loadedFromUserPath != null)
            {
                return loadedFromUserPath;
            }

            if (ResourceReferenceUtilities.PathLooksLikeResource<Texture2D>(userPath))
            {
                var image = Image.LoadFromFile(userPath);
                return new ImageTexture { Image = image };
            }
            
            if (ResourceReferenceUtilities.PathLooksLikeResource<AudioStreamOggVorbis>(userPath))
            {
                var stream = AudioStreamOggVorbis.LoadFromFile(userPath);
                return stream;
            }

            return null;
        }

        return LoadTypelessSimple(path);
    }

    private static Resource? LoadTypelessSimple(string path)
    {
        if (!ResourceLoader.Exists(path))
        {
            return null;
        }

        var loaded = ResourceLoader.Load(path);


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

    public static IEnumerable<string> ListDirectoryRecursive(string path, bool ignoreDotGodot = true)
    {
        foreach (var entry in ResourceLoader.ListDirectory(path))
        {
            if (ignoreDotGodot && entry == ".godot")
            {
                continue;
            }

            var fullPathStringBuilder = new StringBuilder();
            if (path.EndsWith('/'))
            {
                fullPathStringBuilder
                    .Append(path)
                    .Append(entry);
            }
            else
            {
                fullPathStringBuilder
                    .Append(path)
                    .Append('/')
                    .Append(entry);
            }

            var fullPath = fullPathStringBuilder.ToString();

            if (entry.EndsWith('/'))
            {
                foreach (var file in ListDirectoryRecursive(fullPath.TrimEnd('/')))
                {
                    yield return file;
                }
            }
            else
            {
                yield return fullPath;
            }
        }
    }

    public static bool Exists(string path)
    {
        if (LocalClient.IsModdingEnabled && path.StartsWith("mods://"))
        {
            // If it's a .ogg or .png or something that wants an import. ResourceLoader will blissfully ignore it!
            // So we need to ask the filesystem directly if the file exists.
            return CommonSerializationConstants.AppDataFiles.GetDirectory("Mods").HasFile(path.Remove(0,"mods://".Length));
        }

        return ResourceLoader.Exists(path);
    }
}