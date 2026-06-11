using ControlRoom.Core;

namespace ControlRoom.Missions;

public class DownloadLatest : Mission
{
    public DownloadLatest(List<string> rawArgs) : base(rawArgs)
    {
    }

    public override async Task Run()
    {
        var sourceRepo =
            await VirtualRepo.GetAndInitialize("git@github.com:secretplan/secretplan-public.git", "VirtualPublicRepo");
        var destinationRepo =
            await VirtualRepo.GetAndInitialize();
        await CopyRepoCore.CopyCoreRepoContents(sourceRepo, destinationRepo);
    }
}