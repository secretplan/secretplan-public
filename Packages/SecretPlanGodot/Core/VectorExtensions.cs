using Godot;

namespace SecretPlanGodot.Core;

public static class VectorExtensions
{
    public static Vector2I ToVector2I(this Vector2 vector2)
    {
        return new Vector2I((int)vector2.X, (int)vector2.Y);
    }
}