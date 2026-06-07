using System.Text;
using ControlRoom.Core;
using ControlRoom.Programs;
using SecretPlanCore.Core;

namespace ControlRoom.Missions;

public class NewProject : Mission
{
    public NewProject(List<string> rawArgs) : base(rawArgs)
    {
    }

    public override async Task Run()
    {
        var newAssemblyName = PositionalArgs.Get(0, "Assembly Name").ParseAsString();

        if (Directory.Exists(newAssemblyName))
        {
            throw new MissionFailedException($"Directory already exists: {newAssemblyName}");
        }

        var projectDirectory = new RealFileSystem(newAssemblyName);

        var copyToRootFiles = Constants.WorkingDirectoryFiles.GetDirectory("Templates/Godot/CopyToRoot");
        foreach (var path in copyToRootFiles.GetFilesAt("."))
        {
            var content = await copyToRootFiles.ReadFileAsync(path);
            await projectDirectory.WriteToFileAsync(path, content);
        }

        var defaultAssetsFiles = Constants.WorkingDirectoryFiles.GetDirectory("Templates/Godot/DefaultAssets");
        foreach (var path in defaultAssetsFiles.GetFilesAt("."))
        {
            var content = defaultAssetsFiles.ReadBytes(path);
            projectDirectory.WriteToFileBytes(Path.Join("DefaultAssets", path), content);
        }

        await ApplyAssemblyToTemplateAndCopy("project.godot", newAssemblyName);
        await ApplyAssemblyToTemplateAndCopy("export_presets.cfg", newAssemblyName);

        var sampleCsFile = new StringBuilder();
        sampleCsFile.AppendLine($"namespace {newAssemblyName};");
        sampleCsFile.AppendLine();
        sampleCsFile.AppendLine("public class Sample");
        sampleCsFile.AppendLine("{");
        sampleCsFile.AppendLine();
        sampleCsFile.AppendLine("}");

        await projectDirectory.WriteToFileAsync("Scripts/Sample.cs", sampleCsFile.ToString());

        VersionChange.WriteVersionFile(projectDirectory, new SecretPlanVersion());

        await OutPipe.AgentLogMessage("Please run Project > Tools > C# > Create C# Solution");

        var godot = new ProgramGodotLatest(newAssemblyName);
        await godot.Run("--editor");

        if (!projectDirectory.HasFile($"{newAssemblyName}.sln"))
        {
            throw new MissionFailedException($"Could not find {newAssemblyName}.sln");
        }

        if (!projectDirectory.HasFile($"{newAssemblyName}.csproj"))
        {
            throw new MissionFailedException($"Could not find {newAssemblyName}.csproj");
        }

        var dotnet = new ProgramDotnet(newAssemblyName);
        
        await dotnet.AddReferenceToCurrentProject("../Packages/ExTween/ExTween.csproj");
        await dotnet.AddProjectToCurrentSolution("../Packages/ExTween/ExTween.csproj");
        
        await dotnet.AddReferenceToCurrentProject("../Packages/SecretPlanCore/SecretPlanCore.csproj");
        await dotnet.AddProjectToCurrentSolution("../Packages/SecretPlanCore/SecretPlanCore.csproj");
        
        await dotnet.AddReferenceToCurrentProject("../Packages/SecretPlanGodot/SecretPlanGodot.csproj");
        await dotnet.AddProjectToCurrentSolution("../Packages/SecretPlanGodot/SecretPlanGodot.csproj");
        
        await godot.Run("--editor");
    }

    private static async Task ApplyAssemblyToTemplateAndCopy(string fileName, string newAssemblyName)
    {
        var projectDirectory = new RealFileSystem(newAssemblyName);
        var templateFiles = Constants.WorkingDirectoryFiles.GetDirectory("Templates/Godot");
        var template = await templateFiles.ReadFileAsync($"{fileName}.template");
        await projectDirectory.WriteToFileAsync($"{fileName}", template.Replace("%ASSEMBLY_NAME%", newAssemblyName));
    }
}
