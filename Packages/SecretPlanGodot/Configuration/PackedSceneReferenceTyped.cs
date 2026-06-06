using Godot;
using Newtonsoft.Json;
using SecretPlanGodot.Core;

namespace SecretPlanGodot.Configuration;

/// <summary>
///     Rarely used because the Data Assembly doesn't know the scene types the Game Assembly uses.
/// </summary>
public class PackedSceneReferenceTyped<T> : IResourceReference where T : Node
{
    private CachedPackedScene<T>? _cachedPackedScene;
    private string _underlyingPath = string.Empty;

    public PackedSceneReferenceTyped()
    {
    }

    public PackedSceneReferenceTyped(string path)
    {
        Path = path;
    }

    [JsonIgnore]
    public CachedPackedScene<T> CachedPackedScene => _cachedPackedScene ??= new CachedPackedScene<T>(Path);

    [JsonProperty("path")]
    public string Path
    {
        get => _underlyingPath;
        private set
        {
            // path changed, so packed scene must as well
            _cachedPackedScene = new CachedPackedScene<T>(value);
            _underlyingPath = value;
        }
    }

    [JsonIgnore]
    public Type ResourceType => typeof(PackedScene);

    public TDerived LoadAndInstantiateAs<TDerived>() where TDerived : T
    {
        return CachedPackedScene.GetOrLoad().Instantiate<TDerived>();
    }
}