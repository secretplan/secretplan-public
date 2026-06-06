using SokoCore;
using SokoGame.World;

namespace SokoGame.Transforms;

public readonly record struct TransformMoveEntityInCardinalDirection(EntityId EntityId, CardinalDirection Direction) : ITransform
{
    public Frame ApplyTo(Frame frame)
    {
        var entity = frame.GetEntity(EntityId);
        frame.SetEntity(EntityId, entity with { Position = entity.Position + Direction });
        return frame;
    }

    public override string ToString()
    {
        return $"MOVE {EntityId} {Direction}";
    }
}