using System;
using DATA_ASSEMBLY.Distributable;
using SecretPlanGodot.Core;
using SecretPlanGodot.Serialization;

namespace FriendShapedDistributable;

public static class UserDataMigration
{
    public static SaveFile UpgradeSaveFileIfNeeded(SaveFile existingSaveFile)
    {
        return existingSaveFile;
    }

    public static Settings LoadSettings()
    {
        var settings =
            GeneralUtilities.LoadJsonOrDefault<Settings>(CommonSerializationConstants.SettingsPath, SetupDefaultSettings);
        settings.CacheDefaultAndLoadKeybinds();

        if (settings.Version == SettingsVersion.Unknown)
        {
        }

        return settings;
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

    public static void SetupDefaultSaveFile(SaveFile saveFile)
    {
        // does nothing (for now)
    }

    public static void SetupDefaultSettings(Settings settings)
    {
        // does nothing (for now)
    }
}