using System;
using Newtonsoft.Json;

namespace Sokomatic.Aseprite;

[Serializable]
public struct AsepriteSize
{
    [JsonProperty("w")]
    public int Width { get; set; }

    [JsonProperty("h")]
    public int Height { get; set; }
}