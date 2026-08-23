using Newtonsoft.Json;
using SecretPlanGodot.Configuration;

namespace DATA_ASSEMBLY.DistributableConfig;

[CustomEditor("res://FriendShapedPlugin/ConfigEditor/LocalizationIdAndTranslationEditor.tscn")]
public record struct LocalizationExtensionIdAndTranslation(
    [property: JsonProperty("id")] uint Id,
    [property: JsonProperty("translation")] string Translation);