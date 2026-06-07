using Godot;
using SecretPlanGodot.Core;

namespace Sokomatic;

public class ParentCore : CachedAncestor<Core>
{
    public CoreState State(Node node)
    {
        var state = GetOrNull(node)?.CoreState;
        if (state != null)
        {
            return state;
        }

        LocalClient.Error("Creating new CoreState");
        return new CoreState();
    }
}