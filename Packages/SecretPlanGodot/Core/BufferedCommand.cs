using Godot;

namespace SecretPlanGodot.Core;

public readonly record struct BufferedCommand(Node Runner, NodePath NodePath, string FunctionName, Variant[] Args)
{
    public bool CanRun()
    {
        return Runner.GetNodeOrNull(NodePath) != null;
    }

    public void Run()
    {
        LocalClient.Print($"Running buffered command {this}");
        Runner.GetNode(NodePath).Call(FunctionName, Args);
    }

    public override string ToString()
    {
        return $"{NodePath}.{FunctionName}({string.Join(",", Args)})";
    }
}