using Newtonsoft.Json;
using SecretPlanCore.Configuration;

namespace DATA_ASSEMBLY.DistributableConfig;

[SerializedTypeId("Dlc")]
public class DlcConfig : Config
{
    [JsonProperty("app_id")]
    public uint AppId { get; set; }
    
    [JsonProperty("name")]
    public LocalizedStringReference DlcNameLocString { get; set; }
}