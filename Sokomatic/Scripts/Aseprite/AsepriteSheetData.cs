using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Sokomatic.Aseprite;

[Serializable]
public class AsepriteSheetData
{
    [JsonProperty("frames")]
    public Dictionary<string, AsepriteFrame> Frames { get; set; } = new();

    [JsonProperty("meta")]
    public AsepriteMetaData Meta { get; set; } = new();
}
