namespace SecretPlanCore.ArgumentParsing;

public readonly record struct PositionalArgument(
    string OriginalToken,
    int TokenIndex,
    bool IsEmpty = false,
    string Name = "")
{
    public override string ToString()
    {
        var nameString = $"<{Name}> ";
        if (Name == null)
        {
            nameString = "";
        }

        if (IsEmpty)
        {
            return $"{nameString}at [{TokenIndex}]";
        }

        return $"{nameString}at [{TokenIndex}] given: `{OriginalToken}`";
    }

    public bool ParseAsBool()
    {
        if (IsEmpty)
        {
            throw new ParseFailedEmptyException<bool>(this);
        }

        if (bool.TryParse(OriginalToken, out var result))
        {
            return result;
        }

        var lower = OriginalToken.ToLower().Trim();
        if (lower == "off")
        {
            return false;
        }

        if (lower == "on")
        {
            return true;
        }

        throw new ArgParseFailedTypeException<bool>(this);
    }

    public string ParseAsString()
    {
        if (IsEmpty)
        {
            throw new ParseFailedEmptyException<string>(this);
        }

        return OriginalToken;
    }

    public string ParseAsSpecificString(params string[] validResponses)
    {
        if (IsEmpty)
        {
            throw new ArgParseFailedException(this,
                $"Expected one of: {string.Join(", ", validResponses)}");
        }

        var trimmedInput = OriginalToken.Trim().ToLowerInvariant();

        if (!string.IsNullOrWhiteSpace(trimmedInput))
        {
            HashSet<string> possibleResponse = [];

            foreach (var validResponse in validResponses)
            {
                if (string.Equals(trimmedInput, validResponse, StringComparison.CurrentCultureIgnoreCase))
                {
                    return trimmedInput;
                }

                if (validResponse.StartsWith(trimmedInput, StringComparison.CurrentCultureIgnoreCase))
                {
                    possibleResponse.Add(validResponse);
                }
            }

            if (possibleResponse.Count == 1)
            {
                // You typed `mac` when we wanted `mac-os`, good enough assuming nothing else started with `mac`
                return possibleResponse.First();
            }
        }

        throw new ArgParseFailedException(this,
            $"Could not parse `{OriginalToken}` as one of: {string.Join(", ", validResponses)}");
    }

    public int ParseAsInt()
    {
        if (IsEmpty)
        {
            throw new ParseFailedEmptyException<int>(this);
        }

        if (int.TryParse(OriginalToken, out var result))
        {
            return result;
        }

        throw new ArgParseFailedTypeException<int>(this);
    }
    
    public uint ParseAsUInt()
    {
        if (IsEmpty)
        {
            throw new ParseFailedEmptyException<uint>(this);
        }

        if (uint.TryParse(OriginalToken, out var result))
        {
            return result;
        }

        throw new ArgParseFailedTypeException<uint>(this);
    }

    public float ParseAsFloat()
    {
        if (IsEmpty)
        {
            throw new ParseFailedEmptyException<float>(this);
        }

        if (float.TryParse(OriginalToken, out var result))
        {
            return result;
        }

        throw new ArgParseFailedTypeException<float>(this);
    }

    public ulong ParseAsULong()
    {
        if (IsEmpty)
        {
            throw new ParseFailedEmptyException<ulong>(this);
        }

        if (ulong.TryParse(OriginalToken, out var result))
        {
            return result;
        }

        throw new ArgParseFailedTypeException<ulong>(this);
    }

    public T ParseAsEnum<T>() where T : struct, Enum
    {
        if (Enum.TryParse(typeof(T), OriginalToken, true, out var enumResult))
        {
            return (T)enumResult;
        }

        throw new ArgParseFailedException(this,
            $"Could not parse `{OriginalToken}` as any of: {string.Join(", ", Enum.GetNames<T>())}");
    }
}