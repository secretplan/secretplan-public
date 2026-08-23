using SecretPlanGodot.Core;

namespace FriendShapedPlugin.ConfigEditor;

public partial class StringEditor : FieldEditorText
{
    protected override bool IsValidText(string newText)
    {
        return true;
    }

    protected override void OnSubmitted(string newText)
    {
        if (ConfigField == null)
        {
            LocalClient.Error("Attempted to submit StringEditor with missing config field");
            return;
        }
        
        ConfigField.SetValue(newText);
    }

    protected override void Initialize2()
    {
    }
}