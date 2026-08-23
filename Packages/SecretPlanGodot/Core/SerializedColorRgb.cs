using Godot;
using Newtonsoft.Json;
using SecretPlanGodot.Configuration;

namespace SecretPlanGodot.Core;

[CustomEditor("res://FriendShapedPlugin/ConfigEditor/Scenes/ColorEditor.tscn")]
public record struct SerializedColorRgb(
    [property: JsonProperty("red")] float Red,
    [property: JsonProperty("green")] float Green,
    [property: JsonProperty("blue")] float Blue) : ISerializedColor
{
    public static readonly SerializedColorRgb White = new(1, 1, 1);

    public Color ToColor()
    {
        return new Color(Red, Green, Blue);
    }

    public static SerializedColorRgb FromColor(Color color)
    {
        return new SerializedColorRgb(color.R, color.G, color.B);
    }
}