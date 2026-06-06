using ControlRoom.Missions;
using ControlRoom.Programs;
using SecretPlanCore.Core;

namespace ControlRoom.Core;

public class VirtualRepo
{
    public readonly RealFileSystem Files = new(Constants.VirtualRepoFullPath);
    public readonly ProgramGit Git = new(Constants.VirtualRepoFullPath);

    private VirtualRepo()
    {
    }

    public static async Task<VirtualRepo> GetAndInitialize()
    {
        var repo = new VirtualRepo();
        await repo.CloneOrReset();
        return repo;
    }

    /// <summary>
    ///     Clones the Virtual Repo and/or resets it into a vanilla state
    /// </summary>
    private async Task CloneOrReset()
    {
        Git.LogLevel = LogLevel.LogFileOnly;
        // This git lives in the "sandbox" aka: `.build` 
        var outerGit = new ProgramGit(Constants.SandboxPath);
        outerGit.LogLevel = LogLevel.LogFileOnly;
        await outerGit.CloneWithSsh(Constants.MonorepoSshUrl, Constants.VirtualRepoLocalPath);
        outerGit.LogLevel = LogLevel.ConsoleAndLogFile;
        if (!outerGit.WasSuccessful())
        {
            await VirtualRepoLog("Looks like the repo already exists, we will reuse it");
        }


        var originUrl = await Git.GetOriginUrl();
        MissionAssert.AreEqual(originUrl, Constants.MonorepoSshUrl,
            $"Virtual repo has wrong remote URL, run `{Constants.GenerateMissionCommand<CleanSandbox>()}`");

        await VirtualRepoLog("Setting up repo");

        await VirtualRepoLog("Attempting to abort any current state");
        await Git.ResetAndAbort();

        await VirtualRepoLog("Checking out main");
        await Git.CheckoutBranch("main");

        await VirtualRepoLog("Resetting to origin");
        await Git.ResetToOrigin();

        await VirtualRepoLog("Attempting to abort any current state (again, for safety)");
        await Git.ResetAndAbort();

        await VirtualRepoLog("Pulling latest");
        await Git.Pull();

        Git.LogLevel = LogLevel.ConsoleAndLogFile;
        await VirtualRepoLog("Ready!");
    }

    public RealFileSystem GetSubdirectory(string directory)
    {
        return (RealFileSystem)Files.GetDirectory(directory);
    }

    public async Task CommitAndPush(string commitMessage)
    {
        await VirtualRepoLog($"Committing everything with message: {commitMessage}");
        await Git.CommitAll(commitMessage);
        await Git.PushSetUpstream();
    }

    public async Task CheckoutNewBranchAndPush(string releaseBranchName)
    {
        await VirtualRepoLog($"Checking out and pushing: {releaseBranchName}");
        await Git.CheckoutNewBranch(releaseBranchName);
        await Git.PushSetUpstream();
    }

    private static Task VirtualRepoLog(string message)
    {
        return OutPipe.AgentLogMessage($"VirtualRepo: {message}");
    }

    public FileInfo BuildAndDumpVdf(SteamUploadSku uploadSku, DirectoryInfo contentRoot, string description)
    {
        var buildVdfLocalPath = "build.vdf";
        Files.WriteToFile(buildVdfLocalPath,
            ProgramSteamCmd.GenerateBuildVdf(uploadSku, contentRoot.FullName, description));

        return new FileInfo(Path.Join(Files.GetCurrentDirectory(), buildVdfLocalPath));
    }
}