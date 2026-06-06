using Newtonsoft.Json;
using SecretPlanCore.Core;

namespace SecretPlanCore.Configuration;

/// <summary>
///     Represents the InstanceId and TypeId of a Config
/// </summary>
/// <param name="InstanceId">Unique ID of this config instance, zero if invalid</param>
/// <param name="TypeId">Unique ID of the type of this config instance, null if unknown</param>
[HideFromConfigEditor]
public record struct ConfigInstanceInfo(
    [property: JsonProperty("config_instance_id")] uint InstanceId,
    [property: JsonProperty("config_type_id")] string TypeId
)
{
    private readonly string? _instanceName;

    /// <summary>
    ///     Only gets written to at runtime. This is just here for runtime convenience so we know what file it came from.
    /// </summary>
    [JsonIgnore]
    public string Name
    {
        get => _instanceName ?? InstanceId.ToString();
        internal init => _instanceName = value;
    }

    public override string ToString()
    {
        return $"{Name} <{TypeId}> {InstanceId}";
    }

    public string ShortName()
    {
        var name = new FileInfo(Name).Name.RemoveFileExtension()
            .Replace("_", "")
            .Replace("-", "");
        if (name.StartsWith(TypeId))
        {
            name = name.Substring(TypeId.Length, name.Length - TypeId.Length);
        }
        
        if (char.IsNumber(name[0]))
        {
            name = "N" + name;
        }
        
        return name;
    }
}