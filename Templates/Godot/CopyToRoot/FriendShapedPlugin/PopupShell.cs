using Godot;
using SecretPlanGodot.Core;

namespace FriendShapedPlugin;

public partial class PopupShell : Control
{
    private readonly CachedNode<AspectRatioContainer> _aspectContent = new("AspectContent");
    private CachedNode<Control> _scrimRoot = new("ScrimContent");

    public PopupController? Popup { get; private set; }

    public void SetPopup(PopupController popupInstance)
    {
        _aspectContent.Get(this).AddChild(popupInstance);
        Popup = popupInstance;
    }

    public void SetScrim(Control customScrim)
    {
        _scrimRoot.Get(this).QueueFreeAllChildren();
        _scrimRoot.Get(this).AddChild(customScrim);
    }
}