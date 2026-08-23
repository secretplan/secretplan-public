using Newtonsoft.Json;

namespace SecretPlanCore.Core;

public class BaseSaveFile
{
    /// <summary>
    ///     This is saved to the save file so we continue to rotate backups appropriately.
    /// </summary>
    [JsonProperty("backup_index")]
    public uint BackupIndex { get; set; }
    
    [JsonProperty("telemetry_player_id")]
    public string TelemetryTrackingId { get; set; } = Guid.NewGuid().ToString();

    [JsonProperty("has_seen_telemetry_consent")]
    public bool HasFinishedTelemetryFlow { get; set; }
    
    public void IncrementBackupIndex()
    {
        BackupIndex++;
        BackupIndex %= 10;
    }
}