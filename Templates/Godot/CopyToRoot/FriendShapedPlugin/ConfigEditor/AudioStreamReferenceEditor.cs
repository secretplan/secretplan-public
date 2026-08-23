using Godot;
using SecretPlanGodot.Core;

namespace FriendShapedPlugin.ConfigEditor;

public partial class AudioStreamReferenceEditor : ResourceReferenceEditor<AudioStream>
{
    private readonly CachedNode<Button> _playButton = new("AudioStreamReferenceEditor/PlayButton");
    private readonly CachedNode<AudioStreamPlayer> _player = new("Player");

    private Button Button => _playButton.Get(this);

    private AudioStreamPlayer Player => _player.Get(this);

    protected override void Refresh()
    {
        var stream = GetCurrentValue()?.CachedResource.GetOrLoadOrNull();

        Button.Disabled = stream == null;
        
        Player.Stream = stream;
    }

    public override void _EnterTree()
    {
        Button.Pressed += ButtonPressed;
    }

    public override void _ExitTree()
    {
        Button.Pressed -= ButtonPressed;
    }

    private void ButtonPressed()
    {
        Player.Stop();
        Player.Play();
    }
}