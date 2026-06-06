using SokoGame.World;

namespace SokoGame.Transforms;

public readonly record struct TransformFallIntoAbyss(EntityId EntityId) : ITransform
{
    public Frame ApplyTo(Frame frame)
    {
        // no-op, but we will animate this
        return frame;
    }

    public override string ToString()
    {
        return $"FALL_ABYSS {EntityId}";
    }
}