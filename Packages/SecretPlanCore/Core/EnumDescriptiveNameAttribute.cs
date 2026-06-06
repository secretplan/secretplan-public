namespace SecretPlanCore.Core;

/// <summary>
///     Put on the value of an enum to describe it
/// </summary>
[AttributeUsage(AttributeTargets.Field)]
public class EnumDescriptiveNameAttribute : Attribute
{
    private const string FailString = "???";

    public EnumDescriptiveNameAttribute(string nameLocStringKey, string? descriptionLocStringKey = null)
    {
        NameLocStringKey = nameLocStringKey;
        DescriptionLocStringKey = descriptionLocStringKey;
    }

    public string? DescriptionLocStringKey { get; }

    public string NameLocStringKey { get; }

    public static NameAndOptionalDescription GetNameAndDescriptionKeys<T>(T value) where T : Enum
    {
        var field = value.GetType().GetField(value.ToString());
        var x = field?
            .GetCustomAttributes(typeof(EnumDescriptiveNameAttribute), false)
            .Cast<EnumDescriptiveNameAttribute>()
            .FirstOrDefault();
        return new NameAndOptionalDescription(x
            ?.NameLocStringKey ?? Enum.GetName(typeof(T), value) ?? FailString, x?.DescriptionLocStringKey);
    }

    public static NameAndOptionalDescription GetNameAndDescriptionKeys(Type enumType, int enumValue)
    {
        if (!enumType.IsEnum)
        {
            return new NameAndOptionalDescription(FailString, null);
        }

        var name = Enum.GetName(enumType, enumValue);
        if (name is null)
        {
            return new NameAndOptionalDescription(FailString, null);
        }

        var field = enumType.GetField(name);
        if (field is null)
        {
            return new NameAndOptionalDescription(name, null);
        }

        var attr = (EnumDescriptiveNameAttribute?)field
            .GetCustomAttributes(typeof(EnumDescriptiveNameAttribute), false)
            .FirstOrDefault();

        return new NameAndOptionalDescription(attr?.NameLocStringKey ?? name, attr?.DescriptionLocStringKey);
    }
}