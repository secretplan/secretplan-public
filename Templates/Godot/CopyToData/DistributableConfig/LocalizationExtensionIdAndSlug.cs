using Newtonsoft.Json;
using SecretPlanGodot.Configuration;

namespace DATA_ASSEMBLY.DistributableConfig;

[CustomEditor("res://FriendShapedPlugin/ConfigEditor/Scenes/LocalizationIdAndSlugEditor.tscn")]
public record struct LocalizationExtensionIdAndSlug(
    [property: JsonProperty("id")] uint Id,
    [property: JsonProperty("string")] string Slug);