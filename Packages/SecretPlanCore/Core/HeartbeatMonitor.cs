namespace SecretPlanCore.Core;

public class HeartbeatMonitor
{
    public enum MonitorMode
    {
        Uninitialized,

        /// <summary>
        ///     Waiting on initial message
        /// </summary>
        WaitingOnFirstMessage,

        /// <summary>
        /// </summary>
        Live
    }

    public const string LoadingScrimFlag = "waiting_for_heartbeats_from_all_players";
    private readonly Dictionary<ulong, HeartbeatMonitorEntry?> _idToLastHeardTime = new();
    private long _startTime;
    public MonitorMode Mode { get; private set; } = MonitorMode.Uninitialized;

    public void Add(ulong steamId)
    {
        _idToLastHeardTime[steamId] = null;
    }

    public void Update(ulong steamId, HeartbeatOrPingMessage message)
    {
        _idToLastHeardTime[steamId] = new HeartbeatMonitorEntry
        {
            ReceivedTime = TimeUtilities.TimeNowMilliseconds(),
            RemoteSentTime = message.SentTime
        };
    }

    public bool HasHeardFromEveryone()
    {
        if (Mode == MonitorMode.Uninitialized)
        {
            return false;
        }

        foreach (var timeStamp in _idToLastHeardTime.Values)
        {
            if (!timeStamp.HasValue)
            {
                return false;
            }
        }

        return true;
    }

    public void Start()
    {
        Mode = MonitorMode.WaitingOnFirstMessage;
        _startTime = TimeUtilities.TimeNowMilliseconds();
    }

    public long MillisecondsSinceStart()
    {
        return TimeUtilities.TimeNowMilliseconds() - _startTime;
    }

    public void SetToLive()
    {
        Mode = MonitorMode.Live;
    }

    public IEnumerable<ulong> ClientsNotHeardFrom()
    {
        foreach (var (id, lastHeardTime) in _idToLastHeardTime)
        {
            if (!lastHeardTime.HasValue)
            {
                yield return id;
            }
        }
    }

    public IEnumerable<(ulong, HeartbeatMonitorEntry?)> GetDebugInformation()
    {
        foreach (var x in _idToLastHeardTime)
        {
            yield return (x.Key, x.Value);
        }
    }

    public struct HeartbeatMonitorEntry
    {
        /// <summary>
        ///     The timestamp of when we got the message
        /// </summary>
        public long ReceivedTime { get; set; }

        /// <summary>
        ///     The time the message was sent according to the remote host (could be a totally different clock than ours)
        /// </summary>
        public long RemoteSentTime { get; set; }
    }

    public void Remove(ulong steamId)
    {
        _idToLastHeardTime.Remove(steamId);
    }

    public bool HasHeardFrom(ulong lobbyOwner)
    {
        return Mode != MonitorMode.Uninitialized && _idToLastHeardTime[lobbyOwner].HasValue;
    }
}