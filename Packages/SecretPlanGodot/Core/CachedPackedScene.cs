using Godot;

namespace SecretPlanGodot.Core;

public class CachedPackedScene<T> : CachedResource<PackedScene> where T : Node
{
    public CachedPackedScene(string path) : base(path)
    {
    }

    public T LoadAndInstantiate()
    {
        return GetOrLoad().Instantiate<T>();
    }
    
    public T? LoadAndInstantiateOrNull()
    {
        return GetOrLoadOrNull()?.Instantiate<T>();
    }

    public LoadingHandle LoadAndInstantiateAsync(Action<T> onLoadFinished)
    {
        return new LoadingHandle(
            Path,
            StartAsyncLoad,
            GetAsyncLoadStatus,
            () =>
            {
                LocalClient.Print($"Starting Instantiate for {Path}");
                var instance = FinishAsyncLoad().Instantiate<T>();
                onLoadFinished(instance);
                LocalClient.Print($"Done Instantiate for {Path}");
            });
    }
}