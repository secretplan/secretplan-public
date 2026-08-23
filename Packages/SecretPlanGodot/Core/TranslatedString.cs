namespace SecretPlanGodot.Core;

public readonly record struct TranslatedString(string Content, TranslationResult Result)
{
    /// <summary>
    ///     This is a sentinel value that represents a string that could not resolve
    /// </summary>
    public static TranslatedString Unknown { get; } = new("???", TranslationResult.Unknown);

    public static TranslatedString Newline { get; } = new("\n", TranslationResult.Success);

    /// <summary>
    ///     Empty string represented as a successful translation
    /// </summary>
    public static TranslatedString Empty { get; } = new("", TranslationResult.Success);

    public static implicit operator string(TranslatedString translatedString)
    {
        return translatedString.Content;
    }

    public static TranslatedString operator +(TranslatedString a, TranslatedString b)
    {
        return new TranslatedString(a.Content + b.Content, TranslationResult.Success);
    }

    public override string ToString()
    {
        // This must just return Content otherwise it will do the wrong thin in interpolated strings
        return Content;
    }

    public static TranslatedString FromString(string content)
    {
        return new TranslatedString(content, TranslationResult.Success);
    }
}

public enum TranslationResult
{
    /// <summary>
    ///     All's well, enjoy your string!
    /// </summary>
    Success = 0,

    /// <summary>
    ///     IdTable doesn't have this id
    /// </summary>
    NoSlug = 1,

    /// <summary>
    ///     IdTable has the string, but neither the requested locale nor the fallback have translations for it.
    /// </summary>
    NoTranslations = 2,

    /// <summary>
    ///     Should only be used internally for TranslatedString.Unknown
    /// </summary>
    Unknown = 3,

    /// <summary>
    ///     The requested locale does not have this string, but the fallback does.
    /// </summary>
    UsedFallback = 4
}