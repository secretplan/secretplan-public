namespace DATA_ASSEMBLY.DistributableConfig;

public interface ICanNotifyOfCleaned
{
    /// <summary>
    ///     The config was dirtied and then cleaned
    /// </summary>
    public void OnNotifyOfClean();
}