using ControlRoom.Core;

namespace ControlRoom.Missions;

public class DownloadLatest : Mission
{
    public DownloadLatest(List<string> rawArgs) : base(rawArgs)
    {
    }

    public override async Task Run()
    {
        var url = "git@github.com:secretplan/secretplan-public.git";
        var sourceRepo =
            await VirtualRepo.GetAndInitialize(url, "VirtualPublicRepo");
        var destinationRepo =
            await VirtualRepo.GetAndInitialize();
        await CopyRepoCore.CopyCoreRepoContents(sourceRepo, destinationRepo);
    }
}