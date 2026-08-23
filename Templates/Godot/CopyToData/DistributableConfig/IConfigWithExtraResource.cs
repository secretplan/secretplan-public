using Godot;

namespace DATA_ASSEMBLY.DistributableConfig;

/// <summary>
///     Represents a config that might have generated some resources in-memory and needs to ensure they get de-allocated
///     when we shut down.
/// </summary>
public interface IConfigWithExtraResource
{
    public IEnumerable<Resource> GetOwnedResources();
}