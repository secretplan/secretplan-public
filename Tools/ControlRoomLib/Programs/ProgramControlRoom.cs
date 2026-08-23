using ControlRoomLib.Core;

namespace ControlRoomLib.Programs;

/// <summary>
///     For ControlRoom missions in another instance of the monorepo. I don't know if we'll ever need to do this, but I
///     tested it, and it does, in fact, work
/// </summary>
public class ProgramControlRoom : ExternalProgram
{
    public ProgramControlRoom(string repoRoot) : base("dotnet", repoRoot)
    {
    }

    public async Task RunMission<T>(params string[] missionArgs) where T : Mission
    {
        List<string> fullArgs = ["run", "--project", "Tools/ControlRoom", "--", typeof(T).Name];
        fullArgs.AddRange(missionArgs);
        await Run(fullArgs.ToArray());
    }
}