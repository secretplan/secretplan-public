using System.Reflection;
using ControlRoom.Missions;
using ControlRoomLib.Core;
using ControlRoomLib.Programs;
using SecretPlanCore.ArgumentParsing;
using SecretPlanCore.Core;


// -------------------------------------------------------------- //
Platform.Startup();

if (await ControlRoomConstants.CheckLocalIsLatest())
{
    var remainingArgs = new List<string>();
    remainingArgs.AddRange(args);

    var missionVariables = ArgumentBundle.Apply(Platform.SerializedState.MissionVariables with { }, remainingArgs);
    var gameDirectory = missionVariables.GameDirectory;
    if (gameDirectory != null)
    {
        var dataAssemblyName = $"{gameDirectory}Data";
        await OutPipe.AgentLogMessage($"Building project: {dataAssemblyName}");
        var dotnet = new ProgramDotnet();
        await dotnet.BuildProjectAtPath($"./Data/{dataAssemblyName}");

        if (dotnet.MostRecentExitCode != 0)
        {
            throw new MissionFailedException("Could not build");
        }

        await OutPipe.AgentLogMessage($"Loading assembly: {dataAssemblyName}");
        Assembly.LoadFrom($"./Data/{dataAssemblyName}/bin/Debug/net8.0/{dataAssemblyName}.dll");
    }

    if (!remainingArgs.IsValidIndex(0))
    {
        await Platform.AttemptToRunMissionWithArgs([nameof(Repl)]);
    }
    else
    {
        await Platform.AttemptToRunMissionWithArgs(remainingArgs);
    }
}

Platform.Shutdown();

// -------------------------------------------------------------- //

