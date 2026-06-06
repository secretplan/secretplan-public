using SecretPlanCore.Telemetry;

namespace BirdGameData.Analytics;

public readonly record struct TimestampedTelemetryEvent(DateTime TimeStamp, TelemetryEvent TelemetryEvent);