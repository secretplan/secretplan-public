using Godot;
using SecretPlanGodot.Core;

namespace FriendShapedDistributable;

public class ConsoleDebug : SecretPlanDebugger
{
    private readonly GameCore _gameCoreNode;

    public ConsoleDebug(GameCore gameCoreNode)
    {
        _gameCoreNode = gameCoreNode;
    }

    [DebugValue("test")]
    public void Test()
    {
        LocalClient.Print("Hello world!");
    }
}