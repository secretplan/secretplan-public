using DATA_ASSEMBLY.DistributableConfig;
using SecretPlan.Generated;
using SecretPlanGodot.Core;

namespace DATA_ASSEMBLY.Distributable;

public static class SerializationConstants
{
    private static TranslatedString TranslatedFromId(LocalizationTableIds tableId, LocaleEnum locale,
        params object[] templateArguments)
    {
        return LocalizationServer.Instance.GetReferenceFromId((uint)tableId).Translated(locale, templateArguments);
    }

    private static TranslatedString TranslatedFromStringReference(LocalizedStringReference localeStringReference,
        LocaleEnum locale, params object[] templateArguments)
    {
        return localeStringReference.Translated(locale, templateArguments);
    }

    public static TranslatedString Translated(string slug, LocaleEnum locale, params object[] templateArguments)
    {
        return LocalizationServer.Instance.GetReferenceFromSlug(slug).Translated(locale, templateArguments);
    }
}