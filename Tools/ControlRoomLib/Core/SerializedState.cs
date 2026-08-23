using Newtonsoft.Json;

namespace ControlRoomLib.Core;

public class SerializedState
{
    [JsonProperty("shortcuts")]
    public readonly Dictionary<string, string> Shortcuts = new();

    [JsonProperty("mission_variables")]
    public readonly MissionVariables MissionVariables = new();
}