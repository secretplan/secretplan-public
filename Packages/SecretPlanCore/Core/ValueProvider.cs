namespace SecretPlanCore.Core;

public class ValueProvider<T>
{
    private readonly T _startingValue;
    private Func<T>? _providerFunction;

    public ValueProvider(T startingValue)
    {
        _startingValue = startingValue;
    }

    public ValueProvider(Func<T> providerFunction)
    {
        _providerFunction = providerFunction;
        _startingValue = default!;
    }

    public T Value => _providerFunction != null ? _providerFunction() : _startingValue;

    public void SetProvider(Func<T> func)
    {
        _providerFunction = func;
    }

    public static implicit operator T(ValueProvider<T> self)
    {
        return self.Value;
    }
}