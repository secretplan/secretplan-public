namespace SecretPlanCore.ArgumentParsing;

public class ArgParseFailedException : Exception
{
    public PositionalArgument Argument { get; }

    public ArgParseFailedException(PositionalArgument argument,
        string message) : base(message)
    {
        Argument = argument;
    }
}