using Godot;
using SecretPlanGodot.ConfigEditor;

namespace FriendShapedPlugin.ConfigEditor;

public abstract partial class FieldEditor : Control
{
    public abstract void Initialize(ConfigField configField);
}