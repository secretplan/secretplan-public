using SecretPlanCore.Core;

namespace SecretPlanCore.ArgumentParsing;

public readonly struct PositionalArgumentList
{
    private readonly PositionalArgument[] _args = [];

    public PositionalArgumentList(PositionalArgument[] args)
    {
        _args = args;
    }

    /// <summary>
    ///     This overload expects that you called SplitTokens or equivalent to make sure quoted strings are handled
    /// </summary>
    public PositionalArgumentList(IEnumerable<string> argTokens)
    {
        var tokens = argTokens.ToList();
        _args = new PositionalArgument[tokens.Count];
        for (var index = 0; index < tokens.Count; index++)
        {
            var token = tokens[index];
            _args[index] = new PositionalArgument(token, index);
        }
    }

    public PositionalArgumentList(string allArgs) : this(allArgs.SplitTokens())
    {
    }

    public PositionalArgument Get(int index, string name)
    {
        if (!_args.IsValidIndex(index))
        {
            return new PositionalArgument(string.Empty, index, true, name);
        }

        return _args[index] with {Name = name};
    }

    public PositionalArgument this[int index]
    {
        get
        {
            if (!_args.IsValidIndex(index))
            {
                return new PositionalArgument("EMPTY", index, true);
            }

            return _args[index];
        }
    }

    public bool HasIndex(int index)
    {
        return _args.IsValidIndex(index);
    }

    public IEnumerable<PositionalArgument> PositionalArguments()
    {
        return _args;
    }

    public IEnumerable<string> GetUserEnteredTokens()
    {
        return _args.Select(a=>a.OriginalToken);
    }
}