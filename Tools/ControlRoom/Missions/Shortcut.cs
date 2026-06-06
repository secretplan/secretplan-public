using ControlRoom.Core;
using JetBrains.Annotations;

namespace ControlRoom.Missions;

[UsedImplicitly]
public class Shortcut : Mission
{
    public Shortcut(List<string> rawArgs) : base(rawArgs)
    {
    }

    public override async Task Run()
    {
        var commandType = PositionalArgs.Get(0, "Command").ParseAsSpecificString("add", "remove", "list");

        if (commandType == "add")
        {
            var key = PositionalArgs.Get(1, "Key").ParseAsString();
            var value = PositionalArgs.Get(2, "Value").ParseAsString();
            Platform.SerializedState.Shortcuts[key] = value;
            await OutPipe.AgentLogMessage($"Added shortcut [{key}] = {value}");
        }
        
        if (commandType == "remove")
        {
            var key = PositionalArgs.Get(1, "Key").ParseAsString();
            if (Platform.SerializedState.Shortcuts.Remove(key))
            {
                await OutPipe.AgentLogMessage($"Removed {key} successfully");
            }
            else
            {
                await OutPipe.AgentLogMessage($"{key} did not have a value, did nothing");
            }
        }

        if (commandType == "list")
        {
            foreach (var (existingKey, existingValue) in Platform.SerializedState.Shortcuts)
            {
                await OutPipe.AgentLogMessage($"[{existingKey}] = {existingValue}");
            }
        }
    }
}