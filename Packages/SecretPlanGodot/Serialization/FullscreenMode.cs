using SecretPlanCore.Core;

namespace SecretPlanGodot.Serialization;

public enum FullscreenMode
{
    [EnumDescriptiveName("settings.window_mode.windowed")]
    Windowed,
    
    [EnumDescriptiveName("settings.window_mode.windowed_fullscreen")]
    WindowedFullscreen,
    
    [EnumDescriptiveName("settings.window_mode.fullscreen")]
    Fullscreen
}