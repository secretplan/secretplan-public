using SokoGame.World;

namespace SokoGame.Transforms;

public readonly record struct TransformBecomeFloating(EntityId EntityId) : ITransform
{
    public Frame ApplyTo(Frame frame)
    {
        // technically a no-op, animation system will use this to play a sound and set the object to a sinking animation
        return frame;
    }

    public override string ToString()
    {
        return $"FLOAT {EntityId}";
    }
}