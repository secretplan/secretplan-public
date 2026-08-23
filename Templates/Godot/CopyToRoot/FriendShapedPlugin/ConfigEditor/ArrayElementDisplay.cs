using Godot;
using SecretPlanGodot.Core;

namespace FriendShapedPlugin.ConfigEditor;

public partial class ArrayElementDisplay : Control
{
    private readonly CachedNode<Control> _contentRoot = new("Root/EditorContent");
    private readonly CachedNode<ArrayElementControls> _controls = new("Root/ArrayElementControls");

    public ArrayElementControls Controls => _controls.Get(this);

    private Control ContentRoot => _contentRoot.Get(this);

    public void SetEditor(Control editor)
    {
        ContentRoot.QueueFreeAllChildren();
        ContentRoot.AddChild(editor);
    }
}