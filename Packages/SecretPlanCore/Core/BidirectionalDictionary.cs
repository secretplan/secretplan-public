namespace SecretPlanCore.Core;

public class BidirectionalDictionary<TKey, TValue> 
    where TKey : notnull 
    where TValue : notnull
{
    private readonly Dictionary<TKey, TValue> _forwardTable = new();
    private readonly Dictionary<TValue, TKey> _reverseTable = new();

    public void AddEntry(TKey key, TValue value)
    {
        _forwardTable[key] = value;
        _reverseTable[value] = key;
    }

    public TKey? GetKeyFromValue(TValue value)
    {
        return _reverseTable.GetValueOrDefault(value);
    }

    public TValue? GetValueFromKey(TKey key)
    {
        return _forwardTable.GetValueOrDefault(key);
    }

    public void RemoveEntryFromValue(TValue value)
    {
        if (_reverseTable.ContainsKey(value))
        {
            var key = _reverseTable[value];
            _reverseTable.Remove(value);
            _forwardTable.Remove(key);
        }
    }
    
    public void RemoveEntryFromKey(TKey key)
    {
        if (_forwardTable.ContainsKey(key))
        {
            var value = _forwardTable[key];
            _reverseTable.Remove(value);
            _forwardTable.Remove(key);
        }
    }

    public bool ContainsKey(TKey key)
    {
        return _forwardTable.ContainsKey(key);
    }
    
    public bool ContainsValue(TValue value)
    {
        return _reverseTable.ContainsKey(value);
    }

    public IEnumerable<TValue> Values()
    {
        return _forwardTable.Values;
    }
    
    public IEnumerable<TKey> Keys()
    {
        return _reverseTable.Values;
    }
}