using Godot;

namespace SecretPlanGodot.Core;

public class CachedAncestor<T>
{
    private T? _cached;

    public T? GetOrNull(Node node)
    {
        return _cached ??= node.ClimbAncestorsUntilFindType<T>();
    }
}