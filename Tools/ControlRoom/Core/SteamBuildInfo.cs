namespace ControlRoom.Core;

public readonly record struct SteamBuildInfo(long AppId, string BranchToForkFrom, SteamDepotInfo[] Depots)
{
    public IEnumerable<SteamUploadSku> UploadSkus()
    {
        foreach (var depot in Depots)
        {
            yield return new SteamUploadSku(AppId, depot.DepotId, depot.OperatingSystem);
        }
    }
}

public readonly record struct SteamDepotInfo(long DepotId, BuildTarget OperatingSystem);

public readonly record struct SteamUploadSku(long AppId, long DepotId, BuildTarget OperatingSystem)
{
    public override string ToString()
    {
        return $"Depot: {DepotId} for {OperatingSystem}";
    }
}