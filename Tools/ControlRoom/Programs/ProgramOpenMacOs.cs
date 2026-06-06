using ControlRoom.Core;

namespace ControlRoom.Programs;

public class ProgramOpenMacOs : ExternalProgram, IFileExplorerProgram
{
    public ProgramOpenMacOs() : base("open", null)
    {
        MissionAssert.HostOperatingSystemIs(HostOperatingSystem.MacOs);
    }

    public async Task Open(string path)
    {
        await Run(path);
    }
}