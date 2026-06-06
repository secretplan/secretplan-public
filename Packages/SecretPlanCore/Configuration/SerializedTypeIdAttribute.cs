namespace SecretPlanCore.Configuration;

/// <summary>
///     Used for Config and Telemetry Events
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class SerializedTypeIdAttribute : Attribute
{
    public SerializedTypeIdAttribute(string name)
    {
        Name = name;
    }

    public string Name { get; }
    
    public static string CalculateTypeId(Type type)
    {
        foreach (var attribute in type.GetCustomAttributes(false))
        {
            if (attribute is SerializedTypeIdAttribute forceTypeIdAttribute)
            {
                return forceTypeIdAttribute.Name;
            }
        }

        return type.Name;
    }
}