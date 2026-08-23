using Newtonsoft.Json;
using SecretPlanCore.Configuration;

namespace DATA_ASSEMBLY.DistributableConfig;

[SerializedTypeId("LocalizationIdTableExtension")]
public class LocalizationIdTableExtension : Config
{
    [JsonProperty("ids_to_slug")]
    public LocalizationExtensionIdAndSlug[] IdsToSlugs { get; set; } = [];
}