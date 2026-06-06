using Godot;

namespace SecretPlanGodot.Configuration;

/// <summary>
/// Use this only when the T is not a built-in type
/// </summary>
public class PackedSceneReference : PackedSceneReferenceTyped<Node>
{
    public PackedSceneReference(string path) : base(path)
    {
    }

    public PackedSceneReference()
    {
    }
}