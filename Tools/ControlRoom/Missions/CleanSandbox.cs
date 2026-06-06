using ControlRoom.Core;
using JetBrains.Annotations;
using SecretPlanCore.Core;

namespace ControlRoom.Missions;

[UsedImplicitly]
public class CleanSandbox : Mission
{
    public CleanSandbox(List<string> rawArgs) : base(rawArgs)
    {
    }

    public override async Task Run()
    {
        var sandboxFiles = new RealFileSystem(Environment.CurrentDirectory);
        var answer = await OutPipe.AgentPrompt($"This is going to attempt to delete your {Constants.SandboxPath}, are you sure?", "no");

        if (answer != null && answer.StartsWith("y"))
        {
            await OutPipe.AgentLogMessage($"Deleting all files at {Constants.SandboxPath}");
            await OutPipe.AgentLogMessage("If this fails, you might need to delete the files manually.");
            sandboxFiles.DeleteDirectory(Constants.SandboxPath, true);
        }
    }
}