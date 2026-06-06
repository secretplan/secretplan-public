using System.Diagnostics.CodeAnalysis;
using Godot;

namespace SecretPlanGodot.Core;

public class CachedNode<T> where T : Node
{
    private readonly NodePath _nodePath;
    private T? _cache;

    public CachedNode(NodePath nodePath)
    {
        _nodePath = nodePath;
    }

    public void ClearCache()
    {
        _cache = null;
    }

    public T? GetOrNull(Node parentNode)
    {
        ClearCacheIfInvalid();
        return _cache ?? parentNode.GetNodeOrNull<T>(_nodePath);
    }

    public T Get(Node parentNode)
    {
        ClearCacheIfInvalid();
        return _cache ??= FindNodeConfident(parentNode);
    }

    private void ClearCacheIfInvalid()
    {
        if (!GodotObject.IsInstanceValid(_cache))
        {
            _cache = null;
        }

        if (_cache?.IsQueuedForDeletion() == true)
        {
            _cache = null;
        }
    }


    private T FindNodeConfident(Node parentNode)
    {
        var maybeNode = parentNode.GetNodeOrNull<T>(_nodePath);

        if (maybeNode == null)
        {
            var typeless = parentNode.GetNodeOrNull(_nodePath);

            if (typeless != null)
            {
                throw new Exception(
                    $"WRONG SCRIPT - Child at path {_nodePath} is a {typeless.GetType().Name}, expected {typeof(T).Name}");
            }

            throw new Exception($"MISSING NODE - Child at path {_nodePath} does not exist");
        }

        return maybeNode;
    }
}