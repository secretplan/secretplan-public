using ControlRoom.Core;
using JetBrains.Annotations;

namespace ControlRoom.Missions;

[UsedImplicitly]
public class Echo : Mission
{
    public Echo(List<string> rawArgs) : base(rawArgs)
    {
    }

    public override async Task Run()
    {
        var message = PositionalArgs.Get(0, "message").ParseAsString();
        await OutPipe.AgentLogMessage(message);
    }
}