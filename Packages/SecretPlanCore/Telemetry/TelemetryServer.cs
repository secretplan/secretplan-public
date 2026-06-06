using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SecretPlanCore.Configuration;
using SecretPlanCore.Core;

namespace SecretPlanCore.Telemetry;

public class TelemetryServer
{
    private readonly Dictionary<string, Type> _typeIdToTypeCache = new();
    private readonly Dictionary<Type, string?> _typeToTypeIdCache = new();

    public TelemetryServer()
    {
        CalculateAllTypeIds();
    }

    public static TelemetryServer Instance { get; private set; } = new();

    public void CalculateAllTypeIds()
    {
        foreach (var telemetryEventType in Reflection.GetAllTypesThatDeriveFrom<TelemetryEvent>())
        {
            var typeId = SerializedTypeIdAttribute.CalculateTypeId(telemetryEventType);
            _typeToTypeIdCache[telemetryEventType] = typeId;
            _typeIdToTypeCache[typeId] = telemetryEventType;
        }
    }

    public string? GetTypeId<TEvent>() where TEvent : TelemetryEvent
    {
        var type = typeof(TEvent);
        if (_typeToTypeIdCache.TryGetValue(type, out var typeId))
        {
            return typeId;
        }

        _typeToTypeIdCache[type] = SerializedTypeIdAttribute.CalculateTypeId(type);

        return _typeToTypeIdCache.GetValueOrDefault(type);
    }

    public Type? GetTypeFromId(string eventTypeId)
    {
        return _typeIdToTypeCache.GetValueOrDefault(eventTypeId);
    }

    public TelemetryEvent? Unpack(string eventTypeId, JObject entryPayload)
    {
        var type = GetTypeFromId(eventTypeId);
        if (type != null)
        {
            return entryPayload.ToObject(type) as TelemetryEvent;
        }

        return null;
    }

    public TelemetryEvent? Unpack(string eventTypeId, string jsonString)
    {
        var type = GetTypeFromId(eventTypeId);
        if (type != null)
        {
            try
            {
                return JsonConvert.DeserializeObject(jsonString, type) as TelemetryEvent;
            }
            catch
            {
                return (TelemetryEvent?)Activator.CreateInstance(type);
            }
        }

        return null;
    }
}