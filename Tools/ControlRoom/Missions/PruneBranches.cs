using ControlRoom.Core;
using JetBrains.Annotations;

namespace ControlRoom.Missions;

[UsedImplicitly]
public class PruneBranches : Mission
{
    public PruneBranches(List<string> rawArgs) : base(rawArgs)
    {
    }

    public override async Task Run()
    {
        var virtualRepo = await VirtualRepo.GetAndInitialize();
        virtualRepo.Git.LogLevel = LogLevel.LogFileOnly;
        
        foreach (var branchName in await virtualRepo.Git.GetAllLocalBranchNames())
        {
            bool hasUpstream = await virtualRepo.Git.DoesBranchHaveUpstreamCounterpart(branchName);
            if (!hasUpstream)
            {
                await OutPipe.AgentLogMessage($"Deleting branch: {branchName}");
                await virtualRepo.Git.DeleteLocalBranch(branchName);
            }
        }
        
    }
}