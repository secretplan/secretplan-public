using Godot;
using Newtonsoft.Json;
using SecretPlanGodot.Configuration;

namespace SecretPlanGodot.Core;

[CustomEditor("res://FriendShapedPlugin/ConfigEditor/Scenes/ColorEditor.tscn")]
public record struct SerializedColorRgba(
    [property: JsonProperty("red")] float Red,
    [property: JsonProperty("green")] float Green,
    [property: JsonProperty("blue")] float Blue,
    [property: JsonProperty("alpha")] float Alpha) : ISerializedColor
{
    public static readonly SerializedColorRgba White = new(1, 1, 1, 1);

    public Color ToColor()
    {
        return new Color(Red, Green, Blue, Alpha);
    }

    public static SerializedColorRgba FromColor(Color color)
    {
        return new SerializedColorRgba(color.R, color.G, color.B, color.A);
    }
}