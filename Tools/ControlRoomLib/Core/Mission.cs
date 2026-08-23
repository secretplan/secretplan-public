using SecretPlanCore.ArgumentParsing;

namespace ControlRoomLib.Core;

public abstract class Mission
{
    protected Mission(List<string> rawArgs, MissionVariables missionVariables)
    {
        var positionalArgs = new List<string>();
        foreach (var token in rawArgs)
        {
            if (!token.StartsWith("--"))
            {
                positionalArgs.Add(token);
            }
        }

        RawArgs = rawArgs;
        PositionalArgs = new PositionalArgumentList(positionalArgs);
        MissionVariables = ArgumentBundle.Apply(missionVariables with { }, rawArgs);
    }

    public MissionVariables MissionVariables { get; set; }

    public List<string> RawArgs { get; set; }

    protected PositionalArgumentList PositionalArgs { get; }

    public abstract Task Run();
}