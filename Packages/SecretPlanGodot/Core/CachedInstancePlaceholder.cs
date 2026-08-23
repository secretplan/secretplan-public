using Godot;

namespace SecretPlanGodot.Core;

public class CachedInstancePlaceholder<T> : CachedNode<InstancePlaceholder> where T : Node
{
    public CachedInstancePlaceholder(NodePath nodePath) : base(nodePath)
    {
    }

    public T CreateInstanceTyped(Node parentNode)
    {
        var instance = Get(parentNode).CreateInstance();

        if (instance == null)
        {
            throw new Exception("Instance Placeholder created a null!");
        }

        if (instance is not T instanceAsT)
        {
            throw new Exception($"Could not cast Instance Placeholder {instance} as {typeof(T).Name}");
        }

        return instanceAsT;
    }
}