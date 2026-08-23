using Newtonsoft.Json;
using SecretPlanCore.Core;

namespace DATA_ASSEMBLY.Distributable;

public class SaveFile : BaseSaveFile
{
    [JsonProperty("format_version")]
    public SaveFileVersion FormatVersion { get; set; } = SaveFileVersion.FirstVersion;


    public void DeleteEverything()
    {
        // clear all save data
    }
    
    /// <summary>
    /// Runs after the save file is loaded every time
    /// </summary>
    public void AfterLoad()
    {
    }
}