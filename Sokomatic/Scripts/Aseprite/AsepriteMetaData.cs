using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Sokomatic.Aseprite;

[Serializable]
public class AsepriteMetaData
{
    [JsonProperty("frameTags")]
    public List<AsepriteFrameTag> FrameTags { get; set; } = new();
}
