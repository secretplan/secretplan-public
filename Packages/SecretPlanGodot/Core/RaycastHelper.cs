using Godot;
using Godot.Collections;

namespace SecretPlanGodot.Core;

public readonly struct ShapeCastResult3D
{
    public Rid Rid { get; init; }
    public int ShapeIndex { get; init; }
    public GodotObject? FoundBody { get; init; }
    public Vector3 Normal { get; init; }
    public Vector3 CollisionPoint { get; init; }
    public bool FoundSomething { get; init; }
}

public readonly struct RaycastResult3D
{
    public Rid Rid { get; init; }
    public Vector3 HitPosition { get; init; }
    public Vector3 Normal { get; init; }
    public Variant FaceIndex { get; init; }
    public int ColliderId { get; init; }
    public GodotObject? FoundBody { get; init; }
    public int ShapeIndex { get; init; }
    public bool FoundSomething { get; init; }


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

        HitPosition = (Vector3)dictionary["position"];
        Normal = (Vector3)dictionary["normal"];
        FaceIndex = dictionary["face_index"];

        // this might be the wrong type? expects an ObjectID
        ColliderId = (int)dictionary["collider_id"];
        FoundBody = (GodotObject)dictionary["collider"];
        ShapeIndex = (int)dictionary["shape"];
        Rid = (Rid)dictionary["rid"];
    }
}