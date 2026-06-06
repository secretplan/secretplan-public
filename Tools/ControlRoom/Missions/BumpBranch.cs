using ControlRoom.Core;

namespace ControlRoom.Missions;

public class BumpBranch : Mission
{
    public BumpBranch(List<string> rawArgs) : base(rawArgs)
    {
    }

    public override async Task Run()
    {
        var branchName = PositionalArgs.Get(0, "Branch Name").ParseAsString();
        var gameDirectoryPath = PositionalArgs.Get(1, "Game Path").ParseAsString();
        var command = PositionalArgs.Get(2, "Command").ParseAsSpecificString("minor", "major", "patch");

        await OutPipe.AgentLogError("This task is not ready for use yet!");
        
        // UNFINISHED
        
        // var virtualRepo = new VirtualRepo();
        // await virtualRepo.Git.CheckoutAndPullBranch();
        // await VersionChange.BumpVersion(virtualRepo.Files, gameDirectoryPath, VersionChange.VersionBumpType.Patch);
    }

    public VersionChange.VersionBumpType GetBumpTypeFromString(string command)
    {
        switch (command)
        {
            case "major":
                return VersionChange.VersionBumpType.Major;
            case "minor":
                return VersionChange.VersionBumpType.Minor;
            case "patch":
                return VersionChange.VersionBumpType.Patch;
            default:
                throw new MissionFailedException($"Unable to parse bump type: {command}");
        }
    }
}