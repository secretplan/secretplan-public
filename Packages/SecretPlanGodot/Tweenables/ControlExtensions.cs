using System.Diagnostics.Contracts;
using ExTween;
using Godot;

namespace SecretPlanGodot.Tweenables;

public static class ControlExtensions
{
    [Pure]
    public static TweenableVector2 TweenableGlobalPosition(this Control node)
    {
        return new TweenableVector2(node.GetGlobalPosition, x => node.SetGlobalPosition(x));
    }

    [Pure]
    public static TweenableVector2 TweenableLocalPosition(this Control node)
    {
        return new TweenableVector2(node.GetPosition, x => node.SetPosition(x));
    }

    [Pure]
    public static TweenableFloat TweenableLocalRotationRadians(this Control node)
    {
        return new TweenableFloat(node.GetRotation, node.SetRotation);
    }

    [Pure]
    public static TweenableFloat TweenableUniformScale(this Control node)
    {
        return new TweenableFloat(() => node.Scale.X, x => node.Scale = new Vector2(x, x));
    }

    [Pure]
    public static TweenableVector2 TweenableScale(this Control node)
    {
        return new TweenableVector2(() => node.Scale, x => node.Scale = x);
    }


    [Pure]
    public static TweenableRect2 TweenableGlobalRect(this Control node)
    {
        return new TweenableRect2(node.GetGlobalRect, x =>
        {
            node.GlobalPosition = x.Position;
            node.Size = x.Size;
        });
    }

    [Pure]
    public static TweenableRect2 TweenableLocalRect(this Control node)
    {
        return new TweenableRect2(node.GetRect, x =>
        {
            node.Position = x.Position;
            node.Size = x.Size;
        });
    }

    public static void SetLocalRect(this Control control, Rect2 rect)
    {
        control.Position = rect.Position;
        control.Size = rect.Size;
    }

    public static void SetGlobalRect(this Control control, Rect2 rect)
    {
        control.GlobalPosition = rect.Position;
        control.Size = rect.Size;
    }
}