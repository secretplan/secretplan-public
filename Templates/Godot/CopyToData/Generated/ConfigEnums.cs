// !! This is NOT generated !!
// This is a stub file that will be replaced by a generated file the first time config catalogue is run
// It is intentionally a very minimal stub, just enough to get the data assembly to compile so it can run real codegen

using DATA_ASSEMBLY.DistributableConfig;

namespace SecretPlan.Generated;

public enum LocalizationTableIds
{
    None,
    pause_menu__save_data__delete_everything__description,
    settings__keybinds__reset_to_default,
    settings__general__language,
    settings__graphics__window_mode,
    settings__graphics__resolution,
    settings__graphics__resolution__description,
    settings__graphics__fov,
    settings__graphics__framerate_cap,
    settings__general__look_sensitivity,
    settings__general__invert_horizontal,
    settings__general__invert_vertical,
    settings__graphics__vsync,
    settings__sound__master_volume,
    settings__sound__music_volume,
    settings__sound__sfx_volume,
    settings__general__send_anonymous_data,
    settings__general__physics_interpolation,
    settings__general__allow_console,
    settings__ui_scale__name,
    settings__draw_distance,
    settings__graphics__show_framerate_counter,
    settings__graphics__brightness,
    settings__general__gc_every_frame__name,
    settings__general__gc_every_frame__description,
    settings__get_logs_description,
    settings__get_logs,
    settings__open_save_data,
    pause_menu__save_data__delete_everything__name,
    settings__off
}

public enum LocalizationRootTableEnum : System.UInt32
{
    LocalizationTable = 3638375294,
    None = 0,
}

public enum LocaleEnum : System.UInt32
{
    English = 442761519,
    None = 0,
    RandomLocale = 12116873,
}

public enum LocalizationIdTableEnum : System.UInt32
{
    IdTable = 1863813702,
    None = 0,
}

public static class EnumExtensions
{
    public static LocalizationRootTable ReadOrDefault(this LocalizationRootTableEnum self)
    {
        var valueAsInt = (System.UInt32) self;
        var value = SecretPlanCore.Configuration.ConfigServer.Instance.GetInstance<DATA_ASSEMBLY.DistributableConfig.LocalizationRootTable>(valueAsInt);
        if (value == null)
        {
            var result = new DATA_ASSEMBLY.DistributableConfig.LocalizationRootTable();
            result.InstanceInfo = result.InstanceInfo with { InstanceId = 0 };
            return result;
        }
        return value;
    }
    
    public static Locale ReadOrDefault(this LocaleEnum self)
    {
        var valueAsInt = (System.UInt32) self;
        var value = SecretPlanCore.Configuration.ConfigServer.Instance.GetInstance<DATA_ASSEMBLY.DistributableConfig.Locale>(valueAsInt);
        if (value == null)
        {
            var result = new DATA_ASSEMBLY.DistributableConfig.Locale();
            result.InstanceInfo = result.InstanceInfo with { InstanceId = 0 };
            return result;
        }
        return value;
    }

    public static LocalizationIdTable ReadOrDefault(this LocalizationIdTableEnum self)
    {
        var valueAsInt = (System.UInt32) self;
        var value = SecretPlanCore.Configuration.ConfigServer.Instance.GetInstance<DATA_ASSEMBLY.DistributableConfig.LocalizationIdTable>(valueAsInt);
        if (value == null)
        {
            var result = new DATA_ASSEMBLY.DistributableConfig.LocalizationIdTable();
            result.InstanceInfo = result.InstanceInfo with { InstanceId = 0 };
            return result;
        }
        return value;
    }

    public static LocaleEnum UidAsEnum(this Locale self)
    {
        return (LocaleEnum) self.Uid();
    }
}