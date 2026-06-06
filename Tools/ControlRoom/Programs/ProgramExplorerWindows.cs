using ControlRoom.Core;

namespace ControlRoom.Programs;

public class ProgramExplorerWindows : ExternalProgram, IFileExplorerProgram
{
    public ProgramExplorerWindows() : base("explorer", null)
    {
        MissionAssert.HostOperatingSystemIs(HostOperatingSystem.Windows);
    }

    public async Task Open(string path)
    {
        await Run(path);
    }
}