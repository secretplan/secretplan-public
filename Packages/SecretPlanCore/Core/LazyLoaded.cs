namespace SecretPlanCore.Core;

public class LazyLoaded<T>
{
    private readonly Func<T> _createValue;
    private T? _value;

    public LazyLoaded(Func<T> createValue)
    {
        _createValue = createValue;
    }

    public T Get()
    {
        return _value ??= _createValue();
    }
}