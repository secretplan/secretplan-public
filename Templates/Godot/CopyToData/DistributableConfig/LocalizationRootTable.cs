using Newtonsoft.Json;
using SecretPlan.Generated;
using SecretPlanCore.Configuration;
using SecretPlanCore.Core;

namespace DATA_ASSEMBLY.DistributableConfig;

[SerializedTypeId("LocalizationRootTable")]
public class LocalizationRootTable : Config
{
    [JsonProperty("id_table")]
    public LocalizationIdTableEnum IdTable { get; init; }

    [JsonProperty("fallback_locale")]
    public LocaleEnum FallbackLocale { get; init; }

    /// <summary>
    ///     Used for editing entries, if you just want to read use one of the GetTranslation methods
    /// </summary>
    public EditableReference GetMutableReference(string slug)
    {
        if (IdTable.ReadOrDefault().HasSlug(slug))
        {
            return new EditableReference(IdTable.ReadOrDefault().GetId(slug), this);
        }

        return new EditableReference(IdTable.ReadOrDefault().AddNewSlug(slug), this);
    }

    public EditableReference GetMutableReference(uint id)
    {
        return new EditableReference(id, this);
    }

    public IEnumerable<uint> AllRelatedUids()
    {
        yield return Uid();

        foreach (var locale in LocalizationServer.Instance.AvailableLocales(true))
        {
            yield return (uint)locale;
        }

        yield return (uint)IdTable;
    }

    public void DebugClearAllEntries()
    {
        foreach (var id in IdTable.ReadOrDefault().AllIds())
        {
            RemoveEntry(id);
        }
    }

    private void RemoveEntry(uint id)
    {
        IdTable.ReadOrDefault().Remove(id);
        foreach (var locale in LocalizationServer.Instance.AvailableLocales(true))
        {
            locale.ReadOrDefault().Remove(id);
        }
    }

    public override GeneratedEnum<uint> CodeGenerateEnum()
    {
        var generatedEnum = new GeneratedEnum<uint>(InstanceInfo.ShortName() + "Ids");
        var idTable = IdTable.ReadOrDefault();
        foreach (var id in idTable.AllIds())
        {
            generatedEnum.AddEntry(idTable.GetSlug(id).Replace(".", "__").Replace("-", "_"), id,
                FallbackLocale.ReadOrDefault().GetTranslationFromId(id));
        }

        generatedEnum.AddEntry("None", 0);

        return generatedEnum;
    }

    public void Remove(uint id)
    {
        IdTable.ReadOrDefault().Remove(id);
        foreach (var locale in LocalizationServer.Instance.AvailableLocales(true))
        {
            locale.ReadOrDefault().Remove(id);
        }
    }

    public readonly record struct EditableReference(uint Id, LocalizationRootTable RootTable)
    {
        public EditableReference SetTranslation(LocaleEnum locale, string translation)
        {
            var availableLocales = LocalizationServer.Instance.AvailableLocales(true).ToList();
            var localeIndex = availableLocales.IndexOf(locale);
            if (availableLocales.IsValidIndex(localeIndex))
            {
                availableLocales[localeIndex].ReadOrDefault().SetTranslation(Id, translation);
            }

            return this;
        }

        public void SetSlug(string newSlug)
        {
            RootTable.IdTable.ReadOrDefault().SetSlug(Id, newSlug);
        }
    }
}