using Godot;

namespace SecretPlanGodot.Core;

public class RemoteHost
{
    public bool IsLobbyPublic { get; set; }

    public void SendMessage(string message)
    {
        SendMessageRequested?.Invoke(message);
    }

    public event Action<string>? SendMessageRequested;

    public void SendBufferedCommand(Node node, string functionName, params Variant[] args)
    {
        SendBufferedCommandRequested?.Invoke(node, functionName, args);
    }

    public event Action<Node, string, Variant[]>? SendBufferedCommandRequested;

    /// <summary>
    ///     catch-all pipe for sending requests to the world
    /// </summary>
    public void DoArbitraryAction(List<string> list)
    {
        ArbitraryActionRequested?.Invoke(list);
    }

    public event Action<List<string>>? ArbitraryActionRequested;

    public void Leave(LeaveReason reason)
    {
        LeaveRequested?.Invoke(reason);
    }

    public event Action<LeaveReason>? LeaveRequested;

    public void KickPlayerByName(string playerNameQuery)
    {
        KickRequestedByName?.Invoke(playerNameQuery);
    }
    
    public void KickPlayerById(ulong steamId)
    {
        KickRequestedById?.Invoke(steamId);
    }

    public event Action<string>? KickRequestedByName;
    public event Action<ulong>? KickRequestedById;
}