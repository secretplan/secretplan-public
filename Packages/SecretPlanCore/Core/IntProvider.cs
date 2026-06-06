namespace SecretPlanCore.Core;

public class IntProvider : ValueProvider<int>
{
    public IntProvider(int startingValue) : base(startingValue)
    {
    }

    public IntProvider(Func<int> providerFunction) : base(providerFunction)
    {
    }

    public IntProvider() : base(0)
    {
    }
}