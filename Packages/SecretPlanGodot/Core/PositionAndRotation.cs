using Godot;

namespace SecretPlanGodot.Core;

public record struct PositionAndRotation(Vector3 Position, Quaternion Rotation)
{
    public static PositionAndRotation Lerp(PositionAndRotation start, PositionAndRotation end, float percent)
    {
        return new PositionAndRotation(
            start.Position.Lerp(end.Position, percent),
            start.Rotation.Slerp(end.Rotation, percent)
        );
    }
}