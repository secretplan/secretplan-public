using Newtonsoft.Json;
using SecretPlan.Generated;
using SecretPlanGodot.Configuration;
using SecretPlanGodot.Core;

namespace DATA_ASSEMBLY.DistributableConfig;

[CustomEditor("res://FriendShapedPlugin/ConfigEditor/Scenes/LocStringFieldEditor.tscn")]
public record struct LocalizedStringReference([property: JsonProperty("id")] uint Id)
{
    [JsonIgnore]
    public string Slug => LocalizationServer.Instance.GetSlug(Id);

    public static LocalizedStringReference FromSlug(string slug)
    {
        return new LocalizedStringReference(LocalizationServer.Instance.GetId(slug));
    }

    public TranslatedString Translated(LocaleEnum locale, params object[] templateArguments)
    {
        return LocalizationServer.Instance.GetTranslatedStringFromId(Id, locale, templateArguments);
    }

    public TranslatedString TranslatedWithFallbackLocale()
    {
        return LocalizationServer.Instance.GetTranslatedStringFromId(Id, LocalizationServer.Instance.FallbackLocale);
    }

    public bool IsEmpty()
    {
        return Id == 0;
    }
}