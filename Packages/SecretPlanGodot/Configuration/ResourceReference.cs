using Godot;
using Newtonsoft.Json;
using SecretPlanGodot.Core;

namespace SecretPlanGodot.Configuration;

/// <summary>
///     References an "asset" represented by a string (usually a Godot resource as a res:// path)
/// </summary>
public class ResourceReference<T> : IResourceReference where T : Resource
{
    private CachedResource<T>? _cachedResource;

    public ResourceReference(string path)
    {
        Path = path;
    }

    public ResourceReference()
    {
    }

    [JsonIgnore]
    public CachedResource<T> CachedResource => _cachedResource ??= new CachedResource<T>(Path);

    [JsonProperty("path")]
    public string Path { get; init; } = string.Empty;

    [JsonIgnore]
    public Type ResourceType => typeof(T);

    public object? GetOrLoadOrNullTypeless()
    {
        return CachedResource.GetOrLoadOrNull();
    }

    public static implicit operator T?(ResourceReference<T>? self)
    {
        if (self == null)
        {
            return null;
        }

        if (self.CachedResource.IsValid())
        {
            return self.CachedResource.GetOrLoad();
        }

        return null;
    }

    protected bool Equals(ResourceReference<T> other)
    {
        return Path == other.Path;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null)
        {
            return false;
        }

        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (obj.GetType() != GetType())
        {
            return false;
        }

        return Equals((ResourceReference<T>)obj);
    }

    public override int GetHashCode()
    {
        return Path.GetHashCode();
    }

    public bool HasValue()
    {
        return !string.IsNullOrWhiteSpace(Path);
    }
}