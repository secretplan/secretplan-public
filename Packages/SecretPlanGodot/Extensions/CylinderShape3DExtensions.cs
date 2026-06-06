using Godot;
using SecretPlanCore.Core;

namespace SecretPlanGodot.Extensions;

public static class CylinderShape3DExtensions
{
    public static Vector3 GetRandomLocalPointAlongTop(this CylinderShape3D cylinderShape3D, NoiseBasedRng random)
    {
        var halfHeight = cylinderShape3D.Height / 2f;
        var radius = cylinderShape3D.Radius;
        var angle = random.NextRadian();
        var finalRadius = radius * random.NextFloat();
        return new Vector3(
            MathF.Cos(angle) * finalRadius,
            halfHeight,
            MathF.Sin(angle) * finalRadius
        );
    }

    public static Vector3 GetRandomLocalPointWithinVolume(this CylinderShape3D cylinderShape3D, NoiseBasedRng random)
    {
        var halfHeight = cylinderShape3D.Height / 2f;
        var radius = cylinderShape3D.Radius;
        var angle = random.NextRadian();
        var finalRadius = radius * random.NextFloat();
        return new Vector3(
            MathF.Cos(angle) * finalRadius,
            random.NextFloat(-halfHeight, halfHeight),
            MathF.Sin(angle) * finalRadius
        );
    }

    public static Vector3 GetRandomLocalPointAlongSide(this CylinderShape3D cylinderShape3D, NoiseBasedRng random)
    {
        var halfHeight = cylinderShape3D.Height / 2f;
        var radius = cylinderShape3D.Radius;
        var angle = random.NextRadian();
        var finalHeight = random.NextFloat(-halfHeight, halfHeight);
        return new Vector3(MathF.Cos(angle) * radius, finalHeight, MathF.Sin(angle) * radius);
    }
}