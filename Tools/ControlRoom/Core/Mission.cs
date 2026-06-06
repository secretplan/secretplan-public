using ControlRoom.Missions;
using SecretPlanCore.ArgumentParsing;

namespace ControlRoom.Core;

public abstract class Mission
{
    protected Mission(List<string> rawArgs)
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
        MissionVariables = ArgumentBundle.Apply(Platform.SerializedState.MissionVariables with {}, rawArgs);
    }

    public MissionVariables MissionVariables { get; set; }

    public List<string> RawArgs { get; set; }

    protected PositionalArgumentList PositionalArgs { get; }

    public abstract Task Run();
}