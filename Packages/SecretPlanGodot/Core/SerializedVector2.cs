using Godot;
using Newtonsoft.Json;
using SecretPlanGodot.Configuration;

namespace SecretPlanGodot.Core;

[CustomEditor("res://FriendShapedPlugin/ConfigEditor/Scenes/Vector3Editor.tscn")]
public record struct SerializedVector2(
    [property: JsonProperty("x")] float X,
    [property: JsonProperty("y")] float Y)
{
    public Vector2 ToVector2()
    {
        return new Vector2(X, Y);
    }

    public static SerializedVector2 FromVector2(Vector2 vector)
    {
        return new SerializedVector2(vector.X, vector.Y);
    }
}