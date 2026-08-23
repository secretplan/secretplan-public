using Godot;
using SecretPlanGodot.ConfigEditor;
using SecretPlanGodot.Core;

namespace FriendShapedPlugin.ConfigEditor;

public partial class ObjectEditor : FieldEditor
{
    private readonly CachedNode<Control> _content = new("ObjectContent");

    /// <summary>
    ///     Represents the whole object (eg: a Vector3 whose subfields are X,Y,Z)
    /// </summary>
    private ConfigField? _objectField;

    private Control Content => _content.Get(this);

    public override void Initialize(ConfigField objectField)
    {
        _objectField = objectField;
        BuildDisplaysForAllFields();
    }

    private void BuildDisplaysForAllFields()
    {
        Content.QueueFreeAllChildren();

        if (_objectField == null)
        {
            LocalClient.Error("ObjectEditor was not initialized!");
            return;
        }
        
        // Subfields are the fields underneath this object (eg: X, Y, and Z of a vector)
        foreach (var subField in _objectField.GetSubfields())
        {
            var fieldDisplay =
                GodotUtilities.InstantiateScene<NamedFieldDisplay>("res://FriendShapedPlugin/ConfigEditor/Scenes/NamedFieldVertical.tscn");
            fieldDisplay.Initialize(subField.HumanReadableName);
            fieldDisplay.AddEditor(ConfigEditor.CreateEditorForConfigField(subField));
            Content.AddChild(fieldDisplay);
        }
    }
}