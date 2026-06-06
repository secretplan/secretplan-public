using Godot;

namespace SecretPlanGodot.Core;

public static class Rect2Extensions
{
    public static bool Contains(this Rect2 rect, Vector2 screenPoint)
    {
        return screenPoint.X >= rect.Position.X && screenPoint.X <= rect.End.X &&
               screenPoint.Y >= rect.Position.Y && screenPoint.Y <= rect.End.Y;
    }
}