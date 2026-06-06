using Godot;
using SecretPlanCore.Core;

namespace SecretPlanGodot.Providers;

public class Rect2Provider : ValueProvider<Rect2>
{
    public Rect2Provider(Rect2 startingValue) : base(startingValue)
    {
    }

    public Rect2Provider(Func<Rect2> providerFunction) : base(providerFunction)
    {
    }

    public Rect2Provider() : base(new Rect2())
    {
        
    }
}