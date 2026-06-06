using ControlRoom.Core;
using ControlRoom.Programs;
using SecretPlanCore.Core;

namespace ControlRoom.Missions;

public class AllSln : Mission
{
    private const string AllSlnFileNameWithoutExtension = "All";
    private const string AllSlnFileName = "All.sln";

    public AllSln(List<string> rawArgs) : base(rawArgs)
    {
    }

    public override async Task Run()
    {
        var localDirectory = new RealFileSystem("Local");

        await GenerateSolutionFileIfNotPresent(localDirectory);
        var projects= EnumerateAllCsProjFiles(localDirectory);
        await AddAllProjectsToSln(localDirectory, projects);
    }

    private async Task AddAllProjectsToSln(RealFileSystem localDirectory, IEnumerable<string> projects)
    {
        var dotnet = new ProgramDotnet(localDirectory.GetCurrentDirectory());
        foreach (var project in projects)
        {
            await dotnet.AddProjectToCurrentSolution(project);
        }
    }

    private IEnumerable<string> EnumerateAllCsProjFiles(RealFileSystem localDirectory)
    {
        foreach (var path in localDirectory.GetFilesAt("..", "csproj"))
        {
            if (path.Contains(".build"))
            {
                // Skip over things in build directories
                continue;
            }
            
            yield return path;
        }
    }

    private async Task GenerateSolutionFileIfNotPresent(RealFileSystem localDirectory)
    {
        var dotnet = new ProgramDotnet(localDirectory.GetCurrentDirectory());

        if (!localDirectory.HasFile(AllSlnFileName))
        {
            await dotnet.CreateNewSolution(AllSlnFileNameWithoutExtension);
        }
    }
}