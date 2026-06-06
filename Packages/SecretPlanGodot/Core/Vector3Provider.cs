using Godot;
using SecretPlanCore.Core;

namespace SecretPlanGodot.Core;

public class Vector3Provider : ValueProvider<Vector3>
{
    public Vector3Provider(Vector3 startingValue) : base(startingValue)
    {
    }

    public Vector3Provider(Func<Vector3> providerFunction) : base(providerFunction)
    {
    }

    public Vector3Provider() : base(Vector3.Zero)
    {
    }
}