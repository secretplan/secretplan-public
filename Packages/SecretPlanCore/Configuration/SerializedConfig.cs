using SecretPlanCore.Core;

namespace SecretPlanCore.Configuration;

/// <summary>
///     Represents an in-memory Config File that hasn't been written to disk yet
/// </summary>
public readonly record struct SerializedConfig(string FilePath, string JsonContent)
{
    public void WriteToFile(IFileSystem fileSystem)
    {
        fileSystem.WriteToFile(FilePath, JsonContent);
    }
}