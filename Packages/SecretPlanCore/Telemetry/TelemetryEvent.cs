using Newtonsoft.Json;
using SecretPlanCore.Configuration;

namespace SecretPlanCore.Telemetry;

public abstract class TelemetryEvent
{
    private string? _underlyingTypeId;

    /// <summary>
    ///     This doesn't need to be in the blob because we will get it from the packed entry.
    /// </summary>
    [JsonIgnore]
    public string TypeId => _underlyingTypeId ??= SerializedTypeIdAttribute.CalculateTypeId(GetType());

    public TelemetryRowUpload PackSelf(string playerId, string sessionId, string branch)
    {
        return new TelemetryRowUpload
        {
            EventTypeId = TypeId,
            PlayerId = playerId,
            SessionId = sessionId,
            Branch = branch,
            RawPayload = this
        };
    }
}