using Godot;
using SecretPlanGodot.Core;

namespace FriendShapedPlugin;

public partial class ClientIdDisplay : RichTextLabel
{
    public override void _Ready()
    {
        MouseFilter = MouseFilterEnum.Ignore;
        SetAnchorsPreset(LayoutPreset.FullRect);
        SetHorizontalAlignment(HorizontalAlignment.Right);
        
        Text = LocalClient.RichTextDescriptor;

        if (!LocalClient.IsDev)
        {
            Hide();
        }

        SetAnchorsPreset(LayoutPreset.FullRect);
    }
}