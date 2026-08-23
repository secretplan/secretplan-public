using Godot;

namespace SecretPlanGodot.Core;

public class CachedStateAncestor<TState> : CachedAncestor<IStateProvider<TState>>
    where TState : new()
{
    public TState State(Node self)
    {
        var result = GetOrNull(self);
        if (result != null)
        {
            return result.State;
        }

        LocalClient.Error($"Creating new {typeof(TState).Name}");
        return new TState();
    }
}