namespace SecretPlanCore.Core;

public static class DictionaryExtensions
{
    /// <summary>
    ///     Attempts to get key if it exists. If it does not exist, creates assigns key to createValue() and
    ///     then returns created value
    /// </summary>
    /// <returns></returns>
    public static TValue GetOrCreateValue<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key,
        Func<TValue> createValue) where TKey : notnull
    {
        if (!dictionary.TryGetValue(key, out var value))
        {
            value = createValue();
            dictionary[key] = value;
        }

        return value;
    }
}