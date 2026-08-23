using SecretPlanCore.Configuration;

namespace FriendShapedPlugin.ConfigEditor;

public interface ICanNotifyOfCleaned
{
    /// <summary>
    ///     The config was dirtied and then cleaned
    /// </summary>
    public void OnNotifyOfClean();
}

public interface ICanInitializeFromConfig
{
    public bool Initialize(Config config);
}