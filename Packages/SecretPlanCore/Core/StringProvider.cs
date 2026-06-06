namespace SecretPlanCore.Core;

public class StringProvider : ValueProvider<string>
{
    public StringProvider(string startingValue) : base(startingValue)
    {
    }

    public StringProvider(Func<string> providerFunction) : base(providerFunction)
    {
    }

    public StringProvider() : base(string.Empty)
    {
    }
}