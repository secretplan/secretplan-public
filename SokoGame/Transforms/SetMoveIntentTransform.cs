using SokoCore;

namespace SokoGame.Transforms;

public readonly record struct SetMoveIntentTransform(EntityId EntityId, CardinalDirection? Direction) : ITransform
{
    public Frame ApplyTo(Frame frame)
    {
        var entity = frame.GetEntity(EntityId);
        frame.SetEntity(EntityId, entity with { MoveIntent = entity.MoveIntent });
        return frame;
    }
}