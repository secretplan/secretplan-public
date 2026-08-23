using Newtonsoft.Json;
using SecretPlan.Generated;
using SecretPlanCore.Configuration;

namespace DATA_ASSEMBLY.DistributableConfig;

[SerializedTypeId("LocaleExtension")]
public class LocaleExtension : Config
{
    [JsonProperty("source_language")]
    public LocaleEnum SourceLocale { get; init; }

    [JsonProperty("ids_to_translations")]
    public LocalizationExtensionIdAndTranslation[] IdsToTranslations { get; init; } = [];
}