using SecretPlan.Generated;

namespace DATA_ASSEMBLY.Distributable;

public class DynamicSettingAttribute : Attribute
{
    public DynamicSettingAttribute(
        SettingsCategoryType category,
        LocalizationTableIds localizationKey,
        LocalizationTableIds description = LocalizationTableIds.None, bool shouldDisplayBeforeFields = false
        )
    {
        ShouldDisplayBeforeFields = shouldDisplayBeforeFields;
        Name = localizationKey;
        Description = description;
        Category = category;
    }

    /// <summary>
    ///     If this attribute is on a method, it will be displayed after the fields unless this property is true
    /// </summary>
    public bool ShouldDisplayBeforeFields { get; }

    public LocalizationTableIds Name { get; }

    public LocalizationTableIds Description { get; }

    public SettingsCategoryType Category { get; }
}

