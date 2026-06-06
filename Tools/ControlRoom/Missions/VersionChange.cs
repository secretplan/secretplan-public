using ControlRoom.Core;
using ControlRoom.Programs;
using JetBrains.Annotations;
using Newtonsoft.Json;
using SecretPlanCore.Core;

namespace ControlRoom.Missions;

[UsedImplicitly]
public class VersionChange : Mission
{
    public enum VersionBumpType
    {
        Major,
        Minor,
        Patch
    }

    public VersionChange(List<string> rawArgs) : base(rawArgs)
    {
    }

    public override async Task Run()
    {
        var command = PositionalArgs.Get(0, "Command")
            .ParseAsSpecificString("create-empty", "bump-minor", "bump-major", "bump-patch");
        
        MissionAssert.MissionVariableNotNull(MissionVariables, nameof(MissionVariables.GameDirectory));
        
        var gameDirectoryPath = MissionVariables.GameDirectory!;

        MissionAssert.IsTrue(Constants.WorkingDirectoryFiles.HasDirectory(gameDirectoryPath),
            $"{gameDirectoryPath} is not a valid directory");
        
        var gameFiles = new RealFileSystem(gameDirectoryPath);
        
        if (command == "create-empty")
        {
            await OutPipe.AgentLogMessage($"Creating empty {Constants.VersionFile} in {gameDirectoryPath}");
            WriteVersionFile(gameFiles, new SecretPlanVersion());
            return;
        }

        await OutPipe.AgentLogMessage($"Bumping version for {gameDirectoryPath}");

        var newVersion = await GetVersionInDirectory(Constants.WorkingDirectoryFiles, gameDirectoryPath);
        var oldVersionNumber = newVersion.Version;

        switch (command)
        {
            case "bump-major":
                newVersion.BumpMajor();
                break;
            case "bump-minor":
                newVersion.BumpMinor();
                break;
            case "bump-patch":
                newVersion.BumpPatch();
                break;
        }

        WriteVersionFile(gameFiles, newVersion);
        await OutPipe.AgentLogMessage($"Bumped version from: {oldVersionNumber} -> {newVersion.Version}");

        await CommitAndPushIfRequested(gameDirectoryPath, newVersion);
    }

    private static async Task CommitAndPushIfRequested(string gameDirectoryPath, SecretPlanVersion newVersion)
    {
        await OutPipe.AgentLogMessage("You must commit and push the bumped version file before you build.");
        var git = new ProgramGit(Environment.CurrentDirectory);
        var numberOfChangedFiles = await git.CountPendingFiles();
        var response = await OutPipe.AgentPrompt(
            $"Do you want me to commit and push for you (will push {numberOfChangedFiles} files)?", "no");

        if (response != null && response.StartsWith("y"))
        {
            await git.CommitAll($"Bump version {gameDirectoryPath} {newVersion.Version}");
            await git.PushNormal();
        }
        else
        {
            await OutPipe.AgentLogMessage("DO NOT FORGET TO PUSH THIS BEFORE YOU BUILD!");
        }
    }

    public static async Task<SecretPlanVersion> GetVersionInDirectory(RealFileSystem files,
        string gameDirectoryPath)
    {
        var gameFiles = (RealFileSystem)files.GetDirectory(gameDirectoryPath);
        MissionAssert.IsTrue(gameFiles.HasFile(Constants.VersionFile),
            $"Expected to find {Constants.VersionFile} in {gameFiles.GetCurrentDirectory()}");

        var versionJson = await gameFiles.ReadFileAsync(Constants.VersionFile);
        var newVersion = await Constants.JsonDeserializeSafe<SecretPlanVersion>(versionJson);
        MissionAssert.IsNotNull(newVersion, $"Failed to parse {gameFiles.GetCurrentDirectory()}/{Constants.VersionFile}");
        return newVersion;
    }
    
    public static string GetPathToVersionFile(RealFileSystem files, string gameDirectoryPath)
    {
        var gameFiles = (RealFileSystem)files.GetDirectory(gameDirectoryPath);
        MissionAssert.IsTrue(gameFiles.HasFile(Constants.VersionFile), $"Expected to find {Constants.VersionFile} in {gameFiles.GetCurrentDirectory()}");

        return Path.Join(gameFiles.GetCurrentDirectory(), Constants.VersionFile);
    }

    public static void WriteVersionFile(RealFileSystem gameFiles, SecretPlanVersion version)
    {
        var json = JsonConvert.SerializeObject(version, Formatting.Indented);
        gameFiles.WriteToFile(Constants.VersionFile, json);
    }

    public static async Task<SecretPlanVersion> Stamp(RealFileSystem virtualRepoFiles, string gameDirectoryPath)
    {
        await OutPipe.AgentLogMessage($"Stamping {Constants.VersionFile} with git info");
        var git = new ProgramGit(virtualRepoFiles.GetCurrentDirectory());
        MissionAssert.IsTrue(virtualRepoFiles.HasDirectory(gameDirectoryPath),
            $"Could not find game directory {virtualRepoFiles.ToAbsolutePath(gameDirectoryPath)}");

        var gameFiles = (RealFileSystem)virtualRepoFiles.GetDirectory(gameDirectoryPath);
        var sha = await git.CurrentSha();
        var branch = await git.CurrentBranch();
        var existingVersion = await GetVersionInDirectory(virtualRepoFiles, gameDirectoryPath);
        existingVersion.StampGitInfo(branch, sha);
        WriteVersionFile(gameFiles, existingVersion);
        return existingVersion;
    }

    public static async Task BumpVersion(RealFileSystem virtualRepoFiles, string gameDirectoryPath,
        VersionBumpType versionBumpType)
    {
        await OutPipe.AgentLogMessage($"Bumping {Constants.VersionFile}");
        MissionAssert.IsTrue(virtualRepoFiles.HasDirectory(gameDirectoryPath),
            $"Could not find game directory {virtualRepoFiles.ToAbsolutePath(gameDirectoryPath)}");

        var gameFiles = (RealFileSystem)virtualRepoFiles.GetDirectory(gameDirectoryPath);
        var existingVersion = await GetVersionInDirectory(virtualRepoFiles, gameDirectoryPath);

        switch (versionBumpType)
        {
            case VersionBumpType.Major:
                existingVersion.BumpMajor();
                break;
            case VersionBumpType.Minor:
                existingVersion.BumpMinor();
                break;
            case VersionBumpType.Patch:
                existingVersion.BumpPatch();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(versionBumpType), versionBumpType, null);
        }

        WriteVersionFile(gameFiles, existingVersion);
    }
}