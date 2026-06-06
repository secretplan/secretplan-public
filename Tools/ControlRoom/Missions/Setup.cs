using ControlRoom.Core;
using ControlRoom.Programs;

namespace ControlRoom.Missions;

public class Setup : Mission
{
    public Setup(List<string> rawArgs) : base(rawArgs)
    {
    }

    public override async Task Run()
    {
        await OutPipe.AgentLogMessage("Testing SteamCmd");
        if (!Platform.SerializedState.Shortcuts.ContainsKey("steamcmd"))
        {
            throw new MissionFailedException("No shortcut for SteamCmd" +
                                             "\n- Install Steamworks SDK: https://partner.steamgames.com/downloads/list" +
                                             $"\n- Add SteamCmd shortcut (hint: {Constants.GenerateMissionCommand<Shortcut>("add steamcmd C:/Path/To/SteamCmd.exe")})");
        }
        var steamCmd = new ProgramSteamCmd();
        await steamCmd.Login();
        if (!steamCmd.WasSuccessful())
        {
            await OutPipe.AgentLogWarning("Login failed, you may need to login to SteamCmd manually the first time");
        }

        await OutPipe.AgentLogMessage("Testing Godot");
        var godot = new ProgramGodot_4_5_0_Mono();
        var version = await godot.GetVersion();
        
        MissionAssert.IsTrue(version.StartsWith("4.5.stable.mono"), "Godot version didn't start with 4.5.stable.mono");
        
        if (!godot.WasSuccessful())
        {
            await OutPipe.AgentLogWarning("Something didn't run successfully when running godot, see above output");
        }

        await OutPipe.AgentLogMessage("Setup looks good!");
    }
}