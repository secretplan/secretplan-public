using SecretPlanGodot.Core;
using SecretPlanGodot.Serialization;

namespace DATA_ASSEMBLY.Distributable;

public static class SaveFileMigration
{
    public static Settings LoadSettings()
    {
        var settings =
            GeneralUtilities.LoadJsonOrDefault<Settings>(CommonSerializationConstants.SettingsPath,
                SetupDefaultSettings);
        settings.CacheDefaultAndLoadKeybinds();

        UpgradeSettingsIfNeeded(settings);

        return settings;
    }

    private static void UpgradeSettingsIfNeeded(Settings settings)
    {
        if (settings.Version == SettingsVersion.Unknown)
        {
            settings.Version = SettingsVersion.Version1;
        }
    }

    public static SaveFile LoadSave()
    {
        var saveFile =
            GeneralUtilities.LoadJsonOrDefault<SaveFile>(CommonSerializationConstants.SavePath, SetupDefaultSaveFile);

        SaveFileVersion previousSaveFileVersion;

        do
        {
            saveFile = UpgradeSaveFileIfNeeded(saveFile);
            previousSaveFileVersion = saveFile.FormatVersion;
        } while (saveFile.FormatVersion != previousSaveFileVersion);

        saveFile.AfterLoad();

        return saveFile;
    }

    public static void SetupDefaultSaveFile<TSaveFile>(TSaveFile tSettings)
    {
        if (tSettings is SaveFile saveFile)
        {
            // does nothing (for now?)
        }
    }

    public static void SetupDefaultSettings<TSettings>(TSettings tSettings) where TSettings : SettingsBlob
    {
        if (tSettings is Settings settings)
        {
        }
    }

    public static SaveFile UpgradeSaveFileIfNeeded(SaveFile existingSaveFile)
    {
        return existingSaveFile;
    }
}