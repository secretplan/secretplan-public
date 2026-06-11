using System.Text;
using ControlRoom.Core;
using SecretPlanCore.Core;

namespace ControlRoom.Missions;

public class CopyRepoCore : Mission
{
    public CopyRepoCore(List<string> rawArgs) : base(rawArgs)
    {
    }

    public override async Task Run()
    {
        var sourceRepoUrl = PositionalArgs.Get(0, "Source Repo ssh URL").ParseAsString();

        if (sourceRepoUrl == "self")
        {
            // null means "use own URL"
            sourceRepoUrl = null;
        }
        
        var sourceRepo = 
            await VirtualRepo.GetAndInitialize(sourceRepoUrl, "SourceRepo");
        
        var destinationRepo =
            await VirtualRepo.GetAndInitialize(PositionalArgs.Get(1, "Target Repo ssh URL").ParseAsString(), "TargetRepo");

        await CopyCoreRepoContents(sourceRepo, destinationRepo);
    }

    public static async Task CopyCoreRepoContents(VirtualRepo sourceRepo, VirtualRepo destinationRepo, Func<RealFileSystem, Task>? scrub = null)
    {
        string[] exactFilesToCopy =
        [
            ".gitattributes",
            ".gitignore",
            "controlroom",
            "ControlRoom.cmd",
            "Directory.Build.props"
        ];

        foreach (var filePath in exactFilesToCopy)
        {
            await CopyFile(sourceRepo.Files, destinationRepo.Files, filePath);
        }

        string[] directoriesToCopy =
        [
            "Packages",
            "Tools",
            "Templates"
        ];

        foreach (var directoryPath in directoriesToCopy)
        {
            await CopyDirectory(sourceRepo.Files.GetPathOfFile(directoryPath),
                destinationRepo.Files.GetPathOfFile(directoryPath));
        }

        string[] missionsToKeep =
        [
            nameof(AllSln),
            nameof(BuildGodotProject),
            nameof(CleanSandbox),
            nameof(ClearLogs),
            nameof(Echo),
            nameof(ListAllMissions),
            nameof(MissionVariablesEdit),
            nameof(NewProject),
            nameof(PatchNotes),
            nameof(PruneBranches),
            nameof(Shortcut),
            nameof(VersionChange),
            nameof(ConfigWorkbench),
            nameof(AsepriteAtlas),
            nameof(CopyRepoCore),
            nameof(DownloadLatest)
        ];

        var missionFiles = destinationRepo.Files.GetDirectory("Tools/ControlRoom/Missions");
        var missionFileNames = missionFiles.GetFilesAt(".");

        if (missionFileNames.Count == 0)
        {
            throw new MissionFailedException("Missions directory in the public repo is empty! Did it get moved?");
        }

        foreach (var fileName in missionFileNames)
        {
            var missionName = new FileInfo(fileName).Name.RemoveFileExtension();
            if (!missionsToKeep.Contains(missionName))
            {
                await OutPipe.AgentLogMessage($"Deleting mission: {missionName}");
                missionFiles.DeleteFile(fileName);
            }
        }

        if (scrub != null)
        {
            await scrub.Invoke(destinationRepo.Files);
        }

        await PushContents(destinationRepo, $"Copied {await sourceRepo.RepoUrl()}@{await sourceRepo.Git.CurrentSha()}");
    }

    private static async Task PushContents(VirtualRepo destinationRepo, string message)
    {
        await destinationRepo.Git.AddAll();

        if (await destinationRepo.Git.CountPendingFiles() == 0)
        {
            await OutPipe.AgentLogMessage("Up-to-date. Nothing to upload");
            return;
        }

        await destinationRepo.CommitAndPush(message);
    }

    /// <summary>
    ///     If the file contains a line that matches the forbidden keyword, remove it
    /// </summary>
    public static async Task ScrubLinesFromFile(RealFileSystem files, string filePath,
        string[] forbiddenKeywords)
    {
        var oldContentLines = (await files.ReadFileAsync(filePath)).SplitLines();
        var newContent = new StringBuilder();

        foreach (var line in oldContentLines)
        {
            var shouldSkip = false;
            foreach (var removeKeyword in forbiddenKeywords)
            {
                if (line.Contains(removeKeyword))
                {
                    shouldSkip = true;
                }
            }

            if (shouldSkip)
            {
                continue;
            }

            newContent.AppendLine(line);
        }

        await files.WriteToFileAsync(filePath, newContent.ToString());
    }


    public static async Task CopyDirectory(string sourceDirectory, string destinationDirectory)
    {
        var dir = new DirectoryInfo(sourceDirectory);

        await OutPipe.AgentLogMessage($"Copying directory {sourceDirectory} -> {destinationDirectory}");

        if (!dir.Exists)
        {
            throw new MissionFailedException($"Source not found: {dir.FullName}");
        }

        Directory.CreateDirectory(destinationDirectory);


        // Copy files
        foreach (var file in dir.GetFiles())
        {
            file.CopyTo(Path.Combine(destinationDirectory, file.Name), true);
        }

        // Recursive call for subdirectories
        foreach (var subDir in dir.GetDirectories())
        {
            await CopyDirectory(subDir.FullName, Path.Combine(destinationDirectory, subDir.Name));
        }
    }

    public static async Task CopyFile(RealFileSystem sourceFiles, RealFileSystem destinationFiles, string filePath)
    {
        var sourcePath = sourceFiles.GetPathOfFile(filePath);
        var destPath = destinationFiles.GetPathOfFile(filePath);
        var file = new FileInfo(sourcePath);

        await OutPipe.AgentLogMessage($"Copying file {sourcePath} -> {destPath}");
        file.CopyTo(destPath, true);
    }
}