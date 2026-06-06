namespace SecretPlanCore.ArgumentParsing;

[AttributeUsage(AttributeTargets.Property)]
public class ArgumentAttribute(string key) : Attribute
{
    public string Key { get; } = key;
}