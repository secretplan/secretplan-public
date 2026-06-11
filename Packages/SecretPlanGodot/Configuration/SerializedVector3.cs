using Godot;
using Newtonsoft.Json;

namespace SecretPlanGodot.Configuration;

[CustomEditor("res://Scenes/ConfigEditor/Vector3Editor.tscn")]

public record struct SerializedVector3(
    [property: JsonProperty("x")] float X,
    [property: JsonProperty("y")] float Y,
    [property: JsonProperty("z")] float Z)
{
    public Vector3 ToVector3()
    {
        return new Vector3(X, Y, Z);
    }

    public static SerializedVector3 FromVector3(Vector3 vector)
    {
        return new SerializedVector3(vector.X, vector.Y, vector.Z);
    }
}