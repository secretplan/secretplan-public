using Godot;

namespace SecretPlanGodot.Extensions;

public static class VectorExtensions
{
    public static string ToCommaSeparatedString(this Vector3 vector3)
    {
        return $"{vector3.X},{vector3.Y},{vector3.Z}";
    }

    /// <summary>
    ///     Turns the X,Z components into a Vector2
    /// </summary>
    public static Vector2 ToGroundPlaneVector(this Vector3 vector3)
    {
        return new Vector2(vector3.X, vector3.Z);
    }
}