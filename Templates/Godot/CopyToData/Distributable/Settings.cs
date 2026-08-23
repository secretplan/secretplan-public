using Godot;
using JetBrains.Annotations;
using Newtonsoft.Json;
using SecretPlan.Generated;
using SecretPlanCore.Core;
using SecretPlanGodot.Core;
using SecretPlanGodot.Serialization;

namespace DATA_ASSEMBLY.Distributable;

public record Settings : SettingsBlob
{
    [JsonIgnore]
    private readonly Dictionary<string, List<SerializedInputEvent>> _defaultBindings = new();

    [JsonProperty("action_to_keybinds_v2")]
    private Dictionary<string, List<SerializedInputEvent>?> _actionToKeybinds = new();

    /// <summary>
    ///     This gets set to true after the game has selected a default locale.
    /// </summary>
    [JsonProperty("has_chosen_locale")]
    public bool HasChosenLocale;

    [JsonProperty("version")]
    public SettingsVersion Version { get; set; } = SettingsVersion.Version1;

    [JsonProperty("language")]
    [DynamicSetting(SettingsCategoryType.General, LocalizationTableIds.settings__general__language)]
    public LocaleEnum Locale { get; set; } = LocaleEnum.English;

    [JsonProperty("window_mode")]
    [DynamicSetting(SettingsCategoryType.Graphics, LocalizationTableIds.settings__graphics__window_mode)]
    public FullscreenMode WindowMode { get; set; } = FullscreenMode.WindowedFullscreen;

    [JsonProperty("viewport_scale")]
    [SettingsRange(Min = 0.2f, Max = 1, Step = 0.1f)]
    [DynamicSetting(SettingsCategoryType.Graphics, LocalizationTableIds.settings__graphics__resolution,
        LocalizationTableIds.settings__graphics__resolution__description)]
    public float ViewportScale { get; set; } = 1f;

    [JsonProperty("fov")]
    [DynamicSetting(SettingsCategoryType.Graphics, LocalizationTableIds.settings__graphics__fov)]
    [SettingsRange(Min = 60, Max = 110, Step = 1, RoundToWholeNumbers = true)]
    public float Fov { get; set; } = 90f;

    [JsonProperty("framerate_cap")]
    [DynamicSetting(SettingsCategoryType.Graphics, LocalizationTableIds.settings__graphics__framerate_cap)]
    [SettingsRange(Min = 30, Max = 240, Step = 1, RoundToWholeNumbers = true)]
    public float FramerateCap { get; set; } = 200;

    [JsonProperty("sensitivity")]
    [DynamicSetting(SettingsCategoryType.General, LocalizationTableIds.settings__general__look_sensitivity)]
    [SettingsRange(Min = 0.1f, Max = 5f, Step = 0.05f)]
    public float LookSensitivity { get; set; } = 1f;

    [JsonProperty("invert_x")]
    [DynamicSetting(SettingsCategoryType.General, LocalizationTableIds.settings__general__invert_horizontal)]
    public bool InvertX { get; set; }

    [JsonProperty("invert_y")]
    [DynamicSetting(SettingsCategoryType.General, LocalizationTableIds.settings__general__invert_vertical)]
    public bool InvertY { get; set; }

    [JsonProperty("vsync")]
    [DynamicSetting(SettingsCategoryType.Graphics, LocalizationTableIds.settings__graphics__vsync)]
    public bool VSyncEnabled { get; set; } = true;

    [JsonProperty("master_volume")]
    [DynamicSetting(SettingsCategoryType.Audio, LocalizationTableIds.settings__sound__master_volume)]
    [SettingsRange(Min = 0, Max = 1, Step = 0.01f)]
    public float MasterVolume { get; set; } = 0.5f;

    [JsonProperty("music_volume")]
    [DynamicSetting(SettingsCategoryType.Audio, LocalizationTableIds.settings__sound__music_volume)]
    [SettingsRange(Min = 0, Max = 1, Step = 0.01f)]
    public float MusicVolume { get; set; } = 0.5f;

    [JsonProperty("effects_volume")]
    [DynamicSetting(SettingsCategoryType.Audio, LocalizationTableIds.settings__sound__sfx_volume)]
    [SettingsRange(Min = 0, Max = 1, Step = 0.01f)]
    public float EffectsVolume { get; set; } = 0.5f;

    [JsonIgnore]
    public bool RefreshRequested { get; set; }

    [JsonProperty("allow_telemetry")]
    [DynamicSetting(SettingsCategoryType.General, LocalizationTableIds.settings__general__send_anonymous_data)]
    public bool AllowTelemetry { get; set; }

    [JsonProperty("physics_interpolation")]
    [DynamicSetting(SettingsCategoryType.General, LocalizationTableIds.settings__general__physics_interpolation)]
    public bool EnablePhysicsInterpolation { get; set; }

    [JsonProperty("allow_console")]
    [DynamicSetting(SettingsCategoryType.General, LocalizationTableIds.settings__general__allow_console)]
    public bool AllowConsole { get; set; }

