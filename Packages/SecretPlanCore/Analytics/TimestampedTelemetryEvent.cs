using SecretPlanCore.Telemetry;

namespace SecretPlanCore.Analytics;

public readonly record struct TimestampedTelemetryEvent(DateTime TimeStamp, TelemetryEvent TelemetryEvent);