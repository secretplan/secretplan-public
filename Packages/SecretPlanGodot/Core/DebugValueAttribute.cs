namespace SecretPlanGodot.Core;

public class DebugValueAttribute : Attribute
{
    public DebugValueAttribute(string invokeWord, bool isHidden = false)
    {
        InvokeWord = invokeWord;
        IsHidden = isHidden;
    }

    public bool IsHidden { get; set; }

    public string InvokeWord { get; }
}