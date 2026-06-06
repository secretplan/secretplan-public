using Godot;

namespace SecretPlanGodot.Core;

public static class MultiplayerExtensions
{
    /// <summary>
    ///     Best-guess approximation of "is the multiplayer api active" since Godot doesn't give us this
    /// </summary>
    public static bool IsMultiplayerActive(this MultiplayerApi self)
    {
        var peer = self.MultiplayerPeer;
        return peer != null && peer.GetConnectionStatus() != MultiplayerPeer.ConnectionStatus.Disconnected;
    }
}