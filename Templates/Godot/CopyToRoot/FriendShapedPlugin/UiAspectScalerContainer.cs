using System;
using DATA_ASSEMBLY.Distributable;
using FriendShapedDistributable;
using Godot;

namespace FriendShapedPlugin;

public partial class UiAspectScalerContainer : AspectRatioContainer
{
    private float _time;

    private ParentCore _parentCore = new();
    private CoreState CoreState => _parentCore.State(this);
    
    public override void _EnterTree()
    {
        CoreState.SerializedState.Settings.ValueChanged += OnSettingChanged;
        SetScaleToSettings();
    }
    
    public override void _ExitTree()
    {
        CoreState.SerializedState.Settings.ValueChanged -= OnSettingChanged;
    }

    private void OnSettingChanged(string settingName)
    {
        if (settingName == nameof(Settings.UiScale))
        {
            SetScaleToSettings();
        }
    }

    private void SetScaleToSettings()
    {
        SetScale(CoreState.SerializedState.Settings.UiScaleValue());
    }

    public void SetScale(float scale)
    {
        var inverseScale = 1f / scale;

        AnchorTop = 0;
        AnchorLeft = 0;
        AnchorRight = inverseScale;
        AnchorBottom = inverseScale;

        OffsetTop = 0;
        OffsetLeft = 0;
        OffsetBottom = 0;
        OffsetRight = 0;

        Scale = new Vector2(scale, scale);
    }
}
