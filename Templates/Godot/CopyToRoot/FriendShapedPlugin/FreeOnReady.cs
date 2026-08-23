using Godot;

namespace FriendShapedPlugin;

public partial class FreeOnReady : Node
{
    public override void _Ready()
    {
        QueueFree();
    }
}