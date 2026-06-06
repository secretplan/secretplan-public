using Godot;

namespace SecretPlanGodot.Configuration;

public interface ISerializedColor
{
    float Red { get; }
    float Green { get; }
    float Blue { get; }
    Color ToColor();
}