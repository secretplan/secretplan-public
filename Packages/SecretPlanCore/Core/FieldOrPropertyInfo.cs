using System.Reflection;

namespace SecretPlanCore.Core;

public readonly record struct FieldOrPropertyInfo(MemberInfo MemberInfo)
{
    public PropertyInfo? PropertyInfo => MemberInfo as PropertyInfo;
    public FieldInfo? FieldInfo => MemberInfo as FieldInfo;

    public Type? AssociatedType()
    {
        return PropertyInfo?.PropertyType ?? FieldInfo?.FieldType;
    }

    public object? GetValue(object instance)
    {
        return PropertyInfo?.GetValue(instance) ?? FieldInfo?.GetValue(instance);
    }

    public void SetValue(object instance, object? value)
    {
        PropertyInfo?.SetValue(instance, value);
        FieldInfo?.SetValue(instance, value);
    }
}