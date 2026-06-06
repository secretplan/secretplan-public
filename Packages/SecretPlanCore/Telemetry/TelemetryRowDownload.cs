using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SecretPlanCore.Telemetry;

/// <summary>
///     Represents a row in the Telemetry table that is downloaded (usually to ControlRoom)
/// </summary>
public class TelemetryRowDownload : TelemetryRowUpload
{
    [JsonProperty("created_at")]
    public DateTime CreatedAtTimeStamp { get; set; } = DateTime.UtcNow;
    
    [JsonProperty("id")]
    public string RowUniqueId { get; set; } = string.Empty;


    public TelemetryEvent? ReadPayload()
    {
        if (RawPayload is JObject jObject)
        {
            var unpacked = TelemetryServer.Instance.Unpack(EventTypeId, jObject);

            if (unpacked != null)
            {
                return unpacked;
            }
        }


        if (RawPayload is TelemetryEvent payload)
        {
            return payload;
        }

        return null;
    }
}