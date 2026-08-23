using Newtonsoft.Json;
using SecretPlanCore.Configuration;

namespace DATA_ASSEMBLY.DistributableConfig;

[SerializedTypeId("Locale")]
public class Locale : Config
{
    [HideFromConfigEditor]
    [JsonProperty("ids_to_translations")]
    public Dictionary<uint, string> IdsToTranslations = [];

    [JsonProperty("is_debug_only")]
    public bool IsDebugOnly;

    /// <summary>
    ///     The name Steam gives this language, used for selecting default locale (eg: english, schinese, japanese)
    /// </summary>
    [JsonProperty("steam_name")]
    public string SteamName = string.Empty;

    /// <summary>
    ///     Locale code (eg en_US, ja_JP, es_ES)
    /// </summary>
    [JsonProperty("locale_code")]
    public string LocaleCode { get; init; } = string.Empty;

    /// <summary>
    ///     Name that the Locale calls itself (eg: English, 日本語, Español)
    /// </summary>
    [JsonProperty("localized_name")]
    public string LocalizedName { get; init; } = string.Empty;

    [JsonProperty("sort_order")]
    public int SortOrder { get; init; }

    /// <summary>
    ///     Gets the string directly from the language table, not the ideal public API
    /// </summary>
    internal string? GetTranslationFromId(uint id)
    {
        return IdsToTranslations.GetValueOrDefault(id);
    }

    public bool HasKey(uint id)
    {
        return IdsToTranslations.ContainsKey(id);
    }

    public void SetTranslation(uint id, string translation)
    {
        IdsToTranslations[id] = translation;
    }

    public void Remove(uint id)
    {
        IdsToTranslations.Remove(id);
    }

    /// <summary>
    /// Only useful in a debug/tooling context
    /// </summary>
    public IEnumerable<uint> DebugGetAllIds()
    {
        return IdsToTranslations.Keys;
    }
}