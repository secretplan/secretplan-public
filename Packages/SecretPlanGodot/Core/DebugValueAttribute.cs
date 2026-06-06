namespace SecretPlanGodot.Core;

public class DebugValueAttribute : Attribute
{
    public DebugValueAttribute(string invokeWord)
    {
        InvokeWord = invokeWord;
    }

    public string InvokeWord { get; }
}