using SokoCore;
using SokoGame.World;

namespace SokoGame.Transforms;

public readonly record struct TransformSetMoveIntent(EntityId EntityId, CardinalDirection? DesiredMoveIntent)
    : ITransform
{
    public Frame ApplyTo(Frame frame)
    {
        var entity = frame.GetEntity(EntityId);
        frame.SetEntity(EntityId, entity with { MoveIntent = DesiredMoveIntent });
        return frame;
    }

    public override string ToString()
    {
        return $"SET_MOVE_INTENT {EntityId} {DesiredMoveIntent?.ToString() ?? "null"}";
    }
}