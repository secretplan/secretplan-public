using Godot;
using SecretPlanGodot.Core;

namespace FriendShapedPlugin;

public partial class ControlSizeProxy : Control
{
    [Export]
    private Control? _controlToGetSizeFrom;

    public override Vector2 _GetMinimumSize()
    {
        var answer = _controlToGetSizeFrom?.GetCombinedMinimumSize();

        if (answer == null)
        {
            LocalClient.Error($"Proxy could not get minimum size from {(_controlToGetSizeFrom?.ToString() ?? "null")}");
            return Vector2.Zero;
        }
        
        return answer.Value;
    }
}