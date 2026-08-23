using ControlRoomLib.Core;

namespace ControlRoomLib.Programs;

public class ProgramLnUnix : ExternalProgram
{
    public ProgramLnUnix(string? workingDirectory) : base("ln", workingDirectory)
    {
        MissionAssert.HostOperatingSystemIsUnix();
    }

    public async void CreateSoftLink(string from, string to)
    {
        await Run($"-s \"{from}\" \"{to}\"");
    }
}