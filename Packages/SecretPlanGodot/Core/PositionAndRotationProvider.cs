using SecretPlanCore.Core;

namespace SecretPlanGodot.Core;

public class PositionAndRotationProvider : ValueProvider<PositionAndRotation>
{
    public PositionAndRotationProvider(PositionAndRotation startingValue) : base(startingValue)
    {
    }

    public PositionAndRotationProvider(Func<PositionAndRotation> providerFunction) : base(providerFunction)
    {
    }

    public PositionAndRotationProvider() : base(new PositionAndRotation())
    {
    }
}