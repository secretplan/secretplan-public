using SecretPlan.Generated;
using SecretPlanCore.Configuration;
using SecretPlanCore.Core;
using SecretPlanGodot.Core;

namespace DATA_ASSEMBLY.DistributableConfig;


public class LocalizationServer
{
    private readonly BidirectionalDictionary<uint, string> _idsAndSlugs = new();
    private readonly Dictionary<LocaleEnum, Dictionary<uint, string>> _translations = new();

    private LocalizationServer()
    {
        var rootTable = LocalizationRootTableEnum.LocalizationTable.ReadOrDefault();
        FallbackLocale = rootTable.FallbackLocale;

        var rootIdTable = rootTable.IdTable.ReadOrDefault();

        foreach (var (id, slug) in rootIdTable.AllIdsAndSlugs())
        {
            _idsAndSlugs.AddEntry(id, slug);
        }

        foreach (var idTable in ConfigServer.Instance.GetAllInstancesOfType<LocalizationIdTableExtension>())
        {
            foreach (var (id, slug) in idTable.IdsToSlugs)
            {
                _idsAndSlugs.AddEntry(id, slug);
            }
        }

        foreach (var locale in ConfigServer.Instance.GetAllInstancesOfType<Locale>())
        {
            var localeEnum = locale.UidAsEnum();
            if (!_translations.ContainsKey(localeEnum))
            {
                _translations[localeEnum] = new Dictionary<uint, string>();
            }
            
            foreach (var (id, translation) in locale.IdsToTranslations)
            {
                _translations[localeEnum][id] = translation;
            }
        }
        
        foreach (var extensionTable in ConfigServer.Instance.GetAllInstancesOfType<LocaleExtension>())
        {
            foreach (var (id, translation) in extensionTable.IdsToTranslations)
            {
                _translations[extensionTable.SourceLocale][id] = translation;
            }
        }
    }

    public static LocalizationServer Instance { get; private set; } = new();

    public LocaleEnum FallbackLocale { get; }

    public IEnumerable<LocaleEnum> AvailableLocales(bool includeDebug)
    {
        foreach (var locale in ConfigServer.Instance.GetAllInstancesOfType<Locale>().OrderBy(a => a.SortOrder))
        {
            if (!locale.IsDebugOnly || includeDebug)
            {
                yield return locale.UidAsEnum();
            }
        }
    }

    public LocalizedStringReference GetReferenceFromId(uint id)
    {
        return new LocalizedStringReference(id);
    }

    public LocalizedStringReference GetReferenceFromSlug(string slug)
    {
        return new LocalizedStringReference(GetId(slug));
    }

    public uint GetId(string slug)
    {
        return _idsAndSlugs.GetKeyFromValue(slug);
    }

    public string GetSlug(uint id)
    {
        return _idsAndSlugs.GetValueFromKey(id) ?? string.Empty;
    }

    public static void Clear()
    {
        Instance = new LocalizationServer();
    }

    public TranslatedString TranslatedFromId(LocalizationTableIds tableId, LocaleEnum locale,
        params object[] templateArguments)
    {
        return Instance.GetTranslatedStringFromId(new LocalizedStringReference((uint)tableId).Id, locale,
            templateArguments);
    }

    public TranslatedString TranslatedFromStringReference(LocalizedStringReference localeStringReference,
        LocaleEnum locale, params object[] templateArguments)
    {
        return Instance.GetTranslatedStringFromId(localeStringReference.Id, locale, templateArguments);
    }

    public TranslatedString Translated(string slug, LocaleEnum locale, params object[] templateArguments)
    {
        return Instance.GetTranslatedStringFromId(GetReferenceFromSlug(slug).Id, locale, templateArguments);
    }

    public TranslatedString GetTranslatedStringFromId(uint id, LocaleEnum locale, params object?[] templateArguments)
    {
        if (!HasId(id))
        {
            return new TranslatedString($"[{id}]", TranslationResult.NoSlug);
        }

        var slug = GetSlug(id);

        var result = _translations.GetValueOrDefault(locale)?.GetValueOrDefault(id);

        if (result == null)
        {
            var fallbackResult = _translations.GetValueOrDefault(FallbackLocale)?.GetValueOrDefault(id);

            if (fallbackResult != null)
            {
                return new TranslatedString(fallbackResult, TranslationResult.UsedFallback);
            }
            
            return new TranslatedString($"[{slug}]", TranslationResult.NoTranslations);
        }

        // If any of the params are LocalizedStringReferences, unpack them
        for (var i = 0; i < templateArguments.Length; i++)
        {
            if (templateArguments[i] is LocalizedStringReference localizedStringReference)
            {
                templateArguments[i] = localizedStringReference.Translated(locale);
            }

            if (templateArguments[i] == null)
            {
                templateArguments[i] = "null";
            }
        }

        if (templateArguments.Length > 0)
        {
            return new TranslatedString(string.Format(result, templateArguments), TranslationResult.Success);
        }

        return new TranslatedString(result, TranslationResult.Success);
    }

    private bool HasId(uint id)
    {
        return _idsAndSlugs.ContainsKey(id);
    }

    public string GetTranslatedStringFromSlug(string slug, LocaleEnum locale)
    {
        return GetTranslatedStringFromId(GetId(slug), locale);
    }


    public TranslationResult GetTranslationStatus(uint id, LocaleEnum locale)
    {
        if (!_idsAndSlugs.ContainsKey(id))
        {
            return TranslationResult.NoSlug;
        }

        var result = locale.ReadOrDefault().GetTranslationFromId(id);

        if (result == null)
        {
            return TranslationResult.NoTranslations;
        }

        return TranslationResult.Success;
    }

    public IEnumerable<uint> AllIds()
    {
        return _idsAndSlugs.Keys();
    }
}