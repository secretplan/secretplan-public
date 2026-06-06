using JetBrains.Annotations;

namespace SecretPlanCore.Configuration;

/// <summary>
///     Represents a config of unknown type, once loaded, we can read its TypeId to figure out what type it actually is.
/// </summary>
[UsedImplicitly]
public class UnknownConfig : Config;