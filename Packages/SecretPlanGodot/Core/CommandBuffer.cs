using Godot;
using Godot.Collections;
using SecretPlanCore.Core;
using Array = Godot.Collections.Array;

namespace SecretPlanGodot.Core;

public class CommandBuffer
{
    private readonly List<BufferedCommand> _bufferedCommands = new();

    public void Poll()
    {
        var commandsToRemove = new List<BufferedCommand>();
        foreach (var command in _bufferedCommands)
        {
            if (command.CanRun())
            {
                command.Run();
                commandsToRemove.Add(command);
            }
        }

        foreach (var command in commandsToRemove)
        {
            _bufferedCommands.Remove(command);
        }
    }

    public void Add(Node runner, Array rawArgs)
    {
        var nodePath = rawArgs[0].As<NodePath>();
        rawArgs.RemoveAt(0);
        var functionName = rawArgs[0].AsString();
        rawArgs.RemoveAt(0);

        var command = new BufferedCommand(runner, nodePath, functionName, rawArgs.ToArray());

        if (command.CanRun())
        {
            command.Run();
        }
        else
        {
            LocalClient.Print($"Buffered command {command}");
            _bufferedCommands.Add(command);
        }
    }

    public static Array<Variant> ConstructCommandArgs(Node parent, Node node, string functionName, Variant[] args)
    {
        var nodePath = "";
        if (node.IsInsideTree())
        {
            nodePath = node.GetPath();
        }
        else
        {
            // makes the (big!) assumption that the node is a direct child of parent 
            nodePath = parent.GetPath() + "/" + node.Name;
        }

        // very intentionally must be a Godot array rather than a Regular Array
        var allArgs = new Array<Variant>
        {
            nodePath,
            functionName
        };
        allArgs.AddRange(args);
        return allArgs;
    }
}