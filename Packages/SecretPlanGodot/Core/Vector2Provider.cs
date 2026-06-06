using Godot;
using SecretPlanCore.Core;

namespace SecretPlanGodot.Core;

public class Vector2Provider : ValueProvider<Vector2>
{
    public Vector2Provider(Vector2 startingValue) : base(startingValue)
    {
    }

    public Vector2Provider(Func<Vector2> providerFunction) : base(providerFunction)
    {
    }

    public Vector2Provider() : base(Vector2.Zero)
    {
    }
}