namespace SecretPlanCore.Core;

public class BoolProvider : ValueProvider<bool>
{
    public BoolProvider(bool startingValue) : base(startingValue)
    {
    }

    public BoolProvider(Func<bool> providerFunction) : base(providerFunction)
    {
    }

    public BoolProvider() : base(false)
    {
    }
}