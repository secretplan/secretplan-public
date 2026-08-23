using DATA_ASSEMBLY.Distributable;
using FriendShapedDistributable;
using Godot;

namespace FriendShapedPlugin;

public partial class FramerateDisplay : RichTextLabel
{
    private readonly ParentCore _core = new();
    private CoreState CoreState => _core.State(this);

    public override void _Ready()
    {
        MouseFilter = MouseFilterEnum.Ignore;
        SetAnchorsPreset(LayoutPreset.FullRect);
        SetHorizontalAlignment(HorizontalAlignment.Left);
        
        CoreState.SerializedState.Settings.ValueChanged += SettingsChanged;
        UpdateVisibility();
    }

    private void SettingsChanged(string settingName)
    {
        if (settingName == nameof(Settings.ShowFramerateCounter))
        {
            UpdateVisibility();
        }
    }

    private void UpdateVisibility()
    {
        Visible = CoreState.SerializedState.Settings.ShowFramerateCounter;
    }

    public override void _Process(double delta)
    {
        var immediateFramerate = (int)(1 / delta);
        Text = immediateFramerate.ToString();
    }
}