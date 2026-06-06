using ControlRoom.Core;

namespace ControlRoom.Programs;

public class ProgramDotnet : ExternalProgram
{
    public ProgramDotnet(string? workingDirectory = null) : base("dotnet", workingDirectory)
    {
    }

    public async Task CreateNewSolution(string fileNameWithoutExtension)
    {
        await Run($"new sln --name {fileNameWithoutExtension}");
    }

    public async Task CreateNewSolution()
    {
        await Run("new sln");
    }

    public async Task AddProjectToCurrentSolution(string pathToProject)
    {
        await Run($"sln add {pathToProject}");
    }

    public async Task New(string arg)
    {
        await Run($"new {arg}");
    }

    public async Task AddReferenceToCurrentProject(string referencePath)
    {
        await Run($"reference add {referencePath} --project .");
    }
}
