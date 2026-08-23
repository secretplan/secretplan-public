using BirdGame.UI;
using Godot;
using SecretPlanGodot.Navigation;

namespace FriendShapedDistributable;

public partial class FocusSink : Control, IFocusSink
{
    public Control? GetDefaultFocusNode()
    {
        return this;
    }

    public override void _Ready()
    {
        FocusMode = FocusModeEnum.All;
        MouseFilter = MouseFilterEnum.Ignore;
    }
}