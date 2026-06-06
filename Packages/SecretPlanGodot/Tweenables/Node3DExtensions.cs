using Godot;

namespace SecretPlanGodot.Tweenables;

public static class Node3DExtensions
{
    public static TweenableVector3 TweenableGlobalPosition(this Node3D node)
    {
        return new TweenableVector3(node.GetGlobalPosition, node.SetGlobalPosition);
    }

    public static TweenableVector3 TweenableLocalPosition(this Node3D node)
    {
        return new TweenableVector3(node.GetPosition, node.SetPosition);
    }

    public static TweenableVector3 TweenableScale(this Node3D node)
    {
        return new TweenableVector3(node.GetScale, node.SetScale);
    }

    public static TweenableQuaternion TweenableGlobalRotationQuaternion(this Node3D node)
    {
        return new TweenableQuaternion(node.GlobalBasis.GetRotationQuaternion, quaternion =>
        {
            var scale = node.Scale;
            node.GlobalBasis = new Basis(quaternion.Normalized());
            node.Scale = scale;
        });
    }

    public static TweenableQuaternion TweenableLocalRotationQuaternion(this Node3D node)
    {
        return new TweenableQuaternion(node.GetQuaternion, node.SetQuaternion);
    }
}