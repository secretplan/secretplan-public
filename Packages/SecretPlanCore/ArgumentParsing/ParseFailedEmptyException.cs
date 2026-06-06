namespace SecretPlanCore.ArgumentParsing;

public class ParseFailedEmptyException<TExpected> : ArgParseFailedException
{
    public ParseFailedEmptyException(PositionalArgument argument) : base(argument, $"Expected {argument} as {typeof(TExpected).Name}, but was empty")
    {
    }
}