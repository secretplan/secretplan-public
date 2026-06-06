using SecretPlanCore.ArgumentParsing;
using SecretPlanCore.Core;

namespace ControlRoom.Core;

public static class MissionDispatch
{
    public static async Task Execute(Mission mission)
    {
        var missionName = mission.GetType().Name;
        await OutPipe.AgentLogMessage($"-- MISSION START: {missionName} --");
        await OutPipe.AgentLogMessage($"-- {mission.MissionVariables} --");

        try
        {
            await mission.Run();
            await OutPipe.AgentLogMessage("-- MISSION COMPLETE --");
        }
        catch (Exception exception)
        {
            await OutPipe.AgentLogError("!! MISSION FAILED !!");

            if (exception is ArgParseFailedException consoleArgParseFailedException)
            {
                await OutPipe.AgentLogError($"Failed to parse argument {consoleArgParseFailedException.Argument}");
                await OutPipe.AgentLogError(consoleArgParseFailedException.Message);
            }
            else
            {
                await OutPipe.AgentLogError(exception.ToString());
                var innerException = exception.InnerException;
                if (innerException != null)
                {
                    await OutPipe.AgentLogError(innerException.ToString());
                }
            }
        }
    }

    public static async Task<Mission?> CreateMission(Type type, List<string> argsAsString)
    {
        try
        {
            return (Mission?)Activator.CreateInstance(type, argsAsString);
        }
        catch (Exception exception)
        {
            await OutPipe.AgentLogError($"Failed to create instance of {type.Name}\n{exception}");
        }

        return null;
    }
    
    
}