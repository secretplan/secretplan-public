using SecretPlanCore.Core;
using SecretPlanGodot.Core;

namespace FriendShapedPlugin.ConfigEditor;

public partial class NumberEditor : FieldEditorText
{
    protected override bool IsValidText(string newText)
    {
        if (ConfigField == null)
        {
            return false;
        }
        
        if (NumberUtilities.TryParseNumeric(newText, ConfigField.AssociatedType, out var result))
        {
            return true;
        }

        return false;
    }

    protected override void OnSubmitted(string newText)
    {
        if (ConfigField == null)
        {
            LocalClient.Error("Attempted to submit NumberEditor with missing config field");
            return;
        }
        
        if (NumberUtilities.TryParseNumeric(newText, ConfigField.AssociatedType, out var result))
        {
            ConfigField.SetValue(result);
        }
    }

    protected override void Initialize2()
    {
    }
}