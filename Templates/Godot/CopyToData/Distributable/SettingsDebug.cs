using Newtonsoft.Json;
using SecretPlanGodot.Core;

namespace DATA_ASSEMBLY.Distributable;

public class SettingsDebug : SecretPlanDebugger
{
    [JsonProperty("is_dev")]
    [DebugValue("dev")]
    public bool IsDev { get; set; }
}