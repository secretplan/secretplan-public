using SokoCore;
using SokoGame.World;

namespace SokoGame.Transforms;

public readonly record struct SetNudgeIntentTransform(EntityId EntityId, CardinalDirection? DesiredMoveIntent)
    : ITransform
{
    public Frame ApplyTo(Frame frame)
    {
        // not implemented yet
        var entity = frame.GetEntity(EntityId);
        // frame.SetEntity(EntityId, entity with { MoveIntent = DesiredMoveIntent });
        return frame;
    }

    public override string ToString()
    {
        return $"SET_NUDGE_INTENT {EntityId} {DesiredMoveIntent?.ToString() ?? "null"}";
    }
}