using Godot;
using SecretPlanGodot.Core;

namespace FriendShapedPlugin.ConfigEditor;

public partial class NamedFieldDisplay : Control
{
    private readonly CachedNode<Control> _contentRoot = new("Root/EditorContent");
    private readonly CachedNode<Label> _fieldName = new("Root/FieldName");

    public void Initialize(string fieldName)
    {
        _fieldName.Get(this).Text = fieldName;
        _contentRoot.Get(this).QueueFreeAllChildren();
    }

    public void AddEditor(Control editor)
    {
        _contentRoot.Get(this).AddChild(editor);
    }
}