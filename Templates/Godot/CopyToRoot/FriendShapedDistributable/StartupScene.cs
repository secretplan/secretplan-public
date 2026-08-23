using FriendShapedPlugin;
using Godot;
using SecretPlanGodot.Core;

namespace FriendShapedDistributable;

public partial class StartupScene : Node
{
    private readonly ParentCore _parentCore = new();
    private readonly CachedPackedScene<Node> _world = new("res://YOUR_FIRST_SCENE.tscn");
    private CoreState CoreState => _parentCore.State(this);

    public override void _Ready()
    {
        CoreState.LoadScene(_world, node => { });
    }
}