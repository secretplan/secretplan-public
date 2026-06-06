using Godot;
using SecretPlanCore.Core;

namespace SecretPlanGodot.Extensions;

public static class BoxShape3DExtensions
{
    public static Vector3 GetRandomLocalPointAlongTop(this BoxShape3D boxShape3D, NoiseBasedRng random)
    {
        var halfBoxSize = boxShape3D.Size / 2f;
        return new Vector3(
            random.NextFloat(-halfBoxSize.X, halfBoxSize.X),
            halfBoxSize.Y,
            random.NextFloat(-halfBoxSize.Z, halfBoxSize.Z));
    }
    
    public static Vector3 GetRandomLocalPointWithinVolume(this BoxShape3D boxShape3D, NoiseBasedRng random)
    {
        var halfBoxSize = boxShape3D.Size / 2f;
        return new Vector3(
            random.NextFloat(-halfBoxSize.X, halfBoxSize.X),
            random.NextFloat(-halfBoxSize.Y, halfBoxSize.Y),
            random.NextFloat(-halfBoxSize.Z, halfBoxSize.Z));
    }
}