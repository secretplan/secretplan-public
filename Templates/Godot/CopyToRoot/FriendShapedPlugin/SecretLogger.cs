using Godot;
using Godot.Collections;
using SecretPlanGodot.Core;

namespace FriendShapedPlugin;

public partial class SecretLogger : Logger
{
    public override void _LogMessage(string message, bool error)
    {
        // we CANNOT call LocalClient.Print here because it will cause recursion
    }

    public override void _LogError(string function, string file, int line, string code, string rationale, bool editorNotify, int errorType,
        Array<ScriptBacktrace> scriptBacktraces)
    {
        var errorTypeName = errorType switch
        {
            0 => "ERROR",
            1 => "WARNING",
            2 => "SCRIPT",
            3 => "SHADER",
            _ => "UNKNOWN"
        };

        LocalClient.Error($"Caught {errorTypeName} at {file}:{line}\nMessage: {code}\nRationale: {rationale}");
        LocalClient.Print("Error Backtrace:");
        foreach (var item in scriptBacktraces)
        {
            if (item.IsEmpty())
            {
                continue;
            }

            LocalClient.Print(item.Format());
        }
    }
}