    [JsonProperty("ui_scale")]
    [DynamicSetting(SettingsCategoryType.General, LocalizationTableIds.settings__ui_scale__name)]
    public UiScaleSetting UiScale { get; set; }

    [JsonProperty("debug")]
    public SettingsDebug Debug { get; set; } = new();

    [JsonProperty("draw_distance")]
    [SettingsRange(Min = 10f, Max = 1500f, Step = 50, RoundToWholeNumbers = true)]
    [DynamicSetting(SettingsCategoryType.Graphics, LocalizationTableIds.settings__draw_distance)]
    public float DrawDistance { get; set; } = 1500f;

    [JsonProperty("show_framerate")]
    [DynamicSetting(SettingsCategoryType.Graphics, LocalizationTableIds.settings__graphics__show_framerate_counter)]
    public bool ShowFramerateCounter { get; set; }

    [JsonProperty("brightness")]
    [SettingsRange(Min = 0.1f, Max = 2, Step = 0.01f)]
    [DynamicSetting(SettingsCategoryType.Graphics, LocalizationTableIds.settings__graphics__brightness)]
    public float Brightness { get; set; } = 1f;

    [JsonProperty("gc_every_frame")]
    [DynamicSetting(SettingsCategoryType.General,
        LocalizationTableIds.settings__general__gc_every_frame__name,
        LocalizationTableIds.settings__general__gc_every_frame__description)]
    public bool GarbageCollectEveryFrame { get; set; }

    [DynamicSetting(SettingsCategoryType.Data, LocalizationTableIds.settings__get_logs,
        LocalizationTableIds.settings__get_logs_description)]
    [UsedImplicitly]
    public void GetLogs()
    {
        GetLogsRequested?.Invoke();
    }

    public event Action? GetLogsRequested;

    [DynamicSetting(SettingsCategoryType.Data, LocalizationTableIds.settings__open_save_data)]
    [UsedImplicitly]
    public void OpenSaveData()
    {
        OpenSaveDataRequested?.Invoke();
    }

    public event Action? OpenSaveDataRequested;

    [DynamicSetting(SettingsCategoryType.Data,
        LocalizationTableIds.pause_menu__save_data__delete_everything__name,
        LocalizationTableIds.pause_menu__save_data__delete_everything__description)]
    [UsedImplicitly]
    public void ClearSaveData()
    {
        SaveDataActionRequested?.Invoke(save => { save.DeleteEverything(); },
            LocalizationTableIds.pause_menu__save_data__delete_everything__description);
    }

    public event Action<Action<SaveFile>, LocalizationTableIds>? SaveDataActionRequested;

    [DynamicSetting(SettingsCategoryType.Keybinds, LocalizationTableIds.settings__keybinds__reset_to_default)]
    [UsedImplicitly]
    public void ResetKeybindsToDefault()
    {
        foreach (var (action, serializedInputEvent) in _defaultBindings)
        {
            AddKeybindsBulk(action, serializedInputEvent);
        }
    }

    private void AddKeybindsBulk(string action, List<SerializedInputEvent> serializedEvents)
    {
        LocalClient.Print("Bulk assigning keybinds");
        GodotFullyUnbindAction(action);

        // Remove all existing bindings for that action
        _actionToKeybinds.GetValueOrDefault(action)?.Clear();

        foreach (var serializedEvent in serializedEvents)
        {
            AddKeybind(action, serializedEvent, true);
        }

        KeybindChanged?.Invoke(action);
    }

    private void GodotUnbindSpecific(string actionName, InputEvent savedEvent)
    {
        InputMap.ActionEraseEvent(actionName, savedEvent);
    }

    public void AddKeybind(string action, SerializedInputEvent serializedEvent, bool skipEvent = false)
    {
        var list = GetInputListForAction(action);

        if (list.Contains(serializedEvent))
        {
            LocalClient.Print("Attempted to add the same binding twice, ignoring");
            return;
        }

        list.Add(serializedEvent);

        var deserialized = serializedEvent.DeserializeToEvent();
        if (deserialized == null)
        {
            LocalClient.Error($"Could not deserialize {serializedEvent}");
            return;
        }

        GodotAddKeybind(action, deserialized);

        if (!skipEvent)
        {
            KeybindChanged?.Invoke(action);
        }
    }

    private List<SerializedInputEvent> GetInputListForAction(string action)
    {
        var list = _actionToKeybinds.GetValueOrDefault(action) ?? [];

        // Add the list to the dictionary (assuming we just generated it above)
        _actionToKeybinds[action] = list;
        return list;
    }

    public void RemoveSpecificKeybind(string action, InputEvent inputEvent)
    {
        LocalClient.Print($"Removing keybind {action} {inputEvent.AsText()}");
        GodotUnbindSpecific(action, inputEvent);
        var list = GetInputListForAction(action);
        var serialized = SerializedInputEvent.FromInputEvent(inputEvent, true);

        if (!list.Remove(serialized))
        {
            LocalClient.Error($"Tried to remove {serialized} from {action} but didn't find a match");
        }

        GodotRemoveKeybind(action, inputEvent);

        KeybindChanged?.Invoke(action);
    }

