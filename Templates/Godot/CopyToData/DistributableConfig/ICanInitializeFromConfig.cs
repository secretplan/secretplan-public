using SecretPlanCore.Configuration;

namespace DATA_ASSEMBLY.DistributableConfig;

public interface ICanInitializeFromConfig
{
    public bool Initialize(Config config);
}