using ExTween;
using SokoCore;
using SokoGame.Animation;
using SokoGame.World;

namespace SokoGame.Transforms;

public readonly record struct TransformSetNudgeIntent(EntityId EntityId, CardinalDirection? DesiredNudgeIntent)
    : ITransform
{
    public Frame ApplyTo(Frame frame)
    {
        // not implemented yet
        var entity = frame.GetEntity(EntityId);
        frame.SetEntity(EntityId, entity with { NudgeIntent = DesiredNudgeIntent });
        return frame;
    }

    public void BuildAnimation(MultiplexTween tween, EntityViewTable table)
    {
    }

    public override string ToString()
    {
        return $"SET_NUDGE_INTENT {EntityId} {DesiredNudgeIntent?.ToString() ?? "null"}";
    }
}