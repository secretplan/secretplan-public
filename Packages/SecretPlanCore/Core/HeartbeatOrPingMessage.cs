using System.Runtime.InteropServices;

namespace SecretPlanCore.Core;

[StructLayout(LayoutKind.Sequential)]
public struct HeartbeatOrPingMessage
{
    public SimpleNetworkMessageType MessageType { get; set; }

    /// <summary>
    ///     Sent time (from the sender's perspective)
    /// </summary>
    public long SentTime { get; set; }
    
    /// <summary>
    /// Arbitrary value. For Ping/Pong this is the "Ping ID"
    /// </summary>
    public long Id { get; set; }
}

public enum SimpleNetworkMessageType
{
    /// <summary>
    ///     Message forgot to assign a type
    /// </summary>
    Unknown = 0,

    /// <summary>
    ///     Heartbeat, just proof of life
    /// </summary>
    Heartbeat = 1,

    /// <summary>
    ///     Ping that expects a reply
    /// </summary>
    Ping = 2,

    /// <summary>
    ///     Ping that does not expect a reply
    /// </summary>
    Pong = 3
}