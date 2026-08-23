namespace SecretPlanGodot.ConfigEditor;

public class ConfigFieldChangeNotifier
{
    private readonly Action _onChanged;

    public ConfigFieldChangeNotifier(Action onChanged)
    {
        _onChanged = onChanged;
    }

    public void NotifyChanged()
    {
        _onChanged();
    }
}