using Godot;
using Godot.Collections;

namespace SecretPlanGodot.Core;

public readonly struct RaycastResult3D
{
    public Rid Rid { get; }
    public Vector3 Position { get; }
    public Vector3 Normal { get; }
    public Variant FaceIndex { get; }
    public int ColliderId { get; }
    public GodotObject? FoundBody { get; }
    public int ShapeIndex { get; }
    public bool FoundSomething { get; }


    /// <summary>
    ///     Converts the (incredibly stupid, ill-conceived) dictionary raycast result into something we can actually use with
    ///     any amount of ergonomics.
    /// </summary>
    /// <param name="dictionary">The dictionary obtained from DirectSpaceState.IntersectsRay</param>
    public RaycastResult3D(Dictionary dictionary)
    {
        if (dictionary.Count == 0)
        {
            return;
        }

        FoundSomething = true;

        Position = (Vector3)dictionary["position"];
        Normal = (Vector3)dictionary["normal"];
        FaceIndex = dictionary["face_index"];

        // this might be the wrong type? expects an ObjectID
        ColliderId = (int)dictionary["collider_id"];
        FoundBody = (GodotObject)dictionary["collider"];
        ShapeIndex = (int)dictionary["shape"];
        Rid = (Rid)dictionary["rid"];
    }
}