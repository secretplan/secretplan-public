namespace SecretPlanCore.ArgumentParsing;

public readonly struct ConsoleCommand
{
    public string InvokeWord { get; }
    private readonly Action<PositionalArgumentList> _run;

    public ConsoleCommand(string invokeWord, Action<PositionalArgumentList> run)
    {
        InvokeWord = invokeWord;
        _run = run;
    }

    public Validity GetValidity(string[] tokens)
    {
        if (tokens.Length == 0)
        {
            return Validity.NoMatch;
        }

        if (tokens[0] == InvokeWord)
        {
            //if (tokens.Length == ArgCount + 1)
            {
                return Validity.CorrectMatch;
            }

            // return Validity.MatchInvokeButWrongArgs;
        }

        return Validity.NoMatch;
    }

    public string? Run(string[] tokens)
    {
        try
        {
            var args = new PositionalArgument[tokens.Length - 1];
            for (var i = 1; i < tokens.Length; i++)
            {
                var argIndex = i - 1;
                args[argIndex] = new PositionalArgument(tokens[i], i);
            }

            _run(new PositionalArgumentList(args));
        }
        catch (ArgParseFailedException exception)
        {
            return $"Failed to parse {exception.Argument}\n" + exception.Message;
        }

        // no errors
        return null;
    }

    public enum Validity
    {
        NoMatch,
        CorrectMatch
    }
}