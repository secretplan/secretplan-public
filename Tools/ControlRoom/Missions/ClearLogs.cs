using ControlRoom.Core;
using SecretPlanCore.Core;

namespace ControlRoom.Missions;

public class ClearLogs : Mission
{
    public ClearLogs(List<string> rawArgs) : base(rawArgs)
    {
    }

    public override async Task Run()
    {
        var files = new RealFileSystem(Constants.LogsFolder);
        foreach (var file in files.GetFilesAt("."))
        {
            if (file != Constants.CurrentLogFile)
            {
                await OutPipe.AgentLogMessage($"Deleting {file}");
                files.DeleteFile(file);
            }
        }
    }
}