namespace SecretPlanCore.ArgumentParsing;

public class ArgParseFailedTypeException<TExpected> : ArgParseFailedException
{
    public ArgParseFailedTypeException(PositionalArgument argument) : base(argument,
        $"Could not parse {argument} as {typeof(TExpected).Name}")
    {
    }
}