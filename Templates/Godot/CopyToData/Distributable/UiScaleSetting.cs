using SecretPlanCore.Core;

namespace DATA_ASSEMBLY.Distributable;

public enum UiScaleSetting
{
    [EnumDescriptiveName("settings.ui_scale.1x")]
    Normal = 0,

    [EnumDescriptiveName("settings.ui_scale.1.5x")]
    OnePointFiveX = 1,

    [EnumDescriptiveName("settings.ui_scale.2x")]
    TwoX = 2,

    [EnumDescriptiveName("settings.ui_scale.2.5x")]
    TwoPointFiveX = 3
}
