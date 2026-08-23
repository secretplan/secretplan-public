using Newtonsoft.Json;
using SecretPlanCore.Configuration;

namespace DATA_ASSEMBLY.DistributableConfig;

[SerializedTypeId("Achievement")]
public class AchievementConfig : Config
{
    [JsonProperty("steam_api_name")]
    public string SteamApiName { get; set; } = "";

    [JsonProperty("steam_loc_id")]
    public string SteamLocId { get; set; } = "";
    
    [JsonProperty("localized_name")]
    public LocalizedStringReference Name { get; set; }
    
    [JsonProperty("localized_description")]
    public LocalizedStringReference Description { get; set; }

    public string NameVdfKey()
    {
        return SteamLocId + "_NAME";
    }
    
    public string DescriptionVdfKey()
    {
        return SteamLocId + "_DESC";
    }
}