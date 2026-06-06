using Newtonsoft.Json;

namespace SecretPlanCore.Telemetry;

/// <summary>
///     Represents a telemetry event that will be sent via HTTP request
/// </summary>
public class TelemetryRowUpload
{
    [JsonProperty("session_id")]
    public string SessionId { get; set; } = string.Empty;

    [JsonProperty("player_id")]
    public string PlayerId { get; set; } = string.Empty;

    [JsonProperty("branch")]
    public string Branch { get; set; } = string.Empty;

    [JsonProperty("event")]
    public string EventTypeId { get; set; } = string.Empty;

    [JsonProperty("payload")]
    public object? RawPayload { get; set; }
}