    private static void GodotRemoveKeybind(string actionName, InputEvent inputEvent)
    {
        if (!InputMap.HasAction(actionName))
        {
            LocalClient.Print($"Skipped assigning <{actionName}>, it doesn't exist)");
            return;
        }

        LocalClient.Print($"Remove Binding: <{actionName}> -> {inputEvent.AsText()}");
        InputMap.ActionEraseEvent(actionName, inputEvent);
    }

    /// <summary>
    ///     Look Sensitivity multiplied by a factor that makes sense for mouse
    /// </summary>
    public float CalculateMouseSensitivity()
    {
        return GetSensitivity() * 0.002f;
    }

    /// <summary>
    ///     Look Sensitivity multiplied by a factor that makes sense for thumbstick
    /// </summary>
    public float CalculateThumbstickLookSensitivity()
    {
        return GetSensitivity() * 0.03f;
    }

    private float GetSensitivity()
    {
        return LookSensitivity;
    }

    public IEnumerable<SerializedInputEvent> GetKeybindsForAction(string action)
    {
        foreach (var inputEvent in InputUtilities.GetInputEventsForAction(action))
        {
            yield return SerializedInputEvent.FromInputEvent(inputEvent, true);
        }
    }

    public event Action<string>? KeybindChanged;

    public void InvokeLocaleChanged()
    {
        LocaleChanged?.Invoke();
    }

    public event Action? LocaleChanged;

    public void ChooseDefaultLocale(LocaleEnum localeEnum)
    {
        if (HasChosenLocale)
        {
            LocalClient.Print($"Skipping locale selection, client already has selection: {Locale}");
            // This client already found a preferred locale (or the player set one)
            return;
        }

        LocalClient.Print($"Chose default locale: {localeEnum}");
        Locale = localeEnum;

        HasChosenLocale = true;

        InvokeValueChangedForSetting(nameof(Locale));
    }

    public Vector2 ViewportScaleVector()
    {
        return new Vector2(ViewportScale, ViewportScale);
    }

    public void InvokeChangedForAllSettings()
    {
        foreach (var setting in
                 Reflection.GetAllMembersInTypeWithAttribute<DynamicSettingAttribute>(typeof(Settings)))
        {
            InvokeValueChangedForSetting(setting.Name);
        }
    }

    public void CacheDefaultAndLoadKeybinds()
    {
        CacheCurrentKeybindsAsDefault();
        PopulateSavedBindingsIfEmpty();
        SetGodotKeybindsToSavedBindings();
    }

    private void PopulateSavedBindingsIfEmpty()
    {
        // User has NO bindings set, pre-populate with defaults
        if (_actionToKeybinds.Count == 0)
        {
            foreach (var (action, events) in _defaultBindings)
            {
                // make sure we COPY the list, not re-reference it
                _actionToKeybinds.Add(action, events.ToList());
            }
        }
    }

    private void CacheCurrentKeybindsAsDefault()
    {
        _defaultBindings.Clear();

        foreach (var (actionName, inputEvents) in InputUtilities.AllBoundActionsAndEvents())
        {
            var serializedEvents = inputEvents.Select(a => SerializedInputEvent.FromInputEvent(a, true)).ToList();
            serializedEvents.RemoveAll(a => a.Type == SerializedInputEventType.Unbound);
            _defaultBindings[actionName] = serializedEvents;
        }
    }

    private void SetGodotKeybindsToSavedBindings()
    {
        LocalClient.Print("Loading Keybinds from save file");

        foreach (var (actionName, inputEvents) in _actionToKeybinds)
        {
            GodotFullyUnbindAction(actionName);

            foreach (var inputEvent in inputEvents?.Select(a => a.DeserializeToEvent()) ?? [])
            {
                if (inputEvent != null)
                {
                    GodotAddKeybind(actionName, inputEvent);
                }
            }
        }
    }

    private static void GodotFullyUnbindAction(string action)
    {
        InputMap.ActionEraseEvents(action);
    }

    /// <summary>
    ///     Sets the keybind in Godot Land
    /// </summary>
    private static void GodotAddKeybind(string actionName, InputEvent inputEvent)
    {
        if (!InputMap.HasAction(actionName))
        {
            LocalClient.Print($"Skipped assigning <{actionName}>, it doesn't exist)");
            return;
        }

        // LocalClient.Print($"Add Binding: <{actionName}> -> {inputEvent.AsText()}");
        InputMap.ActionAddEvent(actionName, inputEvent);
    }

    public float UiScaleValue()
    {
        switch (UiScale)
        {
            default:
            case UiScaleSetting.Normal:
                return 1f;
            case UiScaleSetting.OnePointFiveX:
                return 1.5f;
            case UiScaleSetting.TwoX:
                return 2f;
            case UiScaleSetting.TwoPointFiveX:
                return 2.5f;
        }
    }
